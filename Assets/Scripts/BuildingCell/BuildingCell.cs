using UnityEngine;
using UnityEngine.UI;

public class BuildingCell : MonoBehaviour
{
	public Tower placedTower;
	public bool _isEmpty = true;

	[SerializeField] private MenuUpdater _menuUpdater;
	[SerializeField] private Sprite _image;
	[SerializeField] private string _name;

	public void UpgradeInfo()
	{
		_image = placedTower.mainImage;
		_name = placedTower.towerName;
		_menuUpdater.UpgradeInformation(_image, _name);
	}

}
