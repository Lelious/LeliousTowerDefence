using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
	public Sprite mainImage;
	public string towerName;
	public GameObject towerUpgrade;

	[SerializeField] private Transform _tower;
	[SerializeField] private float _buildingTime;
	[SerializeField] private ParticleSystem _dustParticles;
	[SerializeField] private GameObject _sliderParrent;
	[SerializeField] private Slider _buildingProgress;
	[SerializeField] private Gradient _gradient;
	[SerializeField] private Image _fill;

	private Vector3 _endPosition;

	private void Awake()
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
		_buildingProgress.DOValue(_buildingTime, _buildingTime).OnUpdate(() => 
		{ 
			_fill.color = _gradient.Evaluate(_buildingProgress.normalizedValue); 
		}).OnComplete(() => 
		{ 
			_sliderParrent.SetActive(false);
		});
		_tower.DOMove(_endPosition, _buildingTime);
	}
}
