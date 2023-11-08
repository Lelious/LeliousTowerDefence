using UnityEngine;
using Zenject;

public sealed class EnemyFactory : IInitializable, IEnemyFactory
{
    private EnemyLoadService _enemyLoadService;
    private StartPoint _startPoint;
    private EnemyPool _enemyPool;
    private Object _enemyPrefab;
    private EndPoint _endPoint;

    private int _counter = 1;

    [Inject]
    readonly DiContainer _container = null;

    [Inject]
    private void Construct(EnemyPool enemyPool, StartPoint startPoint, EndPoint endPoint, EnemyLoadService enemyLoadService)
    {
        _enemyPool = enemyPool;
        _startPoint = startPoint;
        _endPoint = endPoint;
        _enemyLoadService = enemyLoadService;
    }

    public DiContainer Container
    {
        get { return _container; }
    }

    public void CreateEnemy(int count = 1, Transform parent = null)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = _container.InstantiatePrefabForComponent<EnemyEntity>(_enemyPrefab, _startPoint.transform.position, Quaternion.identity, parent);
            enemy.InitializeEnemy();
            enemy.gameObject.SetActive(false);
            _enemyPool.AddEnemyToPool(enemy);
        }    
    }

    public EnemyEntity GetEnemy() => _enemyPool.GetEnemyFromPool();
    public void IncreaceCounter() => _counter++;
    public void Initialize() => _enemyPrefab = _enemyLoadService.LoadNextEnemyPrefab(_counter);
}
