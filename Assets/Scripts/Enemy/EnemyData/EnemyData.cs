using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : Data
{
	[Header("Enemy")]
	public EnemyEntity EnemyPrefab;

	[Space]
	[Header("Enemy parameters")]

	public int Worth;
	public float Speed;
	public int Armor;
}
