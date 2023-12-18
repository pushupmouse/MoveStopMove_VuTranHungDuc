using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class GameOverScreenManager : Singleton<GameOverScreenManager>
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private TextMeshProUGUI gameOverText;

    private void Start()
    {
        Unsubscribe();
        Subscribe();
        AddListeners();
    }

    private void OnPlayAgainButtonClick()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        GameManager.Instance.SaveCoins();
        UIManager.Instance.SetCoin();
        LevelManager.Instance.OnInit();
    }

    private void OnNextLevelButtonClick()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        GameManager.Instance.SaveCoins();
        UIManager.Instance.SetCoin();
        LevelManager.Instance.NextLevel();
        LevelManager.Instance.OnInit();
    }

    private void OnMenuButtonClick()
    {
        gameOverPanel.SetActive(false);
        UIManager.Instance.gamePanel.SetActive(false);
        UIManager.Instance.OnInit();
        LevelManager.Instance.OnEnterMenu?.Invoke();
        GameManager.Instance.GoBackToMenu();
    }

    private void ShowMenuVictory()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetText("YOU WIN");
        nextLevelButton.gameObject.SetActive(true);
        playAgainButton.gameObject.SetActive(false);
    }

    private void ShowMenuDefeat()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetText("YOU LOSE");
        playAgainButton.gameObject.SetActive(true);
        nextLevelButton.gameObject.SetActive(false);
    }

    private void AddListeners()
    {
        menuButton.onClick.AddListener(OnMenuButtonClick);
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
    }

    private void Subscribe()
    {
        LevelManager.Instance.OnGameOver += ShowMenuDefeat;
        LevelManager.Instance.OnGameVictory += ShowMenuVictory;
    }

    private void Unsubscribe()
    {
        LevelManager.Instance.OnGameOver -= ShowMenuDefeat;
        LevelManager.Instance.OnGameVictory -= ShowMenuVictory;
    }
}
