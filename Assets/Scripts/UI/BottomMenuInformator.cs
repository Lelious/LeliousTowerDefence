using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BottomMenuInformator : MonoBehaviour
{
    [SerializeField] private BottomMenuIconsContainer _bottomMenuIconsContainer;
    [SerializeField] private Image _previewImage;
    [SerializeField] private Text _name;
    [SerializeField] private Text _damage;
    [SerializeField] private Text _attackSpeed;
    [SerializeField] private Text _armor;
    [SerializeField] private Text _hitPoints;
    [SerializeField] private Gradient _hpColorGradient;

    [Inject] private GameInformationMenu _gameInformationMenu;

    private System.IDisposable _disposableEntity;
    private CompositeDisposable _disposable = new CompositeDisposable();
    private List<UIMenuIcons> _unusedIconsList = new List<UIMenuIcons>();

    public void SetEntityToPannelUpdate(ITouchable touchable, Sprite previewImage, 
                                        FloatReactiveProperty currentHealth,
                                        string name,
                                        float maxHealth, float minDamage = 0, 
                                        float maxDamage = 0, 
                                        float armor = 0, 
                                        float attackSpeed = 0)
    {
        _disposableEntity?.Dispose();
        _unusedIconsList.Clear();
        _previewImage.sprite = previewImage;
        _name.text = name;
        _damage.text = $"{minDamage} - {maxDamage}";
        _attackSpeed.text = $"{attackSpeed}/sec";
        _armor.text = $"{armor}";

        if (minDamage == 0f || maxDamage == 0f)       
            _unusedIconsList.Add(UIMenuIcons.Damage);        
        if (armor == 0)       
            _unusedIconsList.Add(UIMenuIcons.Armor);
        if (attackSpeed == 0)       
            _unusedIconsList.Add(UIMenuIcons.AttackSpeed);

        _bottomMenuIconsContainer.RemoveUnusedIcons(_unusedIconsList);

        _disposableEntity = currentHealth
            .Subscribe(health =>
            {
                if (health <= 0 && touchable.IsTouched())
                {
                    _gameInformationMenu.HideGameMenu();
                }
                SetHpColor(health, maxHealth);
            }).AddTo(_disposable);
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
