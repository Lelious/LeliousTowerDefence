using UnityEngine;
using UnityEngine.UI;

public class TimeBeforeSpawn : MonoBehaviour
{
    [SerializeField] private Text _timeBeforeSpawnText;
    [SerializeField] private float _timeBeforeSpawn;
    [SerializeField] private Button _spawnNowButton;

    private float _remainingTime;
    private bool _onSpawning;

    private protected void Awake()
    {
        _remainingTime = _timeBeforeSpawn;
    }

    private protected void FixedUpdate()
    {
        if (_remainingTime > 0.6f)
        {
            _remainingTime -= Time.deltaTime;
            _timeBeforeSpawnText.text = $"Before Spawn : {string.Format("{0:f0}", _remainingTime)}";
        }

        else
        {
            if (!_onSpawning)
            {
                _onSpawning = !_onSpawning;
                SpawnNow();
                _timeBeforeSpawnText.text = $"Spawning Enemy!";
            }
        }
    }

    public void SpawnNow()
    {
        _remainingTime = 0f;
        _spawnNowButton.interactable = false;
    }

    public void ResetSpawnTime()
    {
        _onSpawning = false;
        _spawnNowButton.interactable = true;
        _remainingTime = _timeBeforeSpawn;
    }
}
