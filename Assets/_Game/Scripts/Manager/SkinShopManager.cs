using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : Singleton<SkinShopManager>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button hatButton;
    [SerializeField] private Button pantsButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private SkinSO hats;
    [SerializeField] private SkinSO pants;
    [SerializeField] private SkinSO shields;

    private List<Button> displayItems = new List<Button>();
    public GameObject skinPanel;

    private void Start()
    {
        AddListeners();

        OnInit();
    }

    private void OnInit()
    {
        OnHatButtonClick();
    }

    private void OnHatButtonClick()
    {
        ClearDisplayItems();

        SetDisplayItems(hats);
    }

    private void OnPantsButtonClick()
    {
        ClearDisplayItems();

        SetDisplayItems(pants);
    }

    private void OnShieldButtonClick()
    {
        ClearDisplayItems();

        SetDisplayItems(shields);
    }

    private void OnCloseButtonClick()
    {
        skinPanel.SetActive(false);
        UIManager.Instance.menuPanel.SetActive(true);
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
    }

    private Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }

    private void ClearDisplayItems()
    {
        if (displayItems.Count != 0)
        {
            for (int i = displayItems.Count - 1; i >= 0; i--)
            {
                Destroy(displayItems[i].gameObject);
            }

            displayItems.Clear();
        }
    }

    private void SetDisplayItems(SkinSO skinType)
    {
        for (int i = 0; i < skinType.skins.Count; i++)
        {
            SkinData skinData = skinType.skins[i];
            int index = i;

            Button button = Instantiate(buttonPrefab, gridLayoutGroup.transform);
            button.image.sprite = SpriteFromTexture2D(skinData.shopPreview);

            button.onClick.AddListener(() => OnItemButtonClick(skinType.skins[index]));

            displayItems.Add(button);
        }
    }

    private void OnItemButtonClick(SkinData skinData)
    {
        Debug.Log(skinData.skinName);
        Debug.Log(skinData.price);

        SetButton(skinData);
    }

    private void SetButton(SkinData skinData)
    {
        int price = skinData.price;

        //prob use switch case... or enums idk

        //if (GameManager.Instance.UserData.availableWeapons.Contains(weaponIndex))
        //{
        //    equipButton.gameObject.SetActive(true);
        //    buyButton.gameObject.SetActive(false);

        //    if (weaponIndex == equippedWeaponIndex)
        //    {
        //        equipButton.interactable = false;
        //        equipButtonText.SetText("EQUIPPED");
        //    }
        //    else
        //    {
        //        equipButton.interactable = true;
        //        equipButtonText.SetText("EQUIP");
        //    }
        //}
        //else
        //{
        //    buyButton.gameObject.SetActive(true);
        //    equipButton.gameObject.SetActive(false);

        //    buyButtonText.SetText(price.ToString());

        //    if (coins >= price)
        //    {
        //        buyButton.interactable = true;
        //        buyButtonText.color = Color.black;

        //    }
        //    else
        //    {
        //        buyButton.interactable = false;
        //        buyButtonText.color = Color.red;
        //    }
        //}
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        hatButton.onClick.AddListener(OnHatButtonClick);
        pantsButton.onClick.AddListener(OnPantsButtonClick);
        shieldButton.onClick.AddListener(OnShieldButtonClick);
    }
}
