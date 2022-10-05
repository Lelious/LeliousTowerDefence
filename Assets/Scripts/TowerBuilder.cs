using UnityEngine;

public sealed class TowerBuilder : MonoBehaviour
{
    public void BuildTower(NewTower tower, Vector3 point, BuildingCell buildingCell)
    {
        var newTower = Instantiate(tower, point, Quaternion.identity);
        buildingCell.UpgradeInfo(newTower);
    }
}
