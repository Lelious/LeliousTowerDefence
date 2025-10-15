using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IEnemyFactory
{
    public async void CreateEnemy(SpawnScheme spawnScheme, Transform parent = null) { }
    public EnemyEntity GetEnemy();    
}
