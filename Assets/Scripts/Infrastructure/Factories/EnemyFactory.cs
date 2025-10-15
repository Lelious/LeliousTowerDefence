using BezierSolution;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public sealed class EnemyFactory : IEnemyFactory
{
    private AssetReferenceGameObject _waveAsset;
    private EnemyPool _enemyPool;

    [Inject]
    readonly DiContainer _container = null;

    [Inject]
    private void Construct(EnemyPool enemyPool)
    {
        _enemyPool = enemyPool;
    }

    public DiContainer Container
    {
        get { return _container; }
    }

    public async UniTaskVoid CreateEnemy(SpawnScheme spawnScheme, Transform parent = null)
    {
        _waveAsset = spawnScheme.EnemyBasePrefab;

        for (int i = 0; i < spawnScheme.Count; i++)
        {
            var handle = Addressables.InstantiateAsync(_waveAsset, parent);
            await handle.ToUniTask();
            var enemy = handle.Result.GetComponent<EnemyEntity>();
            _container.Inject(enemy);
            enemy.InitializeEnemy(spawnScheme.EnemyDataStats);
            enemy.gameObject.SetActive(false);
            _enemyPool.AddEnemyToPool(enemy);
        }    
    }

    public void ReleaseEnemiesWave()
    {
        _enemyPool.ClearEnemyPool();
    }

    public EnemyEntity GetEnemy() => _enemyPool.GetEnemyFromPool();
}
