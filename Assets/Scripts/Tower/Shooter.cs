using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class Shooter : MonoBehaviour, IShoot
{
    public GameObject CurrentTarget;
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 100)] private int _poolSize;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Transform _cannonToRotate;
    [Inject] private PoolService _poolService;
    [Inject] private EnemyPool _enemyPool;
    private bool _isPoolCreated;
    private TowerData _towerData;
    private float _delayBetweenShoots;

    public void SetTowerData(TowerData data) => _towerData = data;
    public void ClearAmmo() => _poolService.RemoveBulletsFromPool(_towerData.BulletPrefab.GetType(), _poolSize);  

    public bool DetectEnemy()
    {
        var enemies = _enemyChecker.GetEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                for (int j = 0; j < _towerData.TargetsCount; j++)
                {
                    if (enemies.Count >= _towerData.TargetsCount)
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

                if (_cannonToRotate != null)
                {
                    var originPoint = enemies[i].GetOrigin().position;
                    _cannonToRotate.DOLookAt(new Vector3(originPoint.x, _cannonToRotate.position.y, originPoint.z), 0.1f);
                }
                return true;
            }
        }
        return false;       
    }

    public void Shoot(IDamagable damagable)
    {
        CurrentTarget = damagable.GetOrigin().gameObject;
        var bullet = _poolService.GetBulletFromPool(_towerData.BulletPrefab.GetType());
        if (bullet == null)
        {
            bullet = CreateBullet();
            bullet.SetBulletPool(_poolService, false);
        }

        bullet.SetBulletParameters(_towerData, _enemyPool, _shootingPoint.position);
        bullet.SetTarget(damagable);
        bullet.FlyOnTarget();
    }       

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            while (DetectEnemy())
            {
                yield return new WaitForSeconds(_delayBetweenShoots);
            }
            yield return new WaitForSeconds(_delayBetweenShoots/5);
        }
    }

    private Bullet CreateBullet() => Instantiate(_towerData.BulletPrefab);

    private protected void OnEnable()
    {
        if (_shootingPoint == null)
            _shootingPoint = transform;
        _delayBetweenShoots = _towerData.AttackSpeed;
        _enemyChecker.SetAttackRange(_towerData.AttackRadius);

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
