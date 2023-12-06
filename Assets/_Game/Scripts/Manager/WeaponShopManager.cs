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
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameObject weaponDisplay;
    [SerializeField] private LayerMask layerUI;

    private GameObject displayWeapon;
    private int equippedWeaponIndex;
    private int weaponIndex;
    private int price;
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
        equippedWeaponIndex = GameManager.Instance.UserData.equippedWeapon;
        weaponIndex = GameManager.Instance.UserData.equippedWeapon;
        DisplayWeapon();
    }

    private void DisplayWeapon()
    {
        if (displayWeapon != null)
        {
            Destroy(displayWeapon);
        }

        displayWeapon = Instantiate(weaponSO.GetWeaponByIndex(weaponIndex).shopPreview, weaponDisplay.transform);
        displayWeapon.layer = (int)Mathf.Log(layerUI.value, 2);

        SetButtonText();
    }

    private void OnEquipButtonClick()
    {
        GameManager.Instance.EquipWeapon((WeaponType)weaponIndex);
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

    private void SetButtonText()
    {
        price = weaponSO.GetWeaponByIndex(weaponIndex).price;
        

        if (GameManager.Instance.UserData.availableWeapons.Contains(weaponIndex))
        {
            if(weaponIndex == equippedWeaponIndex)
            {
                buttonText.SetText("EQUIPPED");
                //gray text cant click
                //bg darker

            }
            else
            {
                buttonText.SetText("EQUIP");
                //normal text can click
                //bg brighter
            }
        }
        else
        {
            if(GameManager.Instance.UserData.coins >= price)
            {
                buttonText.SetText(price.ToString());
                //normal text can click
                //bg brighter green
            }
            else
            {
                buttonText.SetText("CAN'T BUY");
                //red text (price) cant click
                //bg darker green
            }
        }

        //if (GameManager.Instance.UserData.coins >= price)
        //{

        //}
    }
}
