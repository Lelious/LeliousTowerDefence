using System.Collections;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private TowerData _towerData;
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 100)] private int _poolSize;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Transform _cannonToRotate;
    [Inject] private PoolService _poolService;
    private float _delayBetweenShoots;

    private protected void Awake()
    {
        if (_shootingPoint == null)      
            _shootingPoint = transform;       
        _delayBetweenShoots = _towerData.AttackSpeed;
        _enemyChecker.SetAttackRange(_towerData.AttackRadius);
        for (int i = 0; i < _poolSize; i++)
        {
            var bullet = CreateBullet();
            bullet.SetBulletPool(_poolService);
        }       

        StartCoroutine(ShootingRoutine());
    }

    public bool DetectEnemy()
    {
        for (int i = 0; i < _enemyChecker.EnemiesList.Count; i++)
        {
            if (_enemyChecker.EnemiesList[i] != null && _enemyChecker.EnemiesList[i].CanBeAttacked())
            {
                Shoot(_enemyChecker.EnemiesList[i]);

                if (_cannonToRotate != null)
                {
                    var originPoint = _enemyChecker.EnemiesList[i].GetOrigin().position;
                    _cannonToRotate.DOLookAt(new Vector3(originPoint.x, _cannonToRotate.position.y, originPoint.z), 0.1f);
                }
                return true;
            }
        }
        return false;       
    }

    public void Shoot(IDamagable damagable)
    {
        var bullet = _poolService.GetBulletFromPool(_towerData.BulletPrefab.GetType());

        if (bullet == null)
        {
            bullet = CreateBullet();
            bullet.SetBulletPool(_poolService, false);
        }

        bullet.SetBulletParameters(_towerData, _shootingPoint.position);
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
            yield return new WaitForSeconds(_delayBetweenShoots/2);
        }
    }

    private Bullet CreateBullet() => Instantiate(_towerData.BulletPrefab);
}
