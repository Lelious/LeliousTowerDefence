using UnityEngine;
using Zenject;

public sealed class EnemyFactory : IInitializable, IEnemyFactory
{
    private StartPoint _startPoint;
    private EnemyPool _enemyPool;
    private Object _enemyPrefab;
    private string _prefabPath;
    private int _counter = 1;

    [Inject]
    readonly DiContainer _container = null;

    [Inject]
    private void Construct(EnemyPool enemyPool, StartPoint startPoint)
    {
        _enemyPool = enemyPool;
        _startPoint = startPoint;
    }
    
    public DiContainer Container
    {
        get { return _container; }
    }

    public void CreateEnemy(int count = 1, Transform parent = null)
    {
        for (int i = 0; i < count; i++)
        {
            var enemy = _container.InstantiatePrefabForComponent<Enemy>(_enemyPrefab, _startPoint.transform.position, Quaternion.identity, parent);
            enemy.gameObject.SetActive(false);
            _enemyPool.AddEnemyToPool(enemy);
        }    
    }

    public void LoadNextEnemyPrefab()
    {
        _prefabPath = $"Waves/Wave{_counter}";
        _enemyPrefab = Resources.Load(_prefabPath);

        if (_prefabPath == null)
        {
            _prefabPath = $"Waves/Wave{_counter - 1}";
            _enemyPrefab = Resources.Load(_prefabPath);
        }
    }

    public Enemy GetEnemy() => _enemyPool.GetEnemyFromPool();
    public void Initialize() => LoadNextEnemyPrefab();
    public void IncreaceCounter() => _counter++;
}
