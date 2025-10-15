using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/SpawnPattern", order = 1)]
public class SpawnScheme : ScriptableObject
{
    public AssetReferenceGameObject EnemyBasePrefab;
    public EnemyData EnemyDataStats;
    public int Count;
    public List<SpawnSchemePattern> SpawnPattern = new(); 
}

[System.Serializable]
public class SpawnSchemePattern
{
    public SpawnPattern SpawnPhase;
    public int SpawnCount;
    public float Delay;
}
