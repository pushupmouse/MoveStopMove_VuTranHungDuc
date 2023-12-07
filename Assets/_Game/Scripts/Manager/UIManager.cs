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
    [SerializeField] private TextMeshProUGUI coinText;
    
    public GameObject toKillPanel;
    public TextMeshProUGUI toKillText;
    public GameObject menuPanel;

    private void Start()
    {
        WeaponShopManager.Instance.OnWeaponPurchase -= SetCoin;
        WeaponShopManager.Instance.OnWeaponPurchase += SetCoin;
        LevelManager.Instance.OnGameOver -= SetCoin;
        LevelManager.Instance.OnGameOver += SetCoin;
        LevelManager.Instance.OnGameVictory -= SetCoin;
        LevelManager.Instance.OnGameVictory += SetCoin;
        GameOverScreenManager.Instance.OnEnterMenu -= HideKillPanel;
        GameOverScreenManager.Instance.OnEnterMenu += HideKillPanel;
        playButton.onClick.AddListener(OnPlayButtonClick);
        weaponButton.onClick.AddListener(OnWeaponButtonClick);
        OnInit();
    }

    public void OnInit()
    {
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

    private void OnPlayButtonClick()
    {
        menuPanel.SetActive(false);
        toKillPanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        LevelManager.Instance.OnInit();
    }

    private void HideKillPanel()
    {
        toKillPanel.SetActive(false);
    }
}
