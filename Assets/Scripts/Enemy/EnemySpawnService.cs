using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemySpawnService : MonoBehaviour
{
	[SerializeField] private Transform _startPoint, _endPoint;

	private EnemyFactory _enemyFactory;

	[Inject]
	private void Construct(EnemyFactory enemyFactory)
	{
		_enemyFactory = enemyFactory;
	}
	private protected void Awake()
	{
		StartCoroutine(ShootingRoutine());
	}

	private IEnumerator ShootingRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(2f);
			var enemy = _enemyFactory.CreateEnemy(_startPoint.position);
			enemy.SetPath(_endPoint.position);
		}
	}
}
