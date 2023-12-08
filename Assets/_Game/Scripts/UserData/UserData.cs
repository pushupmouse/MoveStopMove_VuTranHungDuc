using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int coins;
    public int equippedWeapon;
    public int equippedHat;
    public int equippedPants;
    public int equippedShield;
    public List<int> availableWeapons;
    public List<int> availableHats;
    public List<int> availablePants;
    public List<int> availableShields;

    public UserData()
    {
        coins = 0;
        equippedWeapon = 0;
        equippedHat = -1;
        equippedPants = -1;
        equippedShield = -1;
        availableWeapons = new List<int>();
        availableHats = new List<int>();
        availablePants = new List<int>();
        availableShields = new List<int>();

        availableWeapons.Add(equippedWeapon);
    }
}
