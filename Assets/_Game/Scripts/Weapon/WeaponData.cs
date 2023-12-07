using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public WeaponType weaponType;
    public GameObject bullet;
    public GameObject weapon;
    public GameObject shopPreview;
    public int price;
    public float range;
    public float speed;
}