using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class EnemyEntity : MonoBehaviour, ITouchable, IDamagable, IEffectable
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _selection;
    [SerializeField] private EnemyHitPoint _hitPoint;
    [SerializeField] private EnemyMovement _enemyMovement;

    private IEffectable _effectable;
    private EnemyStats _enemyStats;
    private GamePannelUdaterInfoContainer _containerInfo;
    private FloatingTextService _floatingTextService;
    private GameUIService _gameInformationMenu;
    private BottomGameMenu _bottomMenuInformator;
    private SelectedFrame _selectedFrame;
    private GameManager _gameManager;
    private EnemyPool _enemyPool;
    private bool _isTouched;
    [SerializeField] private List<IEffect> _effects = new List<IEffect>();

    GameObject ITouchable.gameObject { get => gameObject; }

    [Inject]
    private void Construct(EnemyPool enemyPool, GameManager gameManager, GameUIService gameInformationMenu, SelectedFrame selectedFrame, FloatingTextService floatingTextService)
    {
        _enemyPool = enemyPool;
        _gameManager = gameManager;
        _gameInformationMenu = gameInformationMenu;
        _bottomMenuInformator = _gameInformationMenu.GetBottomMenuInformator();
        _floatingTextService = floatingTextService;
        _selectedFrame = selectedFrame;
        _effectable = GetComponent<IEffectable>();
        _enemyHealth.SetFloatingTextService(_floatingTextService);
    }

    public FloatReactiveProperty GetReactiveHealthProperty() => _enemyStats.Health;
    public IEffectable GetEffectable() => _effectable;
    public IEffect GetEffect(EffectType type) => _effects.Find(x => x.GetEffectType() == type);
    public bool CanBeAttacked() => _enemyStats.Health.Value > 0 ? true : false;
    public EnemyHitPoint HitPoint() => _hitPoint;
    public Transform GetOrigin() => transform;
    public Vector3 GetPosition() => transform.position;
    public bool IsTouched() => _isTouched;

    public void InitializeEnemy()
    {
        _enemyStats = new EnemyStats();
        _enemyStats.InitializeStats(_enemyData);
        _enemyStats.InitializeInfoContainer();
        _containerInfo = _enemyStats.GetContainer();
        _enemyHealth.InitializeHealth(_enemyStats.MaxHealth, _enemyStats.Health);
        _enemyMovement.SetEnemyStats(_enemyStats);
        _enemyMovement.UpdateSpeed();
    }

    public void ReturnToEnemyPool()
    {
        if (_isTouched)
        {
            _gameInformationMenu.HideGameMenu();
            Untouch();
        }
        _enemyPool.ReturnToPool(this);
        _gameManager.AddGold(_enemyData.Worth);

        RemoveAllEffects();
        StartCoroutine(DelayedDisableRoutine());
    }

    public void Touch()
    {
        _isTouched = true;
        _gameInformationMenu.ShowGameMenu();
        _bottomMenuInformator.SetEntityToPannelUpdate(_containerInfo);
        _selectedFrame.DisableFrame();
        _selection.SetActive(_isTouched);
    }

    public void Untouch()
    {
        _isTouched = false;
        _selection.SetActive(_isTouched);
    }

    public void TakeDamage(int damage, DamageSource source = DamageSource.Normal)
    {
        _enemyHealth.ProcessDamage(_hitPoint.transform.position, damage, source);
    }

    private void RecalculateStats()
    {
        _enemyStats.BonusSpeed.Value = 1f;

        foreach (var effect in _effects)
        {            
            switch (effect.GetEffectType())
            {
                case EffectType.DecreaceSpeed:
                    _enemyStats.UpgradeStat(StatType.BonusSpeed, -effect.GetPercentage());
                    break;
            }
        }
        _enemyMovement.UpdateSpeed();
    }

    private IEnumerator DelayedDisableRoutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        gameObject.layer = 2;
    }

    public void ApplyEffect(IEffect effect)
    {
        _effects.Add(effect);
        RecalculateStats();
    }

    public void RemoveEffect(IEffect effect)
    {
        _effects.Remove(effect);
        RecalculateStats();
    }

    public void RefreshEffectValues()
    {
        RecalculateStats();
    }

    public List<IEffect> GetEffects()
    {
        return _effects;
    }

    public void TickAction()
    {
        foreach (var effect in _effects)
        {
            if (effect.IsTickable())
            {
                _enemyHealth.ProcessDamage(_hitPoint.transform.position, effect.GetDamage(), effect.GetDamageSource());
            }
        }
    }

    public void RemoveAllEffects() => _effects.Clear();
}
