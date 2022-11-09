using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerData : ScriptableObject
{
	[Header("Tower")]
	public NewTower TowerPrefab;
	public ParticleSystem DustParticles;
	public Sprite MainImage;

	[Space]
	[Header("Tower bullet")]
	public Bullet BulletPrefab;

	[Space]
	[Header("Tower parameters")]

	public string Name;
	public int Cost;
	public float BuildingTime;
	public int MinimalDamage;
	public int MaximumDamage;
	public float AttackSpeed;
	public float ProjectileSpeed;
	public float ProjectileParentingTime;

	[Space]
	[Header("Upgradables")]
	public List<TowerData> UpgradablesList;
}
