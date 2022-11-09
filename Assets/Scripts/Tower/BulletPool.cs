using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private List<IPoollableBullet> _bulletsPool = new List<IPoollableBullet>();
    private Bullet _bulletPrefab;

    public void InitializePool(Bullet bulletPrefab, int poolSize)
    {
        _bulletPrefab = bulletPrefab;

        for (int i = 0; i < poolSize; i++)       
            SpawnBullet();        
    }

    public IPoollableBullet GetBulletFromPool()
    {
        var bullet = _bulletsPool.Find(x => x.IsFree() == true);

        if (bullet != null)
        {
            _bulletsPool.Remove(bullet);
            return bullet;
        }
        else
        {
            return SpawnOverPoolBullet();
        }
    }

    public void Return(IPoollableBullet bullet)
    {
        bullet.SetInnactive();
        _bulletsPool.Add(bullet);
    }

    private void SpawnBullet()
    {
        var newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<IPoollableBullet>();
        newBullet.SetPool(this);
        newBullet.SetInnactive();     
        _bulletsPool.Add(newBullet);
    }

    private IPoollableBullet SpawnOverPoolBullet()
    {
        var newBullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity).GetComponent<IPoollableBullet>();
        newBullet.SetPool(this);
        newBullet.SetInnactive();
        return newBullet;
    }
}
