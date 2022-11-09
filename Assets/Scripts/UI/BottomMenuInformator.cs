using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BottomMenuInformator : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private Text _towerName;
    [SerializeField] private Text _damage;
    [SerializeField] private Text _attackSpeed;
    [SerializeField] private Text _armor;
    [SerializeField] private Text _currentHitPoints;

    public void UpgradeInformation(Sprite previewImage, string towerName, int minDamage, int maxDamage, int armor, float attackSpeed, string currentHitPoints, Color hitPointsColor)
    {
        _previewImage.sprite = previewImage;
        _towerName.text = towerName;
        _damage.text = $"{minDamage} - {maxDamage}";
        _attackSpeed.text = $"{attackSpeed}/sec";
        _armor.text = $"{armor}";
        _currentHitPoints.text = $"{currentHitPoints}";
        _currentHitPoints.color = hitPointsColor;
    }
}
