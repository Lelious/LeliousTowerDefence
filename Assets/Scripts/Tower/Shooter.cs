using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Tower _tower;
    [SerializeField] private EnemyCheck _enemyChecker;

    private int _minDamage, _maxDamage;
    private List<GameObject> _enemy;
    private Transform _target;
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
        _enemy = _enemyChecker.GetEnemies();

        for (int i = 0; i < _enemy.Count; i++)
        {
            if (_enemy[i] != null)
            {
                _target = _enemy[i].transform;
                Shoot();
                break;
            }
        }
    }

    public void Shoot()
    {
        var newBullet = Instantiate(_bullet, transform.position, Quaternion.identity);
        newBullet.FlyToTarget(_target, Random.Range(_minDamage, _maxDamage + 1));
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
