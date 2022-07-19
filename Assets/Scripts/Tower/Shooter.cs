using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Tower _tower;
    [SerializeField] private EnemyCheck _enemyChecker;

    private IDamagable _damagable;
    private int _minDamage, _maxDamage;
    private float _delayBetweenShoots;

    private protected void Awake()
    {
        _delayBetweenShoots = _tower.GetAttackSpeed();
        _minDamage = _tower.GetMinDamage();
        _maxDamage = _tower.GetMaxDamage();

        StartCoroutine(ShootingRoutine());
    }
    public void DetectEnemy()
    {
        if (_enemyChecker.EnemiesList.Count > 0)
        {
            var actualTarget = _enemyChecker.EnemiesList[0];

            if (actualTarget != null)
            {
                actualTarget.TryGetComponent<IDamagable>(out _damagable);
                Shoot(actualTarget.transform);
            }
            else
            {
                _enemyChecker.EnemiesList.Remove(actualTarget);
            }
        }       
    }

    public void Shoot(Transform point)
    {
        var newBullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        newBullet.FlyToTarget(point, Random.Range(_minDamage, _maxDamage + 1), _damagable);
    }

    public void SpawnBullet()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            DetectEnemy();
            yield return new WaitForSeconds(_delayBetweenShoots);
        }
    }
}
