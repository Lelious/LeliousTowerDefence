using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BottomGameMenu : MonoBehaviour
{
    [SerializeField] private BottomMenuIconsContainer _bottomMenuIconsContainer;
    [SerializeField] private Image _previewImage;
    [SerializeField] private Text _name;
    [SerializeField] private Text _damage;
    [SerializeField] private Text _bonusDamage;
    [SerializeField] private Text _attackSpeed;
    [SerializeField] private Text _armor;
    [SerializeField] private Text _hitPoints;
    [SerializeField] private Gradient _hpColorGradient;
    [SerializeField] private List<UIButton> _upgradesList = new List<UIButton>();
    [SerializeField] private List<GameObject> _visualizedButtons = new List<GameObject>();
    //[Inject] private GameUIService _gameInformationMenu;

    private System.IDisposable _disposableEntity;
    private CompositeDisposable _disposable = new CompositeDisposable();
    private List<UIMenuIcons> _unusedIconsList = new List<UIMenuIcons>();

    public void SetEntityToPannelUpdate(GamePannelUdaterInfoContainer infoContainer)
    {
        _disposableEntity?.Dispose();
        _unusedIconsList.Clear();

        ValidateContainerValues(infoContainer);
    }

    public void UpdateUpgradesInfo(GamePannelUdaterInfoContainer infoContainer)
    {
        foreach (var item in _visualizedButtons)
        {
            item.SetActive(false);
        }

        if (infoContainer.UpgradesList != null)
        {
            for (int i = 0; i < infoContainer.UpgradesList.Count; i++)
            {
                _visualizedButtons[i].SetActive(true);
                _upgradesList[i].SetButton(infoContainer.UpgradesList[i]);
            }
        }
    }

    private void ValidateContainerValues(GamePannelUdaterInfoContainer infoContainer)
    {
        _previewImage.sprite = infoContainer.PreviewImage;
        _name.text = infoContainer.Name;
        _damage.text = $"{infoContainer.MinDamage} - {infoContainer.MaxDamage}";

        if (infoContainer.MinDamage == -1f)
            _unusedIconsList.Add(UIMenuIcons.Damage);
        if (infoContainer.UpgradableStats.TryGetValue(StatType.Armor, out var armorReactiveValue))
        {
            _disposableEntity = armorReactiveValue
            .Subscribe(armor =>
            {
                SetArmor(armor);
            }).AddTo(_disposable);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.Armor);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.BonusAttackPower, out var bonusAttackReactiveValue))
        {
            _disposableEntity = bonusAttackReactiveValue
                .Subscribe(attackBonus =>
                {
                    SetAttackBonus((int)attackBonus);
                }).AddTo(_disposable);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.BonusAttackPower);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.BonusAttackSpeed, out var bonusAttackSpeedReactiveValue))
        {
            _disposableEntity = bonusAttackSpeedReactiveValue
                .Subscribe(attackSpeed =>
                {
                    SetAttackSpeed(attackSpeed);
                }).AddTo(_disposable);
        }
        else
        {
            _unusedIconsList.Add(UIMenuIcons.AttackSpeed);
        }

        if (infoContainer.UpgradableStats.TryGetValue(StatType.Health, out var healthReactiveValue))
        {
            _disposableEntity = healthReactiveValue
                .Subscribe(health =>
                {
                    SetHp(health, infoContainer.MaxHealth);
                }).AddTo(_disposable);
        }

        foreach (var item in _visualizedButtons)
        {
            item.SetActive(false);
        }

        if (infoContainer.UpgradesList != null)
        {
            for (int i = 0; i < infoContainer.UpgradesList.Count; i++)
            {
                _visualizedButtons[i].SetActive(true);
                _upgradesList[i].SetButton(infoContainer.UpgradesList[i]);
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

    private void SetAttackSpeed(float value)
    {
        Debug.Log($"AttackSpeed = {value}");
        string trim = string.Format("{0:f2}", 1 / (value / 100));
        _attackSpeed.text = $"{trim}/sec";
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
