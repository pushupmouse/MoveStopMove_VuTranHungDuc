using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelSO levelSO;
    [SerializeField] private Player player;
    [SerializeField] private CameraFollow _camera;
    
    private int botsToKill;
    private int coinPerKill = 1;
    private Level currentLevel;
    private int levelIndex;
    public Action OnGameStart;
    public Action OnGameVictory;
    public Action OnGameOver;
    public Action OnEnterMenu;

    private void Start()
    {
        levelIndex = GameManager.Instance.userData.currentLevel;
        
        SpawnLevel();
    }

    public void OnInit()
    {
        OnGameStart?.Invoke();
        botsToKill = currentLevel.enemiesToKill;
        UIManager.Instance.toKillText.SetText(botsToKill.ToString());
        _camera.OnInit();
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
        GameManager.Instance.GainCoins(coinPerKill);
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

    private void SpawnLevel()
    {
        List<Level> levels = levelSO.levels;

        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[levelIndex % levelSO.levels.Count], Vector3.zero, Quaternion.identity);
    }

    public void NextLevel()
    {
        levelIndex++;

        GameManager.Instance.ChangeLevel(levelIndex % levelSO.levels.Count);

        SpawnLevel();
    }
}
