using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerFactory : IInitializable
{
	private List<Tower> _towers = new List<Tower>();

	[Inject]
	readonly DiContainer _container = null;

	public DiContainer Container
	{
		get { return _container; }
	}

	public void Initialize()
	{
		
	}

	public Tower CreateNewTower(TowerData data, Vector3 position, Transform parent = null)
	{
		var newTower = 
			_container
			.InstantiatePrefabForComponent<Tower>(data.TowerPrefab, position, Quaternion.identity, parent);

		_towers.Add(newTower);

		return newTower;
	}
}
