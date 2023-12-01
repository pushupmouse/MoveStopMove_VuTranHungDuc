using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class GameManager : Singleton<GameManager>
{
    private GameState gameState;

    public enum GameState
    {
        MainMenu = 0,
        Gameplay = 1,
        WeaponSelect = 2,
    }

    private void Awake()
    {
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
}
