using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public WeaponType weaponType;
    public Bullet bullet;
    public Weapon weapon;
    public float range;
    public float speed;
}

public enum WeaponType
{
    Axe = 0,
    Boomerang = 1,
    Candy = 2,
    Hammer = 3,
    Knife = 4,
    Uzi = 5,
}