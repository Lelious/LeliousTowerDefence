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
            _playerGold -= cost;
            return true;           
        }
    }
}
