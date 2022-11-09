using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

public class Enemy : MonoBehaviour
{
	[SerializeField] private EnemyData _enemyData;
	[SerializeField] private Animator _animator;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private GameObject _selection;
	[SerializeField] private HealthBar _hpBar;

	private GameManager _gameManager;
	private EnemyPool _enemyPool;
	private const float _navMeshSpeedConst = 2f;
	private readonly int _hashSpeed = Animator.StringToHash("Speed");

	//[Inject]
	//private void Construct(EnemyPool enemyPool, GameManager gameManager)
	//{
	//	_enemyPool = enemyPool;
	//	_gameManager = gameManager;
	//	_hpBar.SetHealth(_enemyData.Hp);
	//}

	private protected void LateUpdate()
	{
		_animator.SetFloat(_hashSpeed, _enemyData.Speed);
		_navMeshAgent.speed = _navMeshSpeedConst * _enemyData.Speed;
	}	

	public void EnableSelectFrame()
	{
		_selection.SetActive(true);
	}

	public void DisableSelectFrame()
	{
		_selection.SetActive(false);
	}

	public void UpgradeInformation()
	{
		//_menuUpdater.UpgradeInformation(_mainImage, _name, 0, 0, _armor, 0, $"{_currentHealth}/{_health}", _fill.color);
	}

	public void SetPath(Vector3 targetToMove)
	{
		_navMeshAgent.SetDestination(targetToMove);
	}

	public void SetPool(EnemyPool pool) => _enemyPool = pool;

	public void ReturnToEnemyPool()
	{
		//_enemyPool.ReturnToPool(this);
		_gameManager.AddGold(_enemyData.Worth);
		StartCoroutine(DelayedDisableRoutine());
	}

	private IEnumerator DelayedDisableRoutine()
	{
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
	}
}
