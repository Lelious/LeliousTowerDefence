using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPointInfoField : MonoBehaviour
{
    [SerializeField] private TopMenuInformator _topMenu;
    [SerializeField] private GameObject _infoField;
    [SerializeField] private TextMeshProUGUI _pointName;
    [SerializeField] private Image _waveImage;
    [SerializeField] private TextMeshProUGUI _waveName;
    [SerializeField] private TextMeshProUGUI _waveCount;
    [SerializeField] private TextMeshProUGUI _waveHealth;
    [SerializeField] private TextMeshProUGUI _waveReward;

    private PointDescriptionData _data;

    public void ShowInfoField(PointDescriptionData data)
    {
        _data = data;
        _pointName.text = _data.PointName;
        _waveImage.sprite = _data.WaveImage;
        _waveName.text = $"Creature: {_data.WaveName}";
        _waveCount.text = $"Count: {_data.Count}";
        _waveHealth.text = $"Health: {_data.Health}";
        _waveReward.text = $"Reward: {_data.Reward}";

        _infoField.SetActive(true);
    }

    public void HideInfoField()
    {
        _infoField.SetActive(false);
    }

    public void StartGameMap()
    {
        _topMenu.StartGameState(_data);
    }
}
