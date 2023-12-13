using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private SkinSO hatSO;
    [SerializeField] private SkinSO pantsSO;
    [SerializeField] private SkinSO shieldSO;

    private GameState gameState;
    private int coinsGained = 0;
    public UserData userData;

    public UserData UserData { get => userData; set => userData = value; }

    public enum GameState
    {
        MainMenu = 0,
        Gameplay = 1,
        WeaponSelect = 2,
        SkinSelect = 3,
        Pause = 4,
        GameOver = 5,
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
            userData = new UserData(weaponSO, hatSO, pantsSO, shieldSO);
            DataManager.Instance.SaveData(userData);
        }
    }



    public void GainCoins(int amount)
    {
        coinsGained += amount;
    }

    public void SaveCoins()
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
