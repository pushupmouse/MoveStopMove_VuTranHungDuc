using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private WeaponSO weaponSO;

    private GameState gameState;
    private UserData userData;
    private int coinsGained = 0;
    public Action OnWeaponChanged;

    public UserData UserData { get => userData; set => userData = value; }

    public enum GameState
    {
        MainMenu = 0,
        Gameplay = 1,
        WeaponSelect = 2,
        Pause = 3,
        GameOver = 4,
    }

    private void Awake()
    {
        GetUserData();
        ChangeState(GameState.MainMenu);
        OnInit();
    }

    private void Start()
    {
        Unsubscribe();
        Subscribe();
    }

    private void OnInit()
    {
        coinsGained = 0;
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
    }

    public bool IsState(GameState state)
    {
        return gameState == state;
    }

    private void GetUserData()
    {
        if (DataManager.Instance.HasData<UserData>())
        {
            userData = DataManager.Instance.LoadData<UserData>();
        }
        else
        {
            userData = new UserData();
            DataManager.Instance.SaveData(userData);
        }
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        userData.equippedWeapon = (int) weaponType;
        DataManager.Instance.SaveData(userData);
        OnWeaponChanged?.Invoke();
    }

    public void BuyWeapon(WeaponType weaponType) 
    {
        userData.availableWeapons.Add((int) weaponType);
        userData.coins -= weaponSO.GetPrice(weaponType);
        DataManager.Instance.SaveData(userData);
    }

    public void OnBotKill()
    {
        coinsGained++;
    }

    private void SaveCoins()
    {
        userData.coins += coinsGained;
        coinsGained = 0;
        DataManager.Instance.SaveData(userData);
    }

    public void PauseGame()
    {
        ChangeState(GameState.Pause);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Gameplay);
        Time.timeScale = 1;
    }

    public void GoBackToMenu()
    {
        ChangeState(GameState.MainMenu);
        Time.timeScale = 1;
    }

    private void Subscribe()
    {
        LevelManager.Instance.OnGameOver += SaveCoins;
        LevelManager.Instance.OnGameVictory += SaveCoins;
    }

    private void Unsubscribe()
    {
        LevelManager.Instance.OnGameOver -= SaveCoins;
        LevelManager.Instance.OnGameVictory -= SaveCoins;
    }
}