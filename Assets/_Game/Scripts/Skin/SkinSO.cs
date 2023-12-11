using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinData")]
public class SkinSO : ScriptableObject
{
    public SkinType skinType;
    public List<SkinData> skins;

    public SkinData GetSkinByIndex(int index)
    {
        return skins[index];
    }

    public int GetPrice(int index)
    {
        return skins[index].price;
    }
}

public enum SkinType
{
    Hat = 0,
    Pants = 1,
    Shield = 2,
}
