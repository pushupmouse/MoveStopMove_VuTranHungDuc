using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [SerializeField] private WeaponSO weaponSO;

    public Action OnWeaponChanged;


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


}
