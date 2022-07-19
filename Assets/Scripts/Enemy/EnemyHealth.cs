using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour, IDamagable
{
	[SerializeField] private float _health;
	[SerializeField] private GameObject _hpBar;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Slider _hpBarSlider;
	[SerializeField] private Image _fill;

	private float _currentHealth;
	private protected void Awake()
	{
		_currentHealth = _health;
		_hpBarSlider.value = 1f;
		_fill.color = _gradient.Evaluate(_hpBarSlider.normalizedValue);
	}

	public void TakeDamage(int damage)
	{
		if (!_hpBar.activeSelf)
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

			Destroy(gameObject);
		}
	}
}
