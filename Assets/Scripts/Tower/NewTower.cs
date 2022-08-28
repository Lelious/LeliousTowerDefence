using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class NewTower : MonoBehaviour
{
	[SerializeField] private TowerData _towerData;
	[SerializeField] private Shooter _shooter;
	[SerializeField] private GameObject _rangeBoarder;

	private ParticleSystem _dustPatricles;
	private Slider _buildingProgress;
	private Image _fill;
	private UIParentCanvas _uiParentCanvas;
	private BuildCellInitializer _buildCellChanger;
	private Vector3 _endPosition;
	private bool _isBuilded;

	private protected void Awake()
	{
		_uiParentCanvas = Instantiate(_towerData.UIParentCanvas, transform.position, Quaternion.identity, transform).
			GetComponent<UIParentCanvas>();
		_dustPatricles = Instantiate(_towerData.DustParticles, transform.position, Quaternion.identity, transform);
		_buildingProgress = _uiParentCanvas.BuildingProgressSlider;
		_fill = _uiParentCanvas.BuildingProgressSliderFill;			
		_buildingProgress.maxValue = _towerData.BuildingTime;
		_fill.color = _towerData.GradientColor.Evaluate(1f);
		_endPosition = transform.position;
		_endPosition.y += 2.8f;
		_dustPatricles.Stop();
		var main = _towerData.DustParticles.main;
		main.duration = _towerData.BuildingTime;
	}

	public void TowerBuild()
	{
		_towerData.UIParentCanvas.SetActive(true);
		_towerData.DustParticles.Play();
		_buildingProgress.DOValue(_towerData.BuildingTime, _towerData.BuildingTime).SetEase(Ease.Linear).OnUpdate(() =>
		{
			_fill.color = _towerData.GradientColor.Evaluate(_buildingProgress.normalizedValue);
		}).OnComplete(() =>
		{
			_dustPatricles.Clear();
			_uiParentCanvas.gameObject.SetActive(false);
			_shooter.gameObject.SetActive(true);
			_isBuilded = true;
		});

		transform.DOMove(_endPosition, _towerData.BuildingTime);
	}
	public void ShowRange()
	{
		if (_shooter.gameObject.activeSelf)
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
		return _towerData.Name;
	}
	public Sprite GetTowerImage()
	{
		return _towerData.MainImage;
	}
	public int GetMinDamage()
	{
		return _towerData.MinimalDamage;
	}
	public int GetMaxDamage()
	{
		return _towerData.MaximumDamage;
	}
	public float GetAttackSpeed()
	{
		return _towerData.AttackSpeed;
	}
	public string GetHealth()
	{
		return $"{string.Format("{0:f0}", _buildingProgress.value)}/{_towerData.BuildingTime}";
	}
	public Color GetHealthColor()
	{
		return _fill.color;
	}

	public int GetCost()
	{
		return _towerData.Cost;
	}

	public bool GetBuildStatus()
	{
		return _isBuilded;
	}
}
