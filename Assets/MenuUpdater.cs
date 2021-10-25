using UnityEngine;
using UnityEngine.UI;

public class MenuUpdater : MonoBehaviour
{
    [SerializeField] private Image _previewImage;
    [SerializeField] private Text _towerName;
    [SerializeField] private int _minDamage, _maxDamage;
    [SerializeField] private Sprite asd;

    public void UpgradeInformation(Sprite previewImage, string towerName)
    {
        _previewImage.sprite = previewImage;
        _towerName.text = towerName;

    }
}
