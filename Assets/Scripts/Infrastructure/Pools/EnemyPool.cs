using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;
using Infrastructure.StateMachine;

public class EnemyPool : IInitializable
{
    public readonly IntReactiveProperty EnemiesWaveCount = new IntReactiveProperty(0);
    public const int PoolCapasity = 30;

    private GameLoopStateMachine _gameLoopStateMachine;
    private EnemyEntity _lastDefeatedEnemy;
    private List<EnemyEntity> _enemyList = new List<EnemyEntity>();
    private List<EnemyEntity> _defeatedEnemiesList = new List<EnemyEntity>();
    private List<IDamagable> _damageables = new List<IDamagable>();
    private Dictionary<IDamagable, float> _entities = new Dictionary<IDamagable, float>();

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine) 
    {
        _gameLoopStateMachine = gameLoopStateMachine;
    }

    public void AddEnemyToPool(EnemyEntity enemy)
    {
        _enemyList.Add(enemy);
        _damageables.Add(enemy.gameObject.GetComponent<IDamagable>());
        EnemiesWaveCount.Value = _enemyList.Count();
    }

    public EnemyEntity GetEnemyFromPool()
    {
        return _enemyList.Where(x => x.gameObject.activeInHierarchy == false).FirstOrDefault();
    }  

    public IDamagable GetEnemyFromDistance(Transform pos, float distance, IDamagable damagable)
    {
        _entities.Clear();

        var enemies = new List<IDamagable>();

        for (int i = 0; i < _damageables.Count; i++)
        {
            int j = 0;
            var position = _damageables[i].GetOrigin().position;
            if (Vector3.Distance(pos.position, position) <= distance && _damageables[i] != damagable)
            {
                var directionToTarget = position - damagable.GetOrigin().position;
                _entities.Add(_damageables[i], Vector3.Angle(position - damagable.GetOrigin().position, directionToTarget));
                j++;

                if (j >= 2)              
                    continue;                
            }
        }

        enemies.Remove(damagable);

        if (_entities.Count == 0)
        {
            return null;
        }
        else
        {
            _entities.OrderBy(x => x.Value);
            return _entities.FirstOrDefault().Key;
        }      
    }

    public void ReturnToPool(EnemyEntity enemy)
    {
        _lastDefeatedEnemy = enemy;
        _enemyList.Remove(enemy);
        _damageables.Remove(enemy.gameObject.GetComponent<IDamagable>());
        _defeatedEnemiesList.Add(enemy);
        EnemiesWaveCount.Value = _enemyList.Count();
        CheckForEmptyPool();
    }

    public void Initialize()
    {
    }

    private bool CheckForEmptyPool()
    {
        if (_enemyList.Count == 0)
        {
            ClearEnemyPool();            
            return true;
        }

        else
            return false;
    }

    private void ClearEnemyPool()
    {
        for (int i = 0; i < _defeatedEnemiesList.Count; i++)
        {
            if (_defeatedEnemiesList[i] != _lastDefeatedEnemy)
                Object.Destroy(_defeatedEnemiesList[i].gameObject);
            else
                Object.Destroy(_lastDefeatedEnemy.gameObject, 2f);
        }

        _enemyList.Clear();
        _damageables.Clear();
        _defeatedEnemiesList.Clear();
        EnemiesWaveCount.Value = _enemyList.Count();
        _gameLoopStateMachine.Enter<GameBuildingState>();
    }
}
