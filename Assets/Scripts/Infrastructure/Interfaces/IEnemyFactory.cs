using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFactory
{
    public void CreateEnemy(int count = 1, Transform parent = null);
    public EnemyEntity GetEnemy();    
}
