using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private EnemyEntity _enemyEntity;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private Animator _animator;
	private EnemyStats _enemyStats;
	private readonly int _hashSpeed = Animator.StringToHash("Speed");
	private const float _navMeshSpeedConst = 2f;
	private EndPoint _endPoint;
	private bool _isDead;

	[Inject]
	private void Construct(EndPoint endPoint)
	{
		_endPoint = endPoint;
	}

	public void SetEnemyStats(EnemyStats stats) => _enemyStats = stats;

	private void Start()
	{
		SetPath();
	}

	public void UpdateSpeed()
	{
		_animator.SetFloat(_hashSpeed, _enemyStats.Speed * _enemyStats.BonusSpeed.Value);
		_navMeshAgent.speed = _navMeshSpeedConst * _enemyStats.Speed * _enemyStats.BonusSpeed.Value;
	}

	public void EnemyDeath()
	{
		if (!_isDead)
		{
			_navMeshAgent.isStopped = true;
			_animator.SetTrigger("Death");
			_enemyEntity.ReturnToEnemyPool();
			_isDead = true;
		}
	}

	private void SetPath()
	{
		_navMeshAgent.SetDestination(_endPoint.transform.position);
	}
}
