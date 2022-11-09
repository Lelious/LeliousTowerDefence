using Infrastructure.StateMachine;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

public class GameSpawnState : State
{
	private readonly CompositeDisposable _disposables = new CompositeDisposable();
	private EnemyFactory _enemyFactory;
	private EnemyPool _enemyPool;

	[Inject]
	private void Construct(EnemyFactory enemyFactory, EnemyPool enemyPool)
	{
		_enemyFactory = enemyFactory;
		_enemyPool = enemyPool;
	}

	public GameSpawnState(GameLoopStateMachine gameLoopStateMachine) : base(gameLoopStateMachine) { }

	public override void Enter()
	{
		_enemyFactory.CreateEnemy(EnemyPool.PoolCapasity);

		Observable
			.FromCoroutine(SpawningRoutine)
			.Subscribe()
			.AddTo(_disposables);
	}

	public override void Exit()
	{
		_disposables.Clear();
	}

	private IEnumerator SpawningRoutine()
	{
		int iterator = 0;

		yield return new WaitForSeconds(1f);

		while (iterator < EnemyPool.PoolCapasity)
		{
			var enemy = _enemyPool.GetEnemyFromPool();
			enemy.gameObject.SetActive(true);
			iterator++;
			yield return new WaitForSeconds(1.5f);
		}
	}
}
