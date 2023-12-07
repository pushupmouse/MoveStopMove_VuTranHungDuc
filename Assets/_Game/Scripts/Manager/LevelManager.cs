using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int botsToKill = 5;
    public Action OnGameStart;
    public Action OnGameVictory;
    public Action OnGameOver;
    public Action OnEnterMenu;

    public void OnInit()
    {
        OnGameStart?.Invoke();
        botsToKill = 5;
        UIManager.Instance.toKillText.SetText(botsToKill.ToString());
        SpawnBots();
    }

    public void SpawnBots()
    {
        BotPool botPool = BotPool.Instance;

        for (int i = 0; i < botPool.amountToPool; i++)
        {
            GameObject botObj = botPool.GetPooledObject();

            if (botObj != null)
            {
                Bot bot = botObj.GetComponent<Bot>();
                bot.Activate();
            }
        }
    }

    public void Respawn(Bot bot)
    {
        if (GameManager.Instance.IsState(GameManager.GameState.GameOver))
        {
            return;
        }

        bot.Activate();
    }


    public void OnBotKill()
    {
        botsToKill--;
        UIManager.Instance.toKillText.SetText(botsToKill.ToString());
        if (botsToKill == 0)
        {
            GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
            OnGameVictory?.Invoke();
        }
    }

    public void OnPlayerDeath()
    {
        OnGameOver?.Invoke();
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);
    }
}
