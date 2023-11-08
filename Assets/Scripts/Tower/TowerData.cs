using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerData : Data
{
	[Header("Tower")]
	public TowerType Type;
	public NewTower TowerPrefab;
	public ParticleSystem DustParticles;
	public int UpgradeNumber;

	[Space]
	[Header("Tower bullet")]
	public Bullet BulletPrefab;

	[Space]
	[Header("Tower parameters")]

	public int Cost;
	public float AttackSpeed;
	public float ProjectileSpeed;
	public float ExplosionRadius;
	public float AttackRadius;
	public int TargetsCount;
	public int RicochetteCount;

	[Space]
	[Header("Upgradables")]
	public List<TowerData> UpgradablesList;
}
