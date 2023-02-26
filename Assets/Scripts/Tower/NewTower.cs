using UnityEngine;
using DG.Tweening;
using UniRx;

public class NewTower : MonoBehaviour
{
	public FloatReactiveProperty Health = new FloatReactiveProperty();

	[SerializeField] private Shooter _shooter;
	[SerializeField] private GameObject _rangeBoarder;
	[SerializeField] private HealthBar _buildingProgress;
	[SerializeField] private Transform _towerObject;
	[SerializeField] private float _endOffsetY = -0.6f;

	private TowerData _towerData;
	private ParticleSystem _dustPatricles;
	private BuildingCell _buildingCell;
	private float _buildProgress = 0.01f;

	public void TowerBuild(BuildingCell cell)
	{
		_buildingCell = cell;
		_buildingProgress.SetMaxHealth(_towerData.BuildingTime);
		_buildingProgress.SetHealth(0.01f);
		_dustPatricles = Instantiate(_towerData.DustParticles, transform.position, Quaternion.identity, transform);
		_dustPatricles.Stop();
		_shooter.SetTowerData(_towerData);
		var main = _towerData.DustParticles.main;
		main.duration = _towerData.BuildingTime;
		_towerData.DustParticles.Play();
		_buildingCell.Untouch();

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
					_buildingCell.EnableUpgrades();

					if (_buildingCell.IsTouched())
                    {
						ShowRange();						
					}

                }).SetEase(Ease.Linear);
		_towerObject.DOLocalMoveY(_endOffsetY, _towerData.BuildingTime);
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

	public void SetTowerData(TowerData data) => _towerData = data;
}
