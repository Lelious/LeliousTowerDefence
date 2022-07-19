using UnityEngine;

public interface IShoot
{
	public void Shoot(Transform position);
	public void DetectEnemy();
	public void SpawnBullet();
}
