using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : Singleton<SkinShopManager>
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI equipButtonText;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private Button hatButton;
    [SerializeField] private Button pantsButton;
    [SerializeField] private Button shieldButton;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Button buttonPrefab;
    [SerializeField] private SkinSO hats;
    [SerializeField] private SkinSO pants;
    [SerializeField] private SkinSO shields;

    private List<Button> displayItems = new List<Button>();
    private int equippedHatIndex;
    private int equippedPantsIndex;
    private int equippedShieldIndex;
    private int hatIndex;
    private int pantsIndex;
    private int shieldIndex;
    private bool isChoosingHat;
    private bool isChoosingPants;
    private bool isChoosingShield;
    private int coins;
    public GameObject skinPanel;
    public Action OnSkinPurchase;

    private void Start()
    {
        AddListeners();
        OnInit();
        OnHatButtonClick();
    }

    private void OnInit()
    {
        coins = GameManager.Instance.UserData.coins;
        equippedHatIndex = GameManager.Instance.UserData.equippedHat;
        equippedPantsIndex = GameManager.Instance.UserData.equippedPants;
        equippedShieldIndex = GameManager.Instance.UserData.equippedShield;
        hatIndex = GameManager.Instance.UserData.equippedHat;
        pantsIndex = GameManager.Instance.UserData.equippedPants;
        shieldIndex = GameManager.Instance.UserData.equippedShield;

        ResetButton();
    }

    private void OnHatButtonClick()
    {
        ClearDisplayItems();

        isChoosingHat = true;

        SetDisplayItems(hats, SkinType.Hat);
    }

    private void OnPantsButtonClick()
    {
        ClearDisplayItems();

        isChoosingPants = true;

        SetDisplayItems(pants, SkinType.Pants);
    }

    private void OnShieldButtonClick()
    {
        ClearDisplayItems();

        isChoosingShield = true;

        SetDisplayItems(shields, SkinType.Shield);
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

        isChoosingHat = false;
        isChoosingPants = false;
        isChoosingShield = false;
    }

    private void SetDisplayItems(SkinSO skin, SkinType skinType)
    {
        for (int i = 0; i < skin.skins.Count; i++)
        {
            SkinData skinData = skin.skins[i];
            int index = i;

            Button button = Instantiate(buttonPrefab, gridLayoutGroup.transform);
            button.image.sprite = SpriteFromTexture2D(skinData.shopPreview);

            button.onClick.AddListener(() => OnItemButtonClick(skin, index, skinType));

            displayItems.Add(button);
        }
    }

    private void OnItemButtonClick(SkinSO skin, int index, SkinType skinType)
    {
        SetIndex(skinType, index);
        SetButton(skin, index, skinType);
    }

    private void OnEquipButtonClick()
    {
        if (isChoosingHat)
        {
            EquipmentManager.Instance.EquipHat(hatIndex);
        }

        if (isChoosingPants)
        {
            EquipmentManager.Instance.EquipPants(pantsIndex);
        }

        if (isChoosingShield)
        {
            EquipmentManager.Instance.EquipShield(shieldIndex);
        }

        OnInit();
    }

    private void OnBuyButtonClick()
    {
        if (isChoosingHat)
        {
            EquipmentManager.Instance.BuyHat(hatIndex);
            EquipmentManager.Instance.EquipHat(hatIndex);
        }

        if (isChoosingPants)
        {
            EquipmentManager.Instance.BuyPants(pantsIndex);
            EquipmentManager.Instance.EquipPants(pantsIndex);
        }

        if (isChoosingShield)
        {
            EquipmentManager.Instance.BuyShield(shieldIndex);
            EquipmentManager.Instance.EquipShield(shieldIndex);
        }

        OnSkinPurchase?.Invoke();
        OnInit();
    }

    private void SetButton(SkinSO skin, int index, SkinType skinType)
    {
        int price = skin.skins[index].price;

        if (GetAvailableSkin(skinType).Contains(index))
        {
            equipButton.gameObject.SetActive(true);
            buyButton.gameObject.SetActive(false);

            if (GetIndex(skinType) == GetEquippedIndex(skinType))
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

    private void SetIndex(SkinType skinType, int index)
    {
        switch (skinType)
        {
            case SkinType.Hat:
                hatIndex = index;
                break;
            case SkinType.Pants:
                pantsIndex = index;
                break;
            case SkinType.Shield:
                shieldIndex = index;
                break;
            default:
                hatIndex = -1;
                break;
        }
    }

    private int GetIndex(SkinType skinType)
    {
        int index = -1;
        switch (skinType)
        {
            case SkinType.Hat:
                index = hatIndex;
                break;
            case SkinType.Pants:
                index = pantsIndex;
                break;
            case SkinType.Shield:
                index = shieldIndex;
                break;
            default:
                index = -1;
                break;
        }
        return index;
    }

    private int GetEquippedIndex(SkinType skinType)
    {
        int equippedIndex = -1;
        switch (skinType)
        {
            case SkinType.Hat:
                equippedIndex = equippedHatIndex;
                break;
            case SkinType.Pants:
                equippedIndex = equippedPantsIndex;
                break;
            case SkinType.Shield:
                equippedIndex = equippedShieldIndex;
                break;
            default:
                equippedIndex = -1;
                break;
        }
        return equippedIndex;
    }

    private List<int> GetAvailableSkin(SkinType skinType)
    {
        List<int> availableSkin = new List<int>();
        switch (skinType) 
        {
            case SkinType.Hat:
                availableSkin = GameManager.Instance.UserData.availableHats;
                break;
            case SkinType.Pants: 
                availableSkin = GameManager.Instance.UserData.availablePants; 
                break;
            case SkinType.Shield:
                availableSkin = GameManager.Instance.UserData.availableShields;
                break;
            default:
                availableSkin = null;
                break;
        }
        return availableSkin;
    }

    private void ResetButton()
    {
        if (isChoosingHat)
        {
            SetButton(hats, equippedHatIndex, SkinType.Hat);
        }

        if (isChoosingPants)
        {
            SetButton(pants, equippedPantsIndex, SkinType.Pants);
        }

        if (isChoosingShield)
        {
            SetButton(shields, equippedShieldIndex, SkinType.Shield);
        }
    }

    private void AddListeners()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
        hatButton.onClick.AddListener(OnHatButtonClick);
        pantsButton.onClick.AddListener(OnPantsButtonClick);
        shieldButton.onClick.AddListener(OnShieldButtonClick);
        equipButton.onClick.AddListener(OnEquipButtonClick);
        buyButton.onClick.AddListener(OnBuyButtonClick);
    }
}
