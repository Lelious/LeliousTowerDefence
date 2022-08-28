using UnityEngine;

public interface IShoot
{
	public void Shoot(Transform position);
	public bool DetectEnemy();
	public void SpawnBullet();
	public void ReturnToPool(Bullet bullet);
}
