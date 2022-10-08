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

	private GameLoopStateMachine _gameLoopStateMachine;
	private EnemyPool _enemyPool;

	[Inject]
	private void Construct(EnemyPool enemyPool, GameLoopStateMachine gameLoopStateMachine)
	{
		_gameLoopStateMachine = gameLoopStateMachine;
		_enemyPool = enemyPool;
	}

	private void Awake()
	{
		_enemyPool.EnemiesWaveCount
			.ObserveEveryValueChanged(x => x.Value)
			.Subscribe(j =>  SetEnemiesValue(j))
			.AddTo(this);
	}

	public void SetSpawnTime(int value)
	{
		if (value > 0)
			_timeBeforeSpawn.text = CachedStringValues.cachedStringValues[value];
		else
			EnterSpawnState();
	}

	public void EnableDisableCounter()
	{
		_waveInTexts.SetActive(!_waveInTexts.activeInHierarchy);
		_spawningText.SetActive(!_spawningText.activeInHierarchy);
		_spawnNowButton.interactable = !_spawnNowButton.interactable;
	}

	public void SetMoney(int amount)
	{
		_money.text = CachedStringValues.cachedStringValues[amount];
	}

	public void EnterSpawnState()
	{
		_gameLoopStateMachine.Enter<GameSpawnState>();
	}

	private void SetEnemiesValue(int value)
	{
		_waveCount.text = CachedStringValues.cachedStringValues[value];
	}
}
