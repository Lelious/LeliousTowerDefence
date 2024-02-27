using UniRx;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
	[SerializeField] private EnemyMovement _enemyMovement;
	[SerializeField] private EnemyHitPoint _hitPoint;	
	[SerializeField] private HealthBar _hpBar;
	[SerializeField] private Transform _origin;

	private FloatReactiveProperty _health;
	private FloatingTextService _floatingTextService;
	GameObject IDamagable.gameObject { get => gameObject; }
	public void InitializeHealth(float maxHP, FloatReactiveProperty currentHp)
    {
		_hpBar.SetMaxHealth(maxHP);
		_health = currentHp;
		_hpBar.SetHealth(_health.Value);
		_hpBar.Hide();
	}

	public void SetFloatingTextService(FloatingTextService service) => _floatingTextService = service;

	public void TakeDamage(int damage)
	{
		_hpBar.Show();
		_hpBar.SetHealth(_health.Value);

		if (damage >= _health.Value)
		{
			if (_health.Value > 0)
			{
				_floatingTextService.AddFloatingText($"{damage}", _hitPoint.transform.position, Color.white);
			}
			_health.Value = 0;
			_hpBar.SetHealth(_health.Value);
			_hpBar.Hide();
			_enemyMovement.EnemyDeath();
			_hitPoint.ReturnAttachedBulletsToPool();
			_hitPoint.SetInnactive();
		}
		else
		{
			_health.Value -= damage;
			_hpBar.SetHealth(_health.Value);
			_floatingTextService.AddFloatingText($"{damage}", _hitPoint.transform.position, Color.white);
		}
	}

	public FloatReactiveProperty GetReactiveHealthProperty() => _health;	
	public bool CanBeAttacked() => _health.Value > 0 ? true : false;	
	public EnemyHitPoint HitPoint() => _hitPoint;
    public Transform GetOrigin() => _origin;
}
