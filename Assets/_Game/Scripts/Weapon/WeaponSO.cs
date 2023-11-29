using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData")]
public class WeaponSO : ScriptableObject
{
    public List<WeaponData> weapons;
}
