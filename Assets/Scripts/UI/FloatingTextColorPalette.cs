using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatingTextColorPalette", menuName = "ScriptableObjects/UI/ColorPalette", order = 1)]
public class FloatingTextColorPalette : ScriptableObject
{
    [SerializeField] private List<ColorExample> _colorPalette = new List<ColorExample>();

    public Color GetColor(DamageSource source) => _colorPalette.Find(x => x.Source == source).Color;
}

[Serializable]
public class ColorExample
{
    public DamageSource Source;
    public Color Color;
}

public enum DamageSource
{
    Normal,
    Critical,
    Ice,
    Fire,
    Death
}
