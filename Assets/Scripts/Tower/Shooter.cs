using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System.Linq;

public class Shooter : MonoBehaviour, IShoot
{
    [SerializeField] private EnemyCheck _enemyChecker;
    [SerializeField, Range(1, 100)] private int _poolSize;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private Transform _cannonToRotate;
    [Inject] private PoolService _poolService;
    [Inject] private EnemyPool _enemyPool;
    [Inject] private BuffService _buffService;
    [Inject] private TowerFactory _towerFactory;
    [SerializeField] private TowerStats _towerStats;

    private TowerAbility _buffAbility;
    private int _aimCounter;
    private bool _isPoolCreated;

    public void SetTowerData(TowerStats stats)
    {
        _towerStats = stats;
        _buffAbility = _towerStats.Abilities.Find(x => x.AppliedTarget == AppliedTarget.Tower);
    }

    public void ClearAmmo() => _poolService.RemoveBulletsFromPool(_towerStats.BulletType, _poolSize);  

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
        var bullet = _poolService.GetBulletFromPool(_towerStats.BulletType);

        if (bullet == null)
        {
            bullet = CreateBullet();
            bullet.SetBulletType(_towerStats.BulletType);
            bullet.SetBulletPool(_poolService, false);
        }

        bullet.SetBulletParameters(_towerStats, _enemyPool, _buffService, _shootingPoint.position, this, _buffAbility == null ? false : true);
        bullet.SetTarget(damagable);
        bullet.SetActive();
    }       

    public void RegisterAim()
    {
        _aimCounter++;

        if (_aimCounter >= _buffAbility.AttacksToTrigger)
        {
            _aimCounter = 0;
            var towers = _towerFactory.GetEffectableTower(transform.position, _buffAbility.MaxDistance);

            if (towers.Count > 0)
            {
                List<IEffectable> priorityList = new();

                for (int i = 0; i < towers.Count; i++)
                {
                    if (towers[i].GetEffect(_buffAbility.Data.EffectType) == null)
                    {
                        _buffService.ApplyEffect(towers[i], new Buff(towers[i], _buffAbility.Data));
                        return;
                    }
                    else
                    {
                        priorityList.Add(towers[i]);
                    }
                }

                priorityList.OrderBy(x => x.GetEffect(_buffAbility.Data.EffectType).GetDuration());
                _buffService.ApplyEffect(priorityList[0], new Buff(priorityList[0], _buffAbility.Data));

                return;
            }
        }
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
                bullet.SetBulletType(_towerStats.BulletType);
                bullet.SetBulletPool(_poolService);
                _poolService.AddBulletToPool(bullet.GetBulletType(), bullet);
            }
            _isPoolCreated = !_isPoolCreated;
        }

        StartCoroutine(ShootingRoutine());
    }

    private void OnDisable() => StopAllCoroutines();
}
