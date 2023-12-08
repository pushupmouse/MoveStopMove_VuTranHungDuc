using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button skinButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI coinText;

    private bool isPause;
    public GameObject gamePanel;
    public TextMeshProUGUI toKillText;
    public GameObject menuPanel;
   

    private void Start()
    {
        Subscribe();
        Unsubscribe();
        AddListeners();
        OnInit();
    }

    public void OnInit()
    {
        isPause = false;
        SetCoin();
        menuPanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
    }

    private void SetCoin()
    {
        coinText.SetText(GameManager.Instance.UserData.coins.ToString());
    }

    private void OnWeaponButtonClick()
    {
        menuPanel.SetActive(false);
        WeaponShopManager.Instance.weaponPanel.SetActive(true);
        WeaponShopManager.Instance.OnInit();
        GameManager.Instance.ChangeState(GameManager.GameState.WeaponSelect);
    }

    private void OnSkipButtonClick()
    {
        menuPanel.SetActive(false);
        SkinShopManager.Instance.skinPanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.SkinSelect);
    }

    private void OnPlayButtonClick()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        LevelManager.Instance.OnInit();
    }

    private void OnPauseButtonClick()
    {
        if(!isPause)
        {
            GameManager.Instance.PauseGame();
            PauseMenuManager.Instance.pausePanel.SetActive(true);
            isPause = true;
        }
        else
        {
            GameManager.Instance.ResumeGame();
            PauseMenuManager.Instance.pausePanel.SetActive(false);
            isPause = false;
        }
    }

    private void HideGamePanel()
    {
        gamePanel.SetActive(false);
    }

    private void Subscribe()
    {
        WeaponShopManager.Instance.OnWeaponPurchase += SetCoin;
        LevelManager.Instance.OnGameOver += SetCoin;
        LevelManager.Instance.OnGameVictory += SetCoin;
        LevelManager.Instance.OnEnterMenu += HideGamePanel;
    }

    private void Unsubscribe()
    {
        WeaponShopManager.Instance.OnWeaponPurchase -= SetCoin;
        LevelManager.Instance.OnGameOver -= SetCoin;
        LevelManager.Instance.OnGameVictory -= SetCoin;
        LevelManager.Instance.OnEnterMenu -= HideGamePanel;
    }

    private void AddListeners()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        weaponButton.onClick.AddListener(OnWeaponButtonClick);
        skinButton.onClick.AddListener(OnSkipButtonClick);
        pauseButton.onClick.AddListener(OnPauseButtonClick);
    }
}
