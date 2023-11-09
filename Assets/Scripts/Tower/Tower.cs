using UnityEngine;
using DG.Tweening;
using UniRx;
using System.Collections.Generic;

public class Tower : MonoBehaviour, IEffectable
{
	[SerializeField] private Transform _sphere;
	[SerializeField] private Shooter _shooter;
	[SerializeField] private GameObject _rangeBoarder;
	[SerializeField] private HealthBar _buildingProgress;
	[SerializeField] private Transform _towerObject;
	[SerializeField] private float _endOffsetY = -0.6f;
	[SerializeField] private List<GameObject> _upgradablesList = new();	
	[SerializeField] private TowerStats _stats;
	[SerializeField] private ObjectInstanceFromNull _fireBuff, _waterBuff;

	private List<IEffect> _effects = new();
	private ParticleSystem _dustPatricles;
	private BuildingCell _buildingCell;
	private float _buildProgress = 0.01f;
	private float _startOffsetY;

    private void Awake() => _startOffsetY = _towerObject.position.y;	

    public void TowerBuild(BuildingCell cell)
	{
		_buildingCell = cell;
		_buildingProgress.SetMaxHealth(_stats.MaxHealth);
		_buildingProgress.SetHealth(0.01f);
		//_dustPatricles = Instantiate(_towerData.DustParticles, transform.position, Quaternion.identity, transform);
		//_dustPatricles.Stop();

		//var main = _stats.DustParticles.main;
		//main.duration = _towerData.BuildingTime;
		//_towerData.DustParticles.Play();
		_buildingProgress.Show();

		DOTween.To(() => _buildProgress, x => _buildProgress = x, _stats.MaxHealth, _stats.MaxHealth)
			.OnUpdate(() =>
			{
				_stats.Health.Value = _buildProgress;
				_buildingProgress.SetHealth(_buildProgress);
			}).OnComplete(() =>
				{
					//_dustPatricles.Clear();
					_buildingProgress.Hide();
					_shooter.gameObject.SetActive(true);
					_buildingCell.EnableUpgrades(_stats.UpgradablesList);

					if (_buildingCell.IsTouched())
                    {
						ShowRange();						
					}

                }).SetEase(Ease.Linear);
		_towerObject.DOLocalMoveY(_endOffsetY, _stats.MaxHealth);
	}

	public void SetStats(TowerStats stats)
	{
		_stats = stats;
		_shooter.SetTowerData(stats);
	}

	public void ClearAllUnusedBullets() => _shooter.ClearAmmo();
    
	public void RebuildTower()
    {
		_towerObject.position = new Vector3(_towerObject.position.x, _startOffsetY, _towerObject.position.z);
		_shooter.gameObject.SetActive(false);
		_buildProgress = 0.01f;
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

	private void RecalculateStats()
    {
        foreach (var effect in _effects)
        {
			var bonus = effect.GetPercentage();

			switch (effect.GetEffectType())
            {				
                case EffectType.IncreaceAttackPower:
					_stats.UpgradeStat(StatType.BonusAttackPower, bonus);
					break;
                case EffectType.IncreaceAttackSpeed:
					_stats.UpgradeStat(StatType.BonusAttackSpeed, bonus);
					break;
            }         
        }
	}

	public List<IEffect> GetTickableEffects() => _effects.FindAll(x => x.IsTickable());
	public Transform GetOrigin() => transform;
	public List<IEffect> GetEffects() => _effects;

	public void ApplyEffect(IEffect effect)
    {
        if (effect.GetEffectType() == EffectType.IncreaceAttackPower)
        {
			_waterBuff.EnableEffect();
        }

        if (effect.GetEffectType() == EffectType.IncreaceAttackSpeed)
        {
			_fireBuff.EnableEffect();
        }

		_effects.Add(effect);
		RecalculateStats();
	}

    public void RemoveEffect(IEffect effect)
    {
		if (effect.GetEffectType() == EffectType.IncreaceAttackPower)
		{
			_waterBuff.DisableEffect();
			_stats.UpgradeStat(StatType.BonusAttackPower, 0f);
		}

		if (effect.GetEffectType() == EffectType.IncreaceAttackSpeed)
		{
			_fireBuff.DisableEffect();
			_stats.UpgradeStat(StatType.BonusAttackSpeed, 0f);
		}

		_effects.Remove(effect);
	}

	public void TickAction()
    {
		Debug.Log("Tick");
    }

    public void RefreshEffectValues()
    {
		RecalculateStats();
	}
}
