using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _health;
	[SerializeField] private GameObject _hpBar;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Slider _hpBarSlider;
	[SerializeField] private Image _fill;
	[SerializeField] private GameObject _ragdoll;
	[SerializeField] private Sprite _mainImage;
	[SerializeField] private MenuUpdater _menuUpdater;
	[SerializeField] private string _name;
	[SerializeField] private GameObject _selection;

	private BuildCellChanger _buildCellChanger;
	private GameBottomPanel _gameBottomPanel;
	private int _armor;
	private float _currentHealth;
	private Animator _anim;
	private NavMeshAgent _navMeshAgent;

	private protected void Awake()
	{
		_gameBottomPanel = FindObjectOfType<GameBottomPanel>();
		_buildCellChanger = FindObjectOfType<BuildCellChanger>();
		_menuUpdater = FindObjectOfType<MenuUpdater>();
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_anim = GetComponent<Animator>();
		_anim.Play("Run");
		_currentHealth = _health;
		_hpBarSlider.value = 1f;
		_fill.color = _gradient.Evaluate(_hpBarSlider.normalizedValue);
	}

	public void RecieveDamage(int damage)
	{
		if (!_hpBar.activeSelf)
		{
			_hpBar.SetActive(true);
		}
		_currentHealth -= damage;
		_hpBarSlider.value = _currentHealth / _health;
		_fill.color = _gradient.Evaluate(_hpBarSlider.normalizedValue);

		if (_buildCellChanger.Selected == gameObject)
		{
			UpgradeInformation();
		}

		if (_currentHealth <= 0)
		{
			if (_buildCellChanger.Selected == gameObject)
			{
				_gameBottomPanel.HideGameMenu();
			}

			var rotation = transform.rotation;
			Instantiate(_ragdoll, transform.position, rotation);
			Destroy(gameObject);
		}
	}

	public void EnableSelectFrame()
	{
		_selection.SetActive(true);
	}

	public void DisableSelectFrame()
	{
		_selection.SetActive(false);
	}

	public void UpgradeInformation()
	{
		_menuUpdater.UpgradeInformation(_mainImage, _name, 0, 0, _armor, 0, $"{_currentHealth}/{_health}", _fill.color);
	}
}
