using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawnService : MonoBehaviour
{
	[SerializeField] private Transform _startPoint, _endPoint;

	private List<Enemy> _enemyPoolList = new List<Enemy>();
	private EnemyFactory _enemyFactory;


	[Inject]
	private void Construct(EnemyFactory enemyFactory)
	{
		_enemyFactory = enemyFactory;
	}
	private protected void Awake()
	{
		InitializeEnemyPool();
		StartCoroutine(ShootingRoutine());
	}

	public int GetWaveCount()
	{
		return _enemyPoolList.Count;
	}

	private IEnumerator ShootingRoutine()
	{
		while (_enemyPoolList.Count > 0)
		{
			yield return new WaitForSeconds(2f);
			_enemyPoolList.RemoveAll(x => x == null);
			var enemy = _enemyPoolList.Find(x => x.gameObject.activeInHierarchy == false);
			enemy.gameObject.SetActive(true);
			enemy.SetPath(_endPoint.position);
		}
	}

	private void InitializeEnemyPool()
	{
		_enemyFactory.Initialize();

		for (int i = 0; i < 30; i++)
		{
			var enemy = _enemyFactory.CreateEnemy(_startPoint.position);
			enemy.gameObject.SetActive(false);
			_enemyPoolList.Add(enemy);
		}
	}
}
