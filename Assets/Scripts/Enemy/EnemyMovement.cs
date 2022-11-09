using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private EnemyEntity _enemyEntity;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private EnemyData _enemyData;
	[SerializeField] private Animator _animator;

	private readonly int _hashSpeed = Animator.StringToHash("Speed");
	private const float _navMeshSpeedConst = 2f;
	private EndPoint _endPoint;
	private bool _isDead;

	[Inject]
	private void Construct(EndPoint endPoint)
	{
		_endPoint = endPoint;
	}

	private void Start()
	{
		UpdateSpeed();
		SetPath();
	}

	public void UpdateSpeed()
	{
		_animator.SetFloat(_hashSpeed, _enemyData.Speed);
		_navMeshAgent.speed = _navMeshSpeedConst * _enemyData.Speed;
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
