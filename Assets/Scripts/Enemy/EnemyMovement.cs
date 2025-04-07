using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private EnemyEntity _enemyEntity;
	[SerializeField] private NavMeshAgent _navMeshAgent;
	[SerializeField] private EnemyVertexAnimator _animator;
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
		_animator.SetAnimationSpeed(_enemyStats.Speed * _enemyStats.BonusSpeed.Value);
		_navMeshAgent.speed = _navMeshSpeedConst * _enemyStats.Speed * _enemyStats.BonusSpeed.Value;
		Debug.Log(_enemyStats.Speed * _enemyStats.BonusSpeed.Value);
		if(_enemyStats.Speed * _enemyStats.BonusSpeed.Value < 0.7f)
        {
			_animator.SetColor(new Color(0.0f, 0.73f, 1.0f));
		}
        else
        {
			_animator.SetColor(new Color(1.0f, 1.0f, 1.0f));
		}
	}

	public void EnemyDeath()
	{
		if (!_isDead)
		{
			_navMeshAgent.isStopped = true;
			//_animator.SetTrigger("Death");
			_enemyEntity.ReturnToEnemyPool();
			_isDead = true;
		}
	}

	private void SetPath()
	{
		_navMeshAgent.SetDestination(_endPoint.transform.position);
	}
}
