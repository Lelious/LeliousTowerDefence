using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform _startPoint, _endPoint;

    private TopMenuInformator _topMenuInformator;
    private EnemySpawnService _enemySpawnService;
    private int _playerGold = 50;

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

    public void AddGold(int amount)
    {
        _playerGold += amount;
        _topMenuInformator.SetMoney(_playerGold);
    }

    private void NotEnoughGold()
    {

    }
}
