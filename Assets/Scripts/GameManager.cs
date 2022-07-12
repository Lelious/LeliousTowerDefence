using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerGold;

    public bool CheckForGoldAvalability(int cost)
    {
        if (cost > _playerGold)
        {
            return false;
        }
        else
        {
            Debug.Log(_playerGold);
            _playerGold -= cost;
            return true;           
        }
    }
}
