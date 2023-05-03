using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerData : ScriptableObject
{
	[Header("Tower")]
	public TowerType Type;
	public NewTower TowerPrefab;
	public ParticleSystem DustParticles;
	public Sprite MainImage;
	public int UpgradeNumber;

	[Space]
	[Header("Tower bullet")]
	public Bullet BulletPrefab;

	[Space]
	[Header("Tower parameters")]

	public string Name;
	public int Cost;
	public int BuildingTime;
	public int MinimalDamage;
	public int MaximumDamage;
	public float AttackSpeed;
	public float ProjectileSpeed;
	public float ProjectileParentingTime;
	public float ExplosionRadius;
	public float AttackRadius;
	public int TargetsCount;
	public int RicochetteCount;

	[Space]
	[Header("Upgradables")]
	public List<TowerData> UpgradablesList;
}
