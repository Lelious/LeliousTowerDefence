using UnityEngine;
using Zenject;

public sealed class EnemyFactory : IInitializable, IEnemyFactory
{
    private Object _enemyPrefab;
    private string _prefabPath;
    private int _counter = 1;

    [Inject]
    readonly DiContainer _container = null;

    public DiContainer Container
    {
        get { return _container; }
    }

    public Enemy CreateEnemy(Vector3 position, Transform parent = null)
    {
        return _container.InstantiatePrefabForComponent<Enemy>(_enemyPrefab, position, Quaternion.identity, parent);        
    }

    public void Initialize()
    {
        LoadNextEnemyPrefab();
    }

    public void LoadNextEnemyPrefab()
    {
        _prefabPath = $"Waves/Wave{_counter}";
        _enemyPrefab = Resources.Load(_prefabPath);
    }

    public void IncreaceCounter()
    {
        _counter++;
    }
}
