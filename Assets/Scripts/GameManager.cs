using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    private int _playerGold = 20;
    private TopMenuInformator _topMenuInformator;
    private EnemySpawnService _enemySpawnService;

    [Inject]
    private void Construct(TopMenuInformator topMenuInformator)
    {
        _topMenuInformator = topMenuInformator;
        _topMenuInformator.SetMoney(_playerGold);
    }

    public bool CheckForGoldAvalability(int cost)
    {
        if (cost > _playerGold)
        {
            NotEnoughGold();
            return false;
        }
        else
        {
            _playerGold -= cost;
            _topMenuInformator.SetMoney(_playerGold);
            return true;           
        }
    }

    public void NotEnoughGold()
    {

    }
}
