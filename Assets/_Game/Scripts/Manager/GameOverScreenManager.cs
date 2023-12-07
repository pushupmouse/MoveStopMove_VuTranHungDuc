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
    [SerializeField] private TextMeshProUGUI gameOverText;

    public Action OnEnterMenu;

    private void Start()
    {
        LevelManager.Instance.OnGameOver -= ShowMenuDefeat;
        LevelManager.Instance.OnGameOver += ShowMenuDefeat;
        LevelManager.Instance.OnGameVictory -= ShowMenuVictory;
        LevelManager.Instance.OnGameVictory += ShowMenuVictory;

        menuButton.onClick.AddListener(OnMenuButtonClick);
        playAgainButton.onClick.AddListener(OnPlayAgainButtonClick);
    }

    private void OnPlayAgainButtonClick()
    {
        gameOverPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        LevelManager.Instance.OnInit();
    }

    private void OnMenuButtonClick()
    {
        gameOverPanel.SetActive(false);
        UIManager.Instance.OnInit();
        OnEnterMenu?.Invoke();
    }

    private void ShowMenuVictory()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetText("YOU WIN");
    }

    private void ShowMenuDefeat()
    {
        gameOverPanel.SetActive(true);
        gameOverText.SetText("YOU LOSE");
    }
}
