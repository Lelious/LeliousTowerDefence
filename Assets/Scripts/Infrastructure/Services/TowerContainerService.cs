using System.Collections.Generic;
using UnityEngine;

public class TowerContainerService : ScriptableObject
{
	[SerializeField] private List<TowerData> _emptyBuildSlotData = new List<TowerData>();

	//public List<TowerData> GetTowersForInitialize(TowerData data)
	//{
	//	return _towerDatas
	//}
}
