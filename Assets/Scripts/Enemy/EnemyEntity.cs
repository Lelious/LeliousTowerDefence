using System.Collections;
using UnityEngine;
using Zenject;

public class EnemyEntity : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private GameObject _selection;
    [SerializeField] private HealthBar _hpBar;

    private GameManager _gameManager;
    private EnemyPool _enemyPool;

    [Inject]
    private void Construct(EnemyPool enemyPool, GameManager gameManager)
    {
        _enemyPool = enemyPool;
        _gameManager = gameManager;
    }

    public void ReturnToEnemyPool()
    {
        _enemyPool.ReturnToPool(this);
        _gameManager.AddGold(_enemyData.Worth);

        StartCoroutine(DelayedDisableRoutine());
    }

    private IEnumerator DelayedDisableRoutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
