using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData")]
public class SkinSO : ScriptableObject
{
    public List<SkinData> skins;

    public int GetPrice(int index)
    {
        return skins[index].price;
    }
}
