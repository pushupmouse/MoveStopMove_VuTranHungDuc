using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkinData
{
    public string skinName;
    public GameObject skin;
    public Material skinMaterial;
    public Texture2D shopPreview;
    public int price;
}
