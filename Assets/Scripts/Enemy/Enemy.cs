using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;

public class Enemy : MonoBehaviour
{
	[SerializeField] private Sprite _mainImage;
	[SerializeField] private string _name;
	[SerializeField] private GameObject _selection;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _navMeshSpeedConst = 2f;

	private EnemyPool _enemyPool;
	private float _speed = 1f;
	private readonly int _hashSpeed = Animator.StringToHash("Speed");

	[Inject]
	private void Construct(EnemyPool enemyPool)
	{
		_enemyPool = enemyPool;
	}

	private protected void LateUpdate()
	{
		_animator.SetFloat(_hashSpeed, _speed);
		_navMeshAgent.speed = _navMeshSpeedConst * _speed;
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
		_enemyPool.ReturnToPool(this);
		StartCoroutine(DelayedDisableRoutine());
	}

	private IEnumerator DelayedDisableRoutine()
	{
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
	}
}
