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

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine) 
    {
        _gameLoopStateMachine = gameLoopStateMachine;
    }

    public void AddEnemyToPool(EnemyEntity enemy)
    {
        _enemyList.Add(enemy);
        EnemiesWaveCount.Value = _enemyList.Count();
        Debug.Log("AddingEnemyToPool");
    }

    public EnemyEntity GetEnemyFromPool()
    {
        return _enemyList.Where(x => x.gameObject.activeInHierarchy == false).FirstOrDefault();
    }  

    public void ReturnToPool(EnemyEntity enemy)
    {
        _lastDefeatedEnemy = enemy;
        _enemyList.Remove(enemy);
        _defeatedEnemiesList.Add(enemy);
        EnemiesWaveCount.Value = _enemyList.Count();
        CheckForEmptyPool();
        Debug.Log("AddingEnemyToDefeatedPool");
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
        Debug.Log($"Clearing Pool, EnemiesInEnemyList:{_enemyList.Count}");
        Debug.Log($"Clearing Pool, EnemiesInDefeatedList:{_defeatedEnemiesList.Count}");
        for (int i = 0; i < _defeatedEnemiesList.Count; i++)
        {
            if (_defeatedEnemiesList[i] != _lastDefeatedEnemy)
                Object.Destroy(_defeatedEnemiesList[i].gameObject);
            else
                Object.Destroy(_lastDefeatedEnemy.gameObject, 2f);
        }

        _enemyList.Clear();
        _defeatedEnemiesList.Clear();
        EnemiesWaveCount.Value = _enemyList.Count();
        Debug.Log($"Pool cleared, EnemiesInEnemyList:{_enemyList.Count}, {_defeatedEnemiesList.Count}");
        _gameLoopStateMachine.Enter<GameBuildingState>();
    }
}
