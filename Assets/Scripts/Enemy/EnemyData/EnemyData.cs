using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
	[Header("Enemy")]
	public EnemyEntity EnemyPrefab;
	public Sprite MainImage;

	[Space]
	[Header("Enemy parameters")]

	public string Name;
	public int Worth;
	public int Armor;
	public int Hp;
	public float Speed;
}
