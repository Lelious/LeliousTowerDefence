using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/TowerData", order = 1)]
public class TowerData : Data
{
	[Header("Tower")]
	public TowerType Type;
	public Tower TowerPrefab;
	public ParticleSystem DustParticles;
	public int UpgradeNumber;

	[Space]
	[Header("Tower bullet")]
	public BulletType BulletType;
	public Bullet BulletPrefab;
	public float FlyCurvature;
	public bool OnTarget = true;
	public GameObject ImpactOnHit;

	[Space]
	[Header("Tower parameters")]

	public int Cost;
	public float AttackSpeed;
	public float ProjectileSpeed;
	public float ProjectileSpeedIncreacement;
	public float ExplosionRadius;
	public float AttackRadius;
	public int TargetsCount;
	public int RicochetteCount;

	[Space]
	[Header("Upgradables")]
	public List<TowerData> UpgradablesList;

	[Space]
	[Header("Upgradables")]
	public List<TowerAbility> Abilities;
}
