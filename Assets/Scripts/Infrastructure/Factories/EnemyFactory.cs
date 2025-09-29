using BezierSolution;
using UnityEngine;
using Zenject;

public sealed class EnemyFactory : IInitializable, IEnemyFactory
{
    private EnemyLoadService _enemyLoadService;
    private BezierSpline _path;
    private EnemyPool _enemyPool;
    private Object _enemyPrefab;
    private int _counter = 2;

    [Inject]
    readonly DiContainer _container = null;

    [Inject]
    private void Construct(EnemyPool enemyPool, EnemyLoadService enemyLoadService)
    {
        _enemyPool = enemyPool;
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
            var enemy = _container.InstantiatePrefabForComponent<EnemyEntity>(_enemyPrefab, Vector3.zero, Quaternion.identity, parent);
            enemy.InitializeEnemy(_path);
            enemy.gameObject.SetActive(false);
            _enemyPool.AddEnemyToPool(enemy);
        }    
    }

    public void SetMapPath(BezierSpline spline) => _path = spline;

    public EnemyEntity GetEnemy() => _enemyPool.GetEnemyFromPool();
    public void IncreaceWaveCounter()
    {
        _counter = 2; //Debug Vertex Animation Section
        _enemyPrefab = InitializeNextEnemy();
    }

    public void Initialize() { }

    private Object InitializeNextEnemy()
    {
        return _enemyLoadService.LoadNextEnemyPrefab(_counter);
    }
}
