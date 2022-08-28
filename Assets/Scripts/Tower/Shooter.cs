using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShoot
{   
    [SerializeField] private Bullet _bullet;
    [SerializeField] private NewTower _tower;
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 10)] private int _poolSize;

    private List<Bullet> _bulletsPool = new List<Bullet>();
    private IDamagable _damagable;
    private int _minDamage, _maxDamage;
    private float _delayBetweenShoots;

    private protected void Awake()
    {
        _delayBetweenShoots = _tower.GetAttackSpeed();
        _minDamage = _tower.GetMinDamage();
        _maxDamage = _tower.GetMaxDamage();

        for (int i = 0; i < _poolSize; i++)
        {
            SpawnBullet();
        }

        StartCoroutine(ShootingRoutine());
    }
    public bool DetectEnemy()
    {
        if (_enemyChecker.EnemiesList.Count > 0)
        {
            var actualTarget = _enemyChecker.EnemiesList[0];

            if (actualTarget != null)
            {
                actualTarget.TryGetComponent(out _damagable);

                if (_damagable != null && _damagable.CanBeAttacked())
                {
                    Shoot(actualTarget.ShootPoint);

                    return true;
                }
                else
                {
                    _enemyChecker.EnemiesList.Remove(actualTarget);
                    return false;
                }

            }
            else
            {
                _enemyChecker.EnemiesList.Remove(actualTarget);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void Shoot(Transform point)
    {
        var bullet = _bulletsPool.Find(x => x.gameObject.activeInHierarchy == false);

        if (bullet)
        {
            BulletInitialization(point);           
        }

        else
        {
            SpawnBullet();
            BulletInitialization(point);
        }
    }

    public void SpawnBullet()
    {
        var bullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        bullet.gameObject.SetActive(false);
        _bulletsPool.Add(bullet);
    }

    private void BulletInitialization(Transform point)
    {
        var bullet = _bulletsPool.Find(x => x.gameObject.activeInHierarchy == false);

        if (bullet)
        {
            _bulletsPool.Remove(bullet);
            bullet.transform.position = transform.position;
            var direction = point.transform.position - transform.position;
            bullet.transform.rotation = Quaternion.Euler(direction);
            bullet.gameObject.SetActive(true);
            bullet.FlyToTarget(point, Random.Range(_minDamage, _maxDamage + 1), _damagable, this);
        }
        else
        {
            SpawnBullet();
            Shoot(point);
        }
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

    public void ReturnToPool(Bullet bullet)
    {
        bullet.gameObject.transform.SetParent(null);
        bullet.gameObject.SetActive(false);

        if (!_bulletsPool.Contains(bullet))
        {
            _bulletsPool.Add(bullet);
        }
    }
}
