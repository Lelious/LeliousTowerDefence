using UnityEngine;
using Zenject;

public class TowerPlacer : MonoBehaviour
{
	public GameObject buildingCellObject;
	public NewTower tower;
	private Transform _towerPlace;
	private BuildCellInitializer _buildCellInitializer;
	private BuildingCell _buildingCell;
	private GameManager _gameManager;

	[Inject]
	private void Construct(BuildCellInitializer buildCellInitializer, GameManager gameManager)
	{
		_buildCellInitializer = buildCellInitializer;
		_gameManager = gameManager;
	}

	public void PlaceTower()
	{
		_towerPlace = _buildCellInitializer.Selected.transform;

		if (tower != null)
		{
			_buildingCell = buildingCellObject.GetComponent<BuildingCell>();			
			var newTower = Instantiate(tower, new Vector3(_towerPlace.position.x, _towerPlace.position.y - 1f, _towerPlace.position.z), Quaternion.identity);
			_buildingCell.placedTower = newTower.GetComponent<NewTower>();

			if (_gameManager.CheckForGoldAvalability(_buildingCell.placedTower.GetCost()))
			{
				_buildingCell._isEmpty = false;				
				_buildingCell.placedTower.TowerBuild();
				_buildingCell.UpgradeInfo();
			}

			else
			{
				Destroy(newTower);
			}

			_buildCellInitializer.DidsbleSelectFrame();
		}
	}
}
