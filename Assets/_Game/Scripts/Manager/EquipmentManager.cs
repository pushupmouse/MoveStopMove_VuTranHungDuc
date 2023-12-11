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

    public void EquipWeapon(WeaponType weaponType)
    {
        GameManager.Instance.userData.equippedWeapon = (int)weaponType;
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnWeaponChanged?.Invoke();
    }

    public void BuyWeapon(WeaponType weaponType)
    {
        GameManager.Instance.userData.availableWeapons.Add((int)weaponType);
        GameManager.Instance.userData.coins -= weaponSO.GetPrice(weaponType);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void EquipHat(int index)
    {
        GameManager.Instance.userData.equippedHat = index;
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnHatChanged?.Invoke();
    }

    public void EquipPants(int index)
    {
        GameManager.Instance.userData.equippedPants = index;
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnPantsChanged?.Invoke();
    }   

    public void EquipShield(int index)
    {
        GameManager.Instance.userData.equippedShield = index;
        DataManager.Instance.SaveData(GameManager.Instance.userData);
        OnShieldChanged?.Invoke();
    }

    public void BuyHat(int index)
    {
        GameManager.Instance.userData.availableHats.Add(index);
        GameManager.Instance.userData.coins -= hatSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void BuyPants(int index)
    {
        GameManager.Instance.userData.availablePants.Add(index);
        GameManager.Instance.userData.coins -= pantsSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }

    public void BuyShield(int index)
    {
        GameManager.Instance.userData.availableShields.Add(index);
        GameManager.Instance.userData.coins -= shieldSO.GetPrice(index);
        DataManager.Instance.SaveData(GameManager.Instance.userData);
    }
}
