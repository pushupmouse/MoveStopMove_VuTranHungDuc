using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData")]
public class WeaponSO : ScriptableObject
{
    public List<WeaponData> weapons;

    public GameObject GetWeapon(WeaponType type)
    {
        return weapons[(int)type].weapon;
    }

    public WeaponData GetWeaponByIndex(int index)
    {
        return weapons[index];
    }

    public int GetPrice(WeaponType type)
    {
        return weapons[(int)type].price;
    }

    public float GetRange(WeaponType type)
    {
        return weapons[(int)type].range;
    }

    public float GetSpeed(WeaponType type)
    {
        return weapons[(int)type].speed;
    }
}
