using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
	[SerializeField] private EnemyMovement _enemyMovement;
	[SerializeField] private EnemyHitPoint _hitPoint;	
	[SerializeField] private EnemyData _enemyData;
	[SerializeField] private HealthBar _hpBar;

	private float _health;

	private protected void Awake()
	{
		_health = _enemyData.Hp;
		_hpBar.SetMaxHealth(_health);
		_hpBar.SetHealth(_health);
		_hpBar.Hide();
	}

	public void TakeDamage(float damage)
	{
		_hpBar.Show();

		if (damage >= _health)
		{
			_health = 0;
			_hpBar.SetHealth(_health);
			_hpBar.Hide();
			_enemyMovement.EnemyDeath();
		}
		else
		{
			_health -= damage;
			_hpBar.SetHealth(_health);
		}
	}

	public bool CanBeAttacked() => _health > 0 ? true : false;	
	public EnemyHitPoint HitPoint() => _hitPoint;

	private protected void OnDisable()
	{
		_hitPoint.ReturnAttachedBulletsToPool();
	}
}
