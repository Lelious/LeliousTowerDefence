using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TimeBeforeSpawn : MonoBehaviour
{
    [SerializeField] private Text _timeBeforeSpawnText;
    [SerializeField] private float _timeBeforeSpawn;
    [SerializeField] private Button _spawnNowButton;

    private GameLoopStateMachine _stateMachine;
    private float _remainingTime;
    private bool _onSpawning;
    private int _spawnTimer;

    [Inject]
    private void Construct(GameLoopStateMachine gameLoopStateMachine)
    {
        _stateMachine = gameLoopStateMachine;
    }

    private protected void Awake()
    {
        _remainingTime = _timeBeforeSpawn;
    }

    private protected void FixedUpdate()
    {

        if (_remainingTime > 1f)
        {
            _spawnTimer = (int)(_remainingTime -= Time.unscaledDeltaTime);
            _timeBeforeSpawnText.text = $"Before Spawn : {CachedStringValues.cachedStringValues[_spawnTimer]}";
        }

        else
        {
            if (!_onSpawning)
            {
                SpawnNow();
            }
        }
    }

    public void SpawnNow()
    {
        _stateMachine.Enter<GameSpawnState>();
        _onSpawning = true;
        _remainingTime = 0f;
        _spawnNowButton.interactable = false;
        _timeBeforeSpawnText.text = $"Spawning Enemy!";
    }

    public void ResetSpawnTime()
    {
        _onSpawning = false;
        _spawnNowButton.interactable = true;
        _remainingTime = _timeBeforeSpawn;
    }
}
