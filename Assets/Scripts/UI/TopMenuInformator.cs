using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using Infrastructure.StateMachine;
using TMPro;

public class TopMenuInformator : MonoBehaviour
{
	[Header("GameInformator")]
	[SerializeField] private GameObject _gameMenu;
	[SerializeField] private TextMeshProUGUI _money;
	[SerializeField] private TextMeshProUGUI _waveCount;
	[SerializeField] private TextMeshProUGUI _timeBeforeSpawn;
	[SerializeField] private TextMeshProUGUI _waveCreatureName;
	[SerializeField] private TextMeshProUGUI _lives;
	[SerializeField] private TextMeshProUGUI _spawnButtonText;
	[SerializeField] private Color _spawnNowColor;
	[SerializeField] private Color _spawningColor;
	[SerializeField] private Button _spawnNowButton;

	[Header("MiniMapInformator")]
	[SerializeField] private GameObject _miniMapMenu;
	[SerializeField] private MiniMapPointInfoField _miniMapInfoField;
	[SerializeField] private TextMeshProUGUI _progressText;
	[SerializeField] private TextMeshProUGUI _resourceText;

	private GameManager _gameManager;

	[Inject]
	private void Construct(GameManager gameManager)
	{
		_gameManager = gameManager;
	}

	public MiniMapPointInfoField GetInfoField() => _miniMapInfoField;

	public void SwitchToGameInformator(PointDescriptionData data)
    {
		_miniMapMenu.SetActive(false);
		_gameMenu.SetActive(true);

		_money.text = $"<sprite=2>{data.StartGold}";
		_waveCount.text = $"{data.Count}";
		_waveCreatureName.text = $"{data.WaveName}";
	}

	public void SwitchToMiniMapInformator()
    {
		_miniMapMenu.SetActive(true);
		_gameMenu.SetActive(false);
		_progressText.text = $"{PlayerPrefs.GetInt("PlayerProgress", 0)}/30";
		_resourceText.text = $"<sprite=0>200 <sprite=1>0";
	}

	public void SetSpawnTime(int value)
	{
		if (value > 0)
			_timeBeforeSpawn.text = $"Wave in : {value}";
		else
			EnterSpawnState();
    }

	public void EnableCounter()
	{
        _spawnNowButton.interactable = true;
		_spawnButtonText.text = "Spawn Now!";
		_spawnButtonText.color = _spawnNowColor;
    }

	public void DisableCounter()
    {
		_spawnNowButton.interactable = false;
		_spawnButtonText.text = "Spawning!";
		_spawnButtonText.color = _spawningColor;
	}

	public void SetMoney(int amount)
	{
		_money.text = amount < 100 ? CachedStringValues.cachedStringValues[amount] : $"{amount}";
	}

	public void EnterSpawnState()
	{
        _spawnNowButton.interactable = false;
        _timeBeforeSpawn.text = $"Spawning Enemy!";
		_gameManager.ForceSpawn();
	}

	public async void StartGameState(PointDescriptionData data)
    {
		_miniMapInfoField.HideInfoField();
		await _gameManager.SwitchToGameMap(data);
	}

	public void SetEnemiesValue(int value)
	{
		_waveCount.text = $"{value}";
	}
}
