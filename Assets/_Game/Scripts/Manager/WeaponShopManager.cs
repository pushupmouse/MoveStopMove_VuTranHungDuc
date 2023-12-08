using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopManager : Singleton<WeaponShopManager>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI equipButtonText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject weaponDisplay;

    private GameObject displayWeapon;
    private int equippedWeaponIndex;
    private int weaponIndex;
    private int coins;
    public Action OnWeaponPurchase;
    public GameObject weaponPanel;

    private void Start()
    {
        AddListeners();
        OnInit();
    }

    public void OnInit()
    {
        coins = GameManager.Instance.UserData.coins;
        equippedWeaponIndex = GameManager.Instance.UserData.equippedWeapon;
        weaponIndex = GameManager.Instance.UserData.equippedWeapon;
        DisplayShopItem();
    }

    private void DisplayShopItem()
    {
        if (displayWeapon != null)
        {
            Destroy(displayWeapon);
        }

        displayWeapon = Instantiate(weaponSO.GetWeaponByIndex(weaponIndex).shopPreview, weaponDisplay.transform);

        SetButton();
    }

    private void OnEquipButtonClick()
    {
        EquipmentManager.Instance.EquipWeapon((WeaponType)weaponIndex);
        OnInit();
    }

    private void OnBuyButtonClick()
    {
        EquipmentManager.Instance.BuyWeapon((WeaponType)weaponIndex);
        EquipmentManager.Instance.EquipWeapon((WeaponType)weaponIndex);
        OnWeaponPurchase?.Invoke();
        OnInit();
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
        DisplayShopItem();
    }

    private void OnPreviousButtonClick()
    {
        weaponIndex--;
        if( weaponIndex < 0 )
        {
            weaponIndex = weaponSO.weapons.Count - 1;
        }
        DisplayShopItem();
    }

    private void SetButton()
    {
        int price = weaponSO.GetWeaponByIndex(weaponIndex).price;

        if (GameManager.Instance.UserData.availableWeapons.Contains(weaponIndex))
        {
            equipButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            if (weaponIndex == equippedWeaponIndex)
            {
                equipButton.interactable = false;
                equipButtonText.SetText("EQUIPPED");
            }
            else
            {
                equipButton.interactable = true;
                equipButtonText.SetText("EQUIP");
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);

            buyButtonText.SetText(price.ToString());

            if (coins >= price)
            {
                buyButton.interactable = true;
                buyButtonText.color = Color.black;

            }
            else
            {
                buyButton.interactable = false;
                buyButtonText.color = Color.red;
            }
        }
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        nextButton.onClick.AddListener(OnNextButtonClick);
        previousButton.onClick.AddListener(OnPreviousButtonClick);
        equipButton.onClick.AddListener(OnEquipButtonClick);
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }
}
