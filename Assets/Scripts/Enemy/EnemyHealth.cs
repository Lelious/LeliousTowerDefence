using UniRx;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	[SerializeField] private EnemyMovement _enemyMovement;
	[SerializeField] private HealthBar _hpBar;

	private FloatReactiveProperty _health;
	private FloatingTextService _floatingTextService;

	public void InitializeHealth(float maxHP, FloatReactiveProperty currentHp)
    {
		_hpBar.SetMaxHealth(maxHP);
		_health = currentHp;
		_hpBar.SetHealth(_health.Value);
		_hpBar.Hide();
	}

	public void SetFloatingTextService(FloatingTextService service) => _floatingTextService = service;

	public void ProcessDamage(Vector3 position, int damage, DamageSource source = DamageSource.Normal)
	{
		_hpBar.Show();
		_hpBar.SetHealth(_health.Value);

		if (damage >= _health.Value)
		{
			if (_health.Value > 0)
			{
				_floatingTextService.AddFloatingText($"{damage}", position, source);
			}
			_health.Value = 0;
			_hpBar.SetHealth(_health.Value);
			_hpBar.Hide();
			_enemyMovement.EnemyDeath();
		}
		else
		{
			_health.Value -= damage;
			_hpBar.SetHealth(_health.Value);
			_floatingTextService.AddFloatingText($"{damage}", position, source);
		}
	}
}
