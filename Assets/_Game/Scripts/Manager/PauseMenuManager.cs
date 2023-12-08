using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : Singleton<PauseMenuManager>
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button resumeButton;

    public GameObject pausePanel;

    private void Start()
    {
        AddListeners();
    }

    private void OnMenuButtonClick()
    {
        pausePanel.SetActive(false);
        UIManager.Instance.gamePanel.SetActive(false);
        UIManager.Instance.OnInit();
        LevelManager.Instance.OnEnterMenu?.Invoke();
        GameManager.Instance.GoBackToMenu();
    }

    private void OnResumeButtonClick()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    private void AddListeners()
    {
        menuButton.onClick.AddListener(OnMenuButtonClick);
        resumeButton.onClick.AddListener(OnResumeButtonClick);
    }
}
