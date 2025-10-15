using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : Data
{
	[Header("Enemy parameters")]
	public float Scale;
	public int Worth;
	public float Speed;
	public int Armor;

	public EnemyData(float scale, float speed, int hp, int worth, int armor)
    {
		Scale = scale;
		Speed = speed;
		MaxHP = hp;
		Worth = worth;
		Armor = armor;
    }
}
