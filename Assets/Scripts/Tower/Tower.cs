using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Zenject;

public class Tower : MonoBehaviour, IEffectable
{
	[SerializeField] private Shooter _shooter;
	[SerializeField] private GameObject _rangeBoarder;
	[SerializeField] private HealthBar _buildingProgress;
	[SerializeField] private Transform _towerObject;
	[SerializeField] private float _endOffsetY = -0.6f;
	[SerializeField] private List<GameObject> _upgradablesList = new();

	[Inject] private PoolService _poolService;
	private TowerStats _stats;
	private ReactiveCollection<IEffect> _effects = new();
	[SerializeField] private List<VisualBuff> _visualBuffsList = new();
	private ParticleSystem _dustPatricles;
	private BuildingCell _buildingCell;
	private float _buildProgress = 0.01f;
	private float _startOffsetY;
	private int _currentUpgrade = 0;
	private bool _onBuffProcessState;

    private void Awake() => _startOffsetY = _towerObject.position.y;
	public void SetupPoolService(PoolService poolService) => _poolService = poolService; 
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

					if (_buildingCell.IsActive())
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

	public void ReleaseTower()
    {
		_shooter.ClearAmmo();
		Destroy(gameObject);
	}

	public void ClearAllUnusedBullets() => _shooter.ClearAmmo();

    public void SetUpgradeLevel(int level)
    {
        for (int i = 0; i < level; i++)
        {
			_upgradablesList[i].SetActive(true);
        }
    }

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

	public Transform GetOrigin() => transform;
	public ReactiveCollection<IEffect> GetEffects() => _effects;
	public IEffect GetEffect(EffectType type)
	{
		foreach (var eff in _effects)
		{
			if (eff.GetEffectType() == type)
			{
				return eff;
			}
		}
		return null;
	}

	public void ApplyEffect(IEffect effect)
    {
		_effects.Add(effect);
		RecalculateStats();

        if (effect.GetVisualBuff() != null)
        {
			ApplyVisual(effect);
		}
	}

	public void ApplyVisual(IEffect effect)
    {
		var visual = effect.GetVisualBuff();		
		visual.SetupVisualBuff(effect.GetDuration(), true);
		visual.transform.position = transform.position;
		_visualBuffsList.Add(visual);
	}

	

    public void RemoveEffect(IEffect effect)
    {
		if (effect.GetEffectType().Equals(EffectType.IncreaceAttackPower))
		{
			_stats.UpgradeStat(StatType.BonusAttackPower, 0f);
		}

		if (effect.GetEffectType().Equals(EffectType.IncreaceAttackSpeed))
		{
			_stats.UpgradeStat(StatType.BonusAttackSpeed, 0f);
		}

		VisualBuff buff;

		for (int i = 0; i < _visualBuffsList.Count; i++)
		{
			buff = effect.GetVisualBuff();

			if (_visualBuffsList[i].GetPoolableType().Equals(buff.GetPoolableType()))
			{
				_visualBuffsList[i].ReturnToPool();
				_visualBuffsList.Remove(_visualBuffsList[i]);
			}
		}

		_effects.Remove(effect);
	}

	public void RefreshEffectValues(IEffect effect)
    {
		RecalculateStats();

        for (int i = 0; i < _visualBuffsList.Count; i++)
        {
            if (_visualBuffsList[i].GetPoolableType().Equals(effect.GetPoollableType()))
            {
				_visualBuffsList[i].SetupVisualBuff(effect.GetDuration(), false);
				effect.GetVisualBuff().ReturnToPool();
			}
        }
	}

	public void RemoveAllEffects() => _effects.Clear();

	public void SetOnBuffProcessState(bool state) => _onBuffProcessState = state;

	public bool GetProcessStatus() => _onBuffProcessState;
}
