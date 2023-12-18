using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private SkinSO hatSO;
    [SerializeField] private SkinSO pantsSO;
    [SerializeField] private SkinSO shieldSO;

    public Action OnWeaponChanged;
    public Action OnHatChanged;
    public Action OnPantsChanged;
    public Action OnShieldChanged;

    public void EquipWeapon(int previousWeapon, WeaponType weaponType)
    {
        GameManager.Instance.userData.equippedWeapon = (int)weaponType;
        GameManager.Instance.userData.ownershipWeapons[(int)weaponType] = (int)OwnershipType.Equipped;
        GameManager.Instance.userData.ownershipWeapons[previousWeapon] = (int)OwnershipType.Owned;
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnWeaponChanged?.Invoke();
    }

    public void BuyWeapon(WeaponType weaponType)
    {
        GameManager.Instance.userData.ownershipWeapons[(int)weaponType] = (int)OwnershipType.Owned;
        GameManager.Instance.userData.coins -= weaponSO.GetPrice(weaponType);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void EquipHat(int previousHat, int index)
    {
        GameManager.Instance.userData.equippedHat = index;
        GameManager.Instance.userData.ownershipHats[index] = (int)OwnershipType.Equipped;
        if(previousHat != -1) 
        {
            GameManager.Instance.userData.ownershipHats[previousHat] = (int)OwnershipType.Owned;
        }
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnHatChanged?.Invoke();
    }

    public void EquipPants(int previousPants, int index)
    {
        GameManager.Instance.userData.equippedPants = index;
        GameManager.Instance.userData.ownershipPants[index] = (int)OwnershipType.Equipped;
        if (previousPants != -1)
        {
            GameManager.Instance.userData.ownershipPants[previousPants] = (int)OwnershipType.Owned;
        }
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnPantsChanged?.Invoke();
    }   

    public void EquipShield(int previousShield, int index)
    {
        GameManager.Instance.userData.equippedShield = index;
        GameManager.Instance.userData.ownershipShields[index] = (int)OwnershipType.Equipped;
        if (previousShield != -1)
        {
            GameManager.Instance.userData.ownershipShields[previousShield] = (int)OwnershipType.Owned;
        }
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnShieldChanged?.Invoke();
    }

    public void BuyHat(int index)
    {
        GameManager.Instance.userData.ownershipHats[index] = (int)OwnershipType.Owned;
        GameManager.Instance.userData.coins -= hatSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void BuyPants(int index)
    {
        GameManager.Instance.userData.ownershipPants[index] = (int)OwnershipType.Owned;
        GameManager.Instance.userData.coins -= pantsSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void BuyShield(int index)
    {
        GameManager.Instance.userData.ownershipShields[index] = (int)OwnershipType.Owned;
        GameManager.Instance.userData.coins -= shieldSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }
}
