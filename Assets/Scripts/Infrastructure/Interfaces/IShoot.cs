using UnityEngine;

public interface IShoot
{
	public void Shoot(IDamagable damagable);
	public bool DetectEnemy();
}
