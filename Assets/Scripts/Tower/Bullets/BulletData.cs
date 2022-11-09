using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BulletData", order = 1)]
public class BulletData : ScriptableObject
{
	[Header("Bullet parameters")]

	public string Name;
	public int Cost;
	public float BuildingTime;
	public int MinimalDamage;
	public int MaximumDamage;
	public float AttackSpeed;

	[Space]
	[Header("Upgradables")]
	public List<TowerData> UpgradablesList;
}

