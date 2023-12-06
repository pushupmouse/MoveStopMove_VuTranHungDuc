using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button weaponButton;
    
    public GameObject menuPanel;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        weaponButton.onClick.AddListener(OnWeaponButtonClick);
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
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
        LevelManager.Instance.SpawnBots();
    }
}
