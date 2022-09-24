using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerGold;
    [SerializeField] private Text _moneyCountText;
    [SerializeField] private Text _waveCountText;
    [SerializeField] private EnemySpawnService _enemySpawnService;

    private void Update()
    {
        //_moneyCountText.text = CachedStringValues.cachedStringValues[_playerGold];
        //_waveCountText.text = CachedStringValues.cachedStringValues[_enemySpawnService.GetWaveCount()];
    }

    public bool CheckForGoldAvalability(int cost)
    {
        if (cost > _playerGold)
        {
            return false;
        }
        else
        {
            _playerGold -= cost;
            return true;           
        }
    }
}
