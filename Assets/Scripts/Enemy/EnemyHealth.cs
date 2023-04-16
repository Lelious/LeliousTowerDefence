using UniRx;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
	[SerializeField] private EnemyMovement _enemyMovement;
	[SerializeField] private EnemyHitPoint _hitPoint;	
	[SerializeField] private EnemyData _enemyData;
	[SerializeField] private HealthBar _hpBar;
	[SerializeField] private Transform _origin;

	private FloatReactiveProperty _health = new FloatReactiveProperty();

	private protected void Awake()
	{
		_health.Value = _enemyData.Hp;
		_hpBar.SetMaxHealth(_health.Value);
		_hpBar.SetHealth(_health.Value);
		_hpBar.Hide();
	}

	public void TakeDamage(int damage)
	{
		_hpBar.Show();

		if (damage >= _health.Value)
		{
			_health.Value = 0;
			_hpBar.SetHealth(_health.Value);
			_hpBar.Hide();
			_enemyMovement.EnemyDeath();
			_hitPoint.ReturnAttachedBulletsToPool();
		}
		else
		{
			_health.Value -= damage;
			_hpBar.SetHealth(_health.Value);
		}
	}

	public FloatReactiveProperty GetReactiveHealthProperty() => _health;	
	public bool CanBeAttacked() => _health.Value > 0 ? true : false;	
	public EnemyHitPoint HitPoint() => _hitPoint;
    public Transform GetOrigin() => _origin;
}
