using UniRx;
using UnityEngine;

public struct GamePannelUdaterInfoContainer
{
    public ITouchable Touchable;
    public Sprite PreviewImage;
    public FloatReactiveProperty CurrentHealth;
    public string Name;
    public float MaxHealth;
    public float MinDamage;
    public float MaxDamage;
    public float Armor;
    public float AttackSpeed;
}
