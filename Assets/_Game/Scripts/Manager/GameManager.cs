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
    public Action OnWeaponChanged;

    public UserData UserData { get => userData; set => userData = value; }

    public enum GameState
    {
        MainMenu = 0,
        Gameplay = 1,
        WeaponSelect = 2,
    }

    private void Awake()
    {
        GetUserData();
        ChangeState(GameState.MainMenu);
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
}
