using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
	public GameObject buildingCellObject;
	public GameObject tower;
	private Transform _towerPlace;
	private BuildCellChanger _buildCellChanger;
	private BuildingCell _buildingCell;
	private Camera _camera => Camera.main;

	public void PlaceTower()
	{
		_buildCellChanger = _camera.GetComponent<BuildCellChanger>();
		_towerPlace = _buildCellChanger.Selected.transform;

		if (tower != null)
		{
			_buildingCell = buildingCellObject.GetComponent<BuildingCell>();			
			_buildingCell._isEmpty = false;
			var newTower = Instantiate(tower, new Vector3(_towerPlace.position.x, _towerPlace.position.y - 1f, _towerPlace.position.z), Quaternion.identity);
			_buildingCell.placedTower = newTower.GetComponent<Tower>();
			_buildCellChanger.DidsbleSelectFrame();
			_buildingCell.placedTower.TowerBuild();
			_buildingCell.UpgradeInfo();
		}
	}
}
