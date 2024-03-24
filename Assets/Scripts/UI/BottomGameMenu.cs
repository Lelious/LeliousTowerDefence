using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class BottomGameMenu : MonoBehaviour
{
    [SerializeField] private BottomMenuIconsContainer _bottomMenuIconsContainer;
    [SerializeField] private Image _previewImage;
    [SerializeField] private TextMeshProUGUI _name,
                                             _damage,
                                             _bonusDamage,
                                             _attackSpeed,
                                             _armor,
                                             _hitPoints;
    [SerializeField] private Gradient _hpColorGradient;
    [SerializeField] private List<UIButton> _upgradesList = new List<UIButton>();
    [SerializeField] private UIEffectsPlacer _effectsPlacer;
    [SerializeField] private GameObject _upgradesWindow;
    [SerializeField] private ButtonSpriteSwapper _upgradesButtonAnimation;
    [SerializeField] private UIInfobox _infoBox;
    [Inject] private GameUIService _gameInformationMenu;

    private bool _upgradesWindowStatus;
    private List<IDisposable> _disposables = new List<IDisposable>();
    private List<UIMenuIcons> _unusedIconsList = new List<UIMenuIcons>();

    private void Awake()
    {
        _bottomMenuIconsContainer.Initialize();
    }

    public void SetEntityToPannelUpdate(GamePannelUdaterInfoContainer infoContainer, TouchableType type)
    {
        Dispose();

        _unusedIconsList.Clear();

        ValidateContainerValues(infoContainer);
    }

    public void UpdateUpgradesInfo(GamePannelUdaterInfoContainer infoContainer)
    {
        foreach (var item in _upgradesList)
        {
            item.gameObject.SetActive(false);
        }

        if (infoContainer.UpgradesList != null)
        {
            for (int i = 0; i < infoContainer.UpgradesList.Count; i++)
            {
                _upgradesList[i].gameObject.SetActive(true);
                _upgradesList[i].SetButton(infoContainer.UpgradesList[i]);
            }

            if (!_upgradesWindowStatus)
            {
                _upgradesButtonAnimation.StartAnimation();
            }
        }
    }

    public void ShowUpgradesWindow()
    {
        _upgradesWindowStatus = true;
        _upgradesWindow.SetActive(_upgradesWindowStatus);

        HideInfoBox();
    }

    public void HideUpgradesWindow(bool needPlayAnim = true)
    {
        _upgradesWindowStatus = false;
        _upgradesWindow.SetActive(_upgradesWindowStatus);
        _upgradesButtonAnimation.gameObject.SetActive(true);

        if (needPlayAnim)
        {
            _upgradesButtonAnimation.StartAnimation();
        }
    }

    public void ShowInfoBox(string name, string description)
    {
        _infoBox.gameObject.SetActive(true);
        _infoBox.SetDescription(name, description);
    }

    public void HideInfoBox() => _infoBox.gameObject.SetActive(false);

    private void Dispose()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
    }    

    private void ValidateContainerValues(GamePannelUdaterInfoContainer infoContainer)
    {
        _previewImage.sprite = infoContainer.PreviewImage;
        _name.text = infoContainer.Name;
        _damage.text = $"{infoContainer.MinDamage} - {infoContainer.MaxDamage}";
        _effectsPlacer.SetEffects(infoContainer.CurrentEffects);

        if (infoContainer.MinDamage < 0f)
            _unusedIconsList.Add(UIMenuIcons.Damage);
        if (infoContainer.UpgradableStats.TryGetValue(StatType.Armor, out var armorReactiveValue))
        {
            armorReactiveValue
                .Subscribe(armor =>
                {
                    SetArmor(armor);
                }).AddTo(_disposables);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.Armor);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.BonusAttackPower, out var bonusAttackReactiveValue))
        {
            bonusAttackReactiveValue
                .Subscribe(attackBonus =>
                {
                    SetAttackBonus((int)attackBonus);
                }).AddTo(_disposables);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.BonusAttackPower);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.BonusAttackSpeed, out var bonusAttackSpeedReactiveValue))
        {
            bonusAttackSpeedReactiveValue
                .Subscribe(attackSpeed =>
                {
                    SetAttackSpeed(infoContainer.AttackSpeed + attackSpeed, attackSpeed == 0 ? false : true);
                }).AddTo(_disposables);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.AttackSpeed);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.Health, out var healthReactiveValue))
        {
            healthReactiveValue
                .Subscribe(health =>
                {
                    SetHp(health, infoContainer.MaxHealth);
                }).AddTo(_disposables);
        }

        foreach (var item in _upgradesList)
        {
            item.gameObject.SetActive(false);
        }

        if (infoContainer.UpgradesList != null)
        {
            for (int i = 0; i < infoContainer.UpgradesList.Count; i++)
            {
                _upgradesList[i].gameObject.SetActive(true);
                _upgradesList[i].SetButton(infoContainer.UpgradesList[i]);
            }
            if (!_upgradesWindowStatus)
            {
                _upgradesButtonAnimation.StartAnimation();
            }
        }

        _bottomMenuIconsContainer.RemoveUnusedIcons(_unusedIconsList);
    }

    private void SetArmor(float value)
    {
        _armor.text = $"{(int)value}";
    }

    private void SetAttackBonus(int value)
    {
        _bonusDamage.text = value > 0 ? $"+ {value}" : "";
    }

    private void SetAttackSpeed(float value, bool upgraded = false)
    {
        string trim = string.Format("{0:f2}", 1f / (value / 100f));
        _attackSpeed.text = $"{trim}/sec";
        _attackSpeed.color = upgraded == true ? new Color(0, 0.7f, 0.04f, 1) : new Color(0.87f, 0.87f, 0.87f, 1);
    }

    private void SetHp(float currentHealth, float maxHealth)
    {
        float clampHealth = currentHealth / maxHealth;

        if (currentHealth < 1.0f)
            currentHealth = 1f;
        else
            currentHealth = Mathf.Floor(currentHealth);

        _hitPoints.text = $"{Mathf.Floor(currentHealth)}/{maxHealth}";
        _hitPoints.color = SetCurrentHealthColor(clampHealth);
    }

    private Color SetCurrentHealthColor(float value) => _hpColorGradient.Evaluate(value);    
}
