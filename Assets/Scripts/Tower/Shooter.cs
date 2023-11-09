using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 100)] private int _poolSize;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Transform _cannonToRotate;
    [Inject] private PoolService _poolService;
    [Inject] private EnemyPool _enemyPool;
    [SerializeField] private TowerStats _towerStats;
    private bool _isPoolCreated;

    public void SetTowerData(TowerStats stats)
    {
        _towerStats = stats;
    }

    public void ClearAmmo() => _poolService.RemoveBulletsFromPool(_towerStats.BulletPrefab.GetType(), _poolSize);  

    public bool DetectEnemy()
    {
        var enemies = _enemyChecker.GetEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {                
                if (_cannonToRotate != null)
                {
                    var originPoint = enemies[i].GetOrigin().position;
                    _cannonToRotate.DOLookAt(new Vector3(originPoint.x, _cannonToRotate.position.y, originPoint.z), 0.1f).OnComplete(()=>
                    {
                        if (enemies[i] != null)
                        {
                            for (int j = 0; j < _towerStats.TargetsCount; j++)
                            {
                                if (enemies.Count >= _towerStats.TargetsCount)
                                {
                                    Shoot(enemies[j]);
                                }
                                else
                                {
                                    if (j < enemies.Count)
                                    {
                                        Shoot(enemies[j]);
                                    }
                                }
                            }
                        }
                    });
                }
            else
            {
                if (enemies[i] != null)
                {
                    for (int j = 0; j < _towerStats.TargetsCount; j++)
                    {
                        if (enemies.Count >= _towerStats.TargetsCount)
                        {
                            Shoot(enemies[j]);
                        }
                        else
                        {
                            if (j < enemies.Count)
                            {
                                Shoot(enemies[j]);
                            }
                        }
                    }
                }
            }
                return true;          
        }
        return false;       
    }

    public void Shoot(IDamagable damagable)
    {
        var bullet = _poolService.GetBulletFromPool(_towerStats.BulletPrefab.GetType());
        if (bullet == null)
        {
            bullet = CreateBullet();
            bullet.SetBulletPool(_poolService, false);
        }

        bullet.SetBulletParameters(_towerStats, _enemyPool, _shootingPoint.position);
        bullet.SetTarget(damagable);
        bullet.FlyOnTarget();
    }       

    private IEnumerator ShootingRoutine()
    {
        float waitTime;

        while (true)
        {
            while (DetectEnemy())
            {
                waitTime = (_towerStats.AttackSpeed + _towerStats.BonusAttackSpeed.Value) / 100;
                yield return new WaitForSeconds(1 / waitTime);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private Bullet CreateBullet() => Instantiate(_towerStats.BulletPrefab);

    private protected void OnEnable()
    {
        if (_shootingPoint == null)
            _shootingPoint = transform;
        _enemyChecker.SetAttackRange(_towerStats.AttackRadius);

        if (!_isPoolCreated)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                var bullet = CreateBullet();
                bullet.SetBulletPool(_poolService);
            }
            _isPoolCreated = !_isPoolCreated;
        }

        StartCoroutine(ShootingRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
