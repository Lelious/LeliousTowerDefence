using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private TowerData _towerData;
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 10)] private int _poolSize;

    private float _delayBetweenShoots;

    private protected void Awake()
    {
        _delayBetweenShoots = _towerData.AttackSpeed;
        _bulletPool.InitializePool(_towerData.BulletPrefab, _poolSize);

        StartCoroutine(ShootingRoutine());
    }

    public bool DetectEnemy()
    {
        for (int i = 0; i < _enemyChecker.EnemiesList.Count; i++)
        {
            if (_enemyChecker.EnemiesList[i] != null && _enemyChecker.EnemiesList[i].CanBeAttacked())
            {
                Shoot(_enemyChecker.EnemiesList[i]);
                return true;
            }
        }
        return false;       
    }

    public void Shoot(IDamagable damagable)
    {
        var bullet = _bulletPool.GetBulletFromPool();

        bullet.SetActive();
        bullet.GetBullet().SetBulletParameters(_towerData);
        bullet.Transform().position = transform.position;
        bullet.SetTarget(damagable);
        bullet.GetBullet().FlyOnTarget();
    }   

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            while (DetectEnemy())
            {
                yield return new WaitForSeconds(_delayBetweenShoots);
            }
            yield return new WaitForSeconds(_delayBetweenShoots/2);
        }
    }
}
