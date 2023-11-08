using UnityEngine;
using Zenject;

public class EnemyLoadService
{
    private readonly string _prefabPath = AssetPath.EnemyPrefabPath;
    public Object LoadNextEnemyPrefab(int waveCount) => Resources.Load($"{_prefabPath}{waveCount}");
}
