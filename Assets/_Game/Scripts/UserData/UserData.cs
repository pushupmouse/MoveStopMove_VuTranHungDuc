using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int coins;
    public int currentLevel;
    public int equippedWeapon;
    public int equippedHat;
    public int equippedPants;
    public int equippedShield;
    public List<int> ownershipWeapons = new List<int>();
    public List<int> ownershipHats = new List<int>();
    public List<int> ownershipPants = new List<int>();
    public List<int> ownershipShields = new List<int>();


    public UserData(WeaponSO weaponSO, SkinSO hatSO, SkinSO pantsSO, SkinSO shieldSO)
    {
        coins = 0;
        currentLevel = 0;
        equippedWeapon = 0;
        equippedHat = -1;
        equippedPants = -1;
        equippedShield = -1;

        for(int i = 0; i < weaponSO.weapons.Count; i++)
        {
            ownershipWeapons.Add((int)OwnershipType.Unowned);
        }

        for(int i = 0; i < hatSO.skins.Count; i++)
        {
            ownershipHats.Add((int)OwnershipType.Unowned);
        }

        for(int i = 0; i < pantsSO.skins.Count; i++)
        {
            ownershipPants.Add((int)OwnershipType.Unowned);
        }

        for(int i = 0; i < shieldSO.skins.Count; i++)
        {
            ownershipShields.Add((int)OwnershipType.Unowned);
        }

        ownershipWeapons[0] = (int)OwnershipType.Equipped;
    }
}

public enum OwnershipType
{
    Unowned = 0,
    Owned = 1,
    Equipped = 2,
}
