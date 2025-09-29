using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiniMap", menuName = "ScriptableObjects/MiniMapData", order = 1)]
public class ProgressPathData : ScriptableObject
{
    public List<float> PathDataValue = new();
}
