using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string weaponDataKey = "weaponDataKey";

    public string SaveToString()
    {
        WeaponData weaponData = new WeaponData();
        return JsonUtility.ToJson(weaponData);
    }

    public void SaveToPlayerPref(WeaponData weapon)
    {
        PlayerPrefs.SetString(weaponDataKey, SaveToString());
    }

    public WeaponData GetUnitData()
    {
        string data = PlayerPrefs.GetString(weaponDataKey);
        if (!string.IsNullOrEmpty(data))
        {
            WeaponData weaponData = JsonUtility.FromJson<WeaponData>(data);
            return weaponData;
        }
        return null;
    }


    //this will get the weapon type
    public WeaponSO weaponSO;

    public WeaponData GetWeaponData(WeaponType weaponType)
    {
        List<WeaponData> weaponData = weaponSO.weapons;
        for (int i = 0; i < weaponData.Count; i++)
        {
            if(weaponType == weaponData[i].weaponType)
            {
                return weaponData[i];
            }
        }
        return null;
    }
}
