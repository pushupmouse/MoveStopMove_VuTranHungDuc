using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int coins;
    public int equippedWeapon;
    public List<int> availableWeapons;

    public UserData()
    {
        coins = 0;
        equippedWeapon = 0;
        availableWeapons = new List<int> { 0 };
    }
}
