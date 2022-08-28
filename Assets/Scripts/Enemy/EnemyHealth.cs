using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
	public Transform ShootPoint;

	[SerializeField] private float _health;
	[SerializeField] private GameObject _hpBar;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Slider _hpBarSlider;
	[SerializeField] private Image _fill;
	[SerializeField] private Animator _animator;
	[SerializeField] private NavMeshAgent _agent;

	private List<Bullet> _bulletsList = new List<Bullet>();
	private float _currentHealth;
	private bool _isDead;

	private protected void Awake()
	{
		_currentHealth = _health;
		_hpBarSlider.value = 1f;
		_fill.color = _gradient.Evaluate(_hpBarSlider.normalizedValue);
	}

	public void TakeDamage(int damage)
	{
		if (!_hpBar.activeInHierarchy && !_isDead)
		{
			_hpBar.SetActive(true);
		}
		_currentHealth -= damage;
		_hpBarSlider.value = _currentHealth / _health;
		_fill.color = _gradient.Evaluate(_hpBarSlider.normalizedValue);

		//if (_buildCellChanger.Selected == gameObject)
		//{
		//	UpgradeInformation();
		//}

		if (_currentHealth <= 0)
		{
			//if (_buildCellChanger.Selected == gameObject)
			//{
			//	_gameBottomPanel.HideGameMenu();
			//}
			if (!_isDead)
			{
				_animator.SetTrigger("Death");
				_agent.isStopped = true;
				_isDead = true;
				_hpBar.gameObject.SetActive(false);
				ReturnAllBullets();
			}

			Destroy(gameObject, 2f);
		}
	}

	public bool CanBeAttacked()
	{
		return _currentHealth > 0 ? true : false;
	}

	public void RemoveBullet(Bullet bullet)
	{
		if (_bulletsList.Contains(bullet))
		{
			_bulletsList.Remove(bullet);
		}
	}

	private void ReturnAllBullets()
	{
		for (int i = 0; i < _bulletsList.Count; i++)
		{
			if (_bulletsList[i] != null)
			{
				_bulletsList[i].ReturnBullet();
				_bulletsList.Remove(_bulletsList[i]);
			}
		}
	}

	public void ApplyBullet(Bullet bullet)
	{
		if (!_bulletsList.Contains(bullet))
		{
			_bulletsList.Add(bullet);
		}
	}
}
