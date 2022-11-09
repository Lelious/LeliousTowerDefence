using UnityEngine;
using DG.Tweening;

public class NewTower : MonoBehaviour
{
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
		return $"{string.Format("{0:f0}", _buildProgress)}/{_towerData.BuildingTime}";
	}

	public int GetCost()
	{
		return _towerData.Cost;
	}

	public bool GetBuildStatus()
	{
		return _isBuilded;
	}

	public TowerData GetTowerData()	=> _towerData;

	public Vector3 GetPosition()
	{
		throw new System.NotImplementedException();
	}

	public void Touch()
	{
		throw new System.NotImplementedException();
	}
}
