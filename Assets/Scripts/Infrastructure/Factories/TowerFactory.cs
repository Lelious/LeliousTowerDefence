using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

public class TowerFactory : IInitializable
{
	private List<Tower> _towers = new List<Tower>();
	private List<IEffectable> _effectablesList = new List<IEffectable>();

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
		_effectablesList.Add(newTower.GetComponent<IEffectable>());
		return newTower;
	}

	public void ClearTowerData(Tower tower)
    {
		_towers.Remove(tower);
		_effectablesList.Remove(tower.GetComponent<IEffectable>());
		Object.Destroy(tower.gameObject);
	}

	public List<IEffectable> GetEffectableTower(Vector3 position, float distance)
    {
		return _effectablesList.FindAll(x => Vector3.SqrMagnitude(position - x.GetOrigin().position) < distance * distance);
    }
}
