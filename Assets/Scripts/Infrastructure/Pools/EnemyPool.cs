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
    private Enemy _lastDefeatedEnemy;
    private List<Enemy> _enemyList = new List<Enemy>();
    private List<Enemy> _defeatedEnemiesList = new List<Enemy>();

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine) 
    {
        _gameLoopStateMachine = gameLoopStateMachine;
    }

    public void AddEnemyToPool(Enemy enemy)
    {
        _enemyList.Add(enemy);
        EnemiesWaveCount.Value = _enemyList.Count();
    }

    public Enemy GetEnemyFromPool()
    {
        return _enemyList.Where(x => x.gameObject.activeInHierarchy == false).FirstOrDefault();
    }  

    public void ReturnToPool(Enemy enemy)
    {
        _lastDefeatedEnemy = enemy;
        _enemyList.Remove(enemy);
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
            _gameLoopStateMachine.Enter<GameBuildingState>();
            return true;
        }

        else
            return false;
    }

    private void ClearEnemyPool()
    {
        for (int i = 0; i < _enemyList.Count; i++)
            Object.Destroy(_enemyList[i]?.gameObject);

        _enemyList.Clear();

        for (int i = 0; i < _defeatedEnemiesList.Count; i++)
        {
            if (_defeatedEnemiesList[i] != _lastDefeatedEnemy)          
                Object.Destroy(_defeatedEnemiesList[i]?.gameObject);
            
            else           
                Object.Destroy(_lastDefeatedEnemy, 2f);           
        }

        _defeatedEnemiesList.Clear();
        EnemiesWaveCount.Value = _enemyList.Count();
    }
}
