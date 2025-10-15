using Infrastructure.StateMachine;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class GameSpawnState : State
{
	private readonly CompositeDisposable _disposables = new CompositeDisposable();
	private System.IDisposable _disposableEntity;
	private EnemyPool _enemyPool;

	[Inject]
	private void Construct(EnemyPool enemyPool)
	{
		_enemyPool = enemyPool;
	}

	public GameSpawnState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { }

	public override void Enter()
	{
		//_disposableEntity?.Dispose();
		//_enemyFactory.IncreaceWaveCounter();
		//_enemyFactory.CreateEnemy(EnemyPool.PoolCapasity);
		//_disposableEntity = Observable
		//	.FromCoroutine(SpawningRoutine)
		//	.Subscribe()
		//	.AddTo(_disposables);
	}

	public override void Exit()
	{
		_disposables.Clear();
		_enemyPool.ClearEnemyPool();
	}
}
