using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TowerFactory : IInitializable
{
	private List<NewTower> _towers = new List<NewTower>();

	[Inject]
	readonly DiContainer _container = null;

	public DiContainer Container
	{
		get { return _container; }
	}

	public void Initialize()
	{
		
	}

	public NewTower CreateNewTower(TowerData data, Vector3 position, Transform parent = null)
	{
		var newTower = 
			_container
			.InstantiatePrefabForComponent<NewTower>(data.TowerPrefab, position, Quaternion.identity, parent);

		_towers.Add(newTower);

		return newTower;
	}
}
