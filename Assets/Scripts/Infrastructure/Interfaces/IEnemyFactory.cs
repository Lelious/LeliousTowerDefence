using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFactory
{
    public Enemy CreateEnemy(Vector3 position, Transform parent = null);
    public void LoadNextEnemyPrefab();

}
