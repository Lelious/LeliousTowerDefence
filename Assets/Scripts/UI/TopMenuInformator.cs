using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;
using Infrastructure.StateMachine;

public class TopMenuInformator : MonoBehaviour
{
	[SerializeField] private Text _money;
	[SerializeField] private Text _waveCount;
	[SerializeField] private Text _timeBeforeSpawn;
	[SerializeField] private GameObject _waveInTexts, _spawningText;
	[SerializeField] private Button _spawnNowButton;
	[SerializeField] private GameObject _disabledSpawnButton;
	[SerializeField] private MiniMapPointInfoField _miniMapInfoField;

	private GameLoopStateMachine _gameLoopStateMachine;
	private EnemyPool _enemyPool;
	private GameManager _gameManager;
	private bool _isSpawning = false;

	[Inject]
	private void Construct(EnemyPool enemyPool, GameLoopStateMachine stateMachine, GameManager gameManager)
	{
		_gameLoopStateMachine = stateMachine;
        _enemyPool = enemyPool;
		_gameManager = gameManager;
	}

	private void Awake()
	{
		_enemyPool.EnemiesWaveCount
			.ObserveEveryValueChanged(x => x.Value)
			.Subscribe(j =>  SetEnemiesValue(j))
			.AddTo(this);
	}

	public MiniMapPointInfoField GetInfoField() => _miniMapInfoField;

	public void SetSpawnTime(int value)
	{
		if (value > 0)
			_timeBeforeSpawn.text = $"Wave in : {value}";
		else
			EnterSpawnState();
    }

	public void EnableDisableCounter()
	{
		_waveInTexts.SetActive(!_waveInTexts.activeInHierarchy);
		_spawningText.SetActive(!_spawningText.activeInHierarchy);
        _disabledSpawnButton.SetActive(!_disabledSpawnButton.activeInHierarchy);
        _spawnNowButton.interactable = !_spawnNowButton.interactable;
    }

	public void SetMoney(int amount)
	{
		_money.text = amount < 100 ? CachedStringValues.cachedStringValues[amount] : $"{amount}";
	}

	public void EnterSpawnState()
	{
        _gameLoopStateMachine.Enter<GameSpawnState>();
		_isSpawning = true;
        _spawnNowButton.interactable = false;
        _timeBeforeSpawn.text = $"Spawning Enemy!";
    }

	public async void StartGameState(PointDescriptionData data)
    {
		_miniMapInfoField.HideInfoField();
		await _gameManager.SwitchToGameMap(data);
	}

	private void SetEnemiesValue(int value)
	{
		_waveCount.text = $"{value}";
	}
}
