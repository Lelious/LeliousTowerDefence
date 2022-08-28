using UnityEngine;

public sealed class TowerBuilder : MonoBehaviour
{
    public void BuildTower(NewTower tower, Transform point, BuildingCell buildingCell)
    {
        buildingCell.placedTower = Instantiate(tower, point.position, Quaternion.identity);
        buildingCell.UpgradeInfo();
    }
}
