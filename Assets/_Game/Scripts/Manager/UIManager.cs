using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        weaponButton.onClick.AddListener(OnWeaponButtonClick);
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    private void OnCloseButtonClick()
    {
        weaponPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
        menuPanel.SetActive(true);   
    }

    private void OnWeaponButtonClick()
    {
        menuPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.WeaponSelect);
        weaponPanel.SetActive(true);
    }

    private void OnPlayButtonClick()
    {
        menuPanel.SetActive(false);
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        LevelManager.Instance.SpawnBot();
    }
}
