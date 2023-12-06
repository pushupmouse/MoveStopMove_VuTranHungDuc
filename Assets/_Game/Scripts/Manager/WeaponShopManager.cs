using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManager : Singleton<WeaponShopManager>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject weaponDisplay;
    [SerializeField] private LayerMask layerUI;

    private GameObject displayWeapon;
    private int weaponIndex;
    public GameObject weaponPanel;

    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        previousButton.onClick.AddListener(OnPreviousButtonClick);
        equipButton.onClick.AddListener(OnEquipButtonClick);

        OnInit();
    }

    public void OnInit()
    {
        weaponIndex = GameManager.Instance.UserData.equippedWeapon;
        DisplayWeapon();
    }

    private void DisplayWeapon()
    {
        if (displayWeapon != null)
        {
            Destroy(displayWeapon);
        }

        displayWeapon = Instantiate(weaponSO.GetWeaponPreview(weaponIndex), weaponDisplay.transform);
        displayWeapon.layer = 5;
    }

    private void OnEquipButtonClick()
    {
        GameManager.Instance.EquipWeapon((WeaponType)weaponIndex);
    }

    private void OnCloseButtonClick()
    {
        weaponPanel.SetActive(false);
        UIManager.Instance.menuPanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
    }

    private void OnNextButtonClick()
    {
        weaponIndex++;
        if(weaponIndex > weaponSO.weapons.Count - 1) 
        {
            weaponIndex = 0;
        }
        DisplayWeapon();
    }

    private void OnPreviousButtonClick()
    {
        weaponIndex--;
        if( weaponIndex < 0 )
        {
            weaponIndex = weaponSO.weapons.Count - 1;
        }
        DisplayWeapon();
    }
}
