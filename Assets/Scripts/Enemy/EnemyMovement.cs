using BezierSolution;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] private EnemyEntity _enemyEntity;
	[SerializeField] private BezierWalkerWithSpeed _walker;
	[SerializeField] private EnemyVertexAnimator _animator;

	private EnemyStats _enemyStats;
	private bool _isDead;

	public void SetEnemyStats(EnemyStats stats) => _enemyStats = stats;

	public void UpdateSpeed()
	{
		_animator.SetAnimationSpeed(1f * _enemyStats.BonusSpeed.Value);
		_walker.speed = _enemyStats.Speed * _enemyStats.BonusSpeed.Value;
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
			_walker.speed = 0f;
			//_animator.SetTrigger("Death");
			_enemyEntity.ReturnToEnemyPool();
			_isDead = true;
		}
	}

	public void SetPath(BezierSpline path)
	{
		_walker.spline = path;
		_walker.travelMode = TravelMode.Loop;
	}
}
