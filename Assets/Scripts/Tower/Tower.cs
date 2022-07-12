using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
	[SerializeField] private Sprite _mainImage;
	[SerializeField] private string _towerName;
	[SerializeField] private GameObject _towerUpgrade;
	[SerializeField] private int _towerCost;

	[SerializeField] private Transform _tower;
	[SerializeField] private float _buildingTime;
	[SerializeField] private ParticleSystem _dustParticles;
	[SerializeField] private GameObject _sliderParrent;
	[SerializeField] private Slider _buildingProgress;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Image _fill;

	[SerializeField] private int _minDamage;
	[SerializeField] private int _maxDamage;
	[SerializeField] private int _armor;
	[SerializeField] private float _attackSpeed;
	[SerializeField] private GameObject _shooter;
	[SerializeField] private GameObject _rangeBoarder;

	private BuildCellChanger _buildCellChanger;
	private Vector3 _endPosition;
	private bool _isBuilded;

	private protected void Awake()
	{		
		_buildingProgress.maxValue = _buildingTime;
		_fill.color = _gradient.Evaluate(1f);
		_endPosition = _tower.position;
		_endPosition.y += 1.5f;
		_dustParticles.Stop();
		var main = _dustParticles.main;
		main.duration = _buildingTime;
	}

	public void TowerBuild()
	{
		_sliderParrent.SetActive(true);
		_dustParticles.Play();
		_buildingProgress.DOValue(_buildingTime, _buildingTime).SetEase(Ease.Linear).OnUpdate(() => 
		{ 
			_fill.color = _gradient.Evaluate(_buildingProgress.normalizedValue); 
		}).OnComplete(() => 
		{
			_dustParticles.Clear();
			_sliderParrent.SetActive(false);
			_shooter.SetActive(true);
			_isBuilded = true;
		});
		_tower.DOMove(_endPosition, _buildingTime);
	}
	public void ShowRange()
	{
		if (_shooter.activeSelf)
		{
			_rangeBoarder.SetActive(true);
		}
	}
	public void DisableRange()
	{
		_rangeBoarder.SetActive(false);
	}
	public string GetTowerName()
	{
		return _towerName;
	}
	public Sprite GetTowerImage()
	{
		return _mainImage;
	}
	public int GetMinDamage()
	{
		return _minDamage;
	}
	public int GetMaxDamage()
	{
		return _maxDamage;
	}
	public float GetAttackSpeed()
	{
		return _attackSpeed;
	}
	public string GetHealth()
	{
		return $"{string.Format("{0:f0}", _buildingProgress.value)}/{_buildingTime}";
	}
	public Color GetHealthColor()
	{
		return _fill.color;
	}

	public int GetCost()
	{
		return _towerCost;
	}

	public bool GetBuildStatus()
	{
		return _isBuilded;
	}
}
