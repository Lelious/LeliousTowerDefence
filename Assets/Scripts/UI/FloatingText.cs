using System;
using TMPro;
using UnityEngine;

[Serializable]
public class FloatingText
{
    public TextMeshProUGUI Text;
    public Vector3 CurrentVector;
    public float OffsetX;
    public Color Color;
    public float Lifetime;
    public float ScrollSpeed;
    public GameObject Object;

    public void ProcessPosition(float delta)
    {
        Lifetime -= delta;
        CurrentVector = new Vector3(CurrentVector.x + OffsetX * 0.2f, CurrentVector.y + delta * ScrollSpeed, CurrentVector.z);
        if (Mathf.Abs(OffsetX) < delta)
        {
            OffsetX = 0;
        }
        else
        {
            OffsetX = OffsetX > 0 ? OffsetX -= delta * 0.2f : OffsetX += delta * 0.2f;
        }

        Text.rectTransform.position = Camera.main.WorldToScreenPoint(CurrentVector);
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, Lifetime);
    }
}
