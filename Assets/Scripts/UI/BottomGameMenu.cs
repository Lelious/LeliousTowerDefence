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
    [SerializeField] private Text _attackSpeed;
    [SerializeField] private Text _armor;
    [SerializeField] private Text _hitPoints;
    [SerializeField] private Gradient _hpColorGradient;
    [SerializeField] private List<UIButton> _upgradesList = new List<UIButton>();
    [SerializeField] private List<GameObject> _visualizedButtons = new List<GameObject>();
    [Inject] private GameUIService _gameInformationMenu;

    private System.IDisposable _disposableEntity;
    private CompositeDisposable _disposable = new CompositeDisposable();
    private List<UIMenuIcons> _unusedIconsList = new List<UIMenuIcons>();

    public void SetEntityToPannelUpdate(GamePannelUdaterInfoContainer infoContainer)
    {
        _disposableEntity?.Dispose();
        _unusedIconsList.Clear();
        _previewImage.sprite = infoContainer.PreviewImage;

        _name.text = infoContainer.Name;
        _damage.text = $"{infoContainer.MinDamage} - {infoContainer.MaxDamage}";
        string trim = string.Format("{0:f2}", 1 / infoContainer.AttackSpeed);
        _attackSpeed.text = $"{trim}/sec";
        _armor.text = $"{infoContainer.Armor}";

        if (infoContainer.MinDamage == 0f || infoContainer.MaxDamage == 0f)       
            _unusedIconsList.Add(UIMenuIcons.Damage);        
        if (infoContainer.Armor == null)       
            _unusedIconsList.Add(UIMenuIcons.Armor);
        if (infoContainer.AttackSpeed == 0)       
            _unusedIconsList.Add(UIMenuIcons.AttackSpeed);

        _bottomMenuIconsContainer.RemoveUnusedIcons(_unusedIconsList);

        _disposableEntity = infoContainer.CurrentHealth
            .Subscribe(health =>
            {
                if (health <= 0 && infoContainer.Touchable.IsTouched())
                {
                    _gameInformationMenu.HideGameMenu();
                }
                SetHpColor(health, infoContainer.MaxHealth);
            }).AddTo(_disposable);

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

    private void SetHpColor(float currentHealth, float maxHealth)
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
