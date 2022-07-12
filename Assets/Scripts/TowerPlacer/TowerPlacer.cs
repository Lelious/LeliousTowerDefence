using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
	public GameObject buildingCellObject;
	public GameObject tower;
	private Transform _towerPlace;
	private BuildCellChanger _buildCellChanger;
	private BuildingCell _buildingCell;
	private GameManager _gameManager;
	private Camera _camera => Camera.main;

	private protected void Awake()
	{
		_gameManager = FindObjectOfType<GameManager>();
	}

	public void PlaceTower()
	{
		_buildCellChanger = _camera.GetComponent<BuildCellChanger>();
		_towerPlace = _buildCellChanger.Selected.transform;

		if (tower != null)
		{
			_buildingCell = buildingCellObject.GetComponent<BuildingCell>();			
			var newTower = Instantiate(tower, new Vector3(_towerPlace.position.x, _towerPlace.position.y - 1f, _towerPlace.position.z), Quaternion.identity);
			_buildingCell.placedTower = newTower.GetComponent<Tower>();

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

			_buildCellChanger.DidsbleSelectFrame();
		}
	}
}
