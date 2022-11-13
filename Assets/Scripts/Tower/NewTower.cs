using UnityEngine;
using DG.Tweening;
using UniRx;

public class NewTower : MonoBehaviour
{
	public FloatReactiveProperty Health = new FloatReactiveProperty();

	[SerializeField] private TowerData _towerData;
	[SerializeField] private Shooter _shooter;
	[SerializeField] private GameObject _rangeBoarder;
	[SerializeField] private HealthBar _buildingProgress;
	[SerializeField] private Transform _towerObject;

	private ParticleSystem _dustPatricles;	
	private float _endYPosition = -0.6f;
	private bool _isBuilded;
	private float _buildProgress = 0.01f;

	private protected void Awake()
	{
		TowerBuild();
	}

	private void TowerBuild()
	{
		_buildingProgress.SetMaxHealth(_towerData.BuildingTime);
		_buildingProgress.SetHealth(0.01f);
		_dustPatricles = Instantiate(_towerData.DustParticles, transform.position, Quaternion.identity, transform);
		_dustPatricles.Stop();
		var main = _towerData.DustParticles.main;
		main.duration = _towerData.BuildingTime;
		_towerData.DustParticles.Play();
		DOTween.To(() => _buildProgress, x => _buildProgress = x, _towerData.BuildingTime, _towerData.BuildingTime)
			.OnUpdate(() =>
			{
				Health.Value = _buildProgress;
				_buildingProgress.SetHealth(_buildProgress);
			}).OnComplete(() =>
				{
					_dustPatricles.Clear();
					_buildingProgress.Hide();
					_shooter.gameObject.SetActive(true);
					_isBuilded = true;
				}).SetEase(Ease.Linear);
		_towerObject.DOLocalMoveY(_endYPosition, _towerData.BuildingTime);
	}

	public void ShowRange()
	{
		if (_shooter.gameObject.activeSelf)
		{
			_rangeBoarder.SetActive(true);
		}
	}

	public void HideRange()
	{
		_rangeBoarder.SetActive(false);
	}	

	public TowerData GetTowerData()	=> _towerData;
}
