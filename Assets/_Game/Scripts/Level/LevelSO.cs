using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class LevelSO : ScriptableObject
{
    public List<Level> levels;
}
