using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIButton : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Text _name;

	private GameInformationMenu _gameInformationMenu;
	private GameManager _gameManager;
	private TowerData _towerData;	

	public void SetButton(TowerData data, GameManager gameManager, GameInformationMenu gameInformationMenu)
	{
		_towerData = data;
		_gameManager = gameManager;
		_gameInformationMenu = gameInformationMenu;

		_name.text = _towerData.Name;
		_image.sprite = _towerData.MainImage;
	}

	public void TryPlaceTower()
	{
		if (_gameManager.CheckForGoldAvalability(_towerData.Cost))
		{
			var buildingCell = _gameInformationMenu.GetLastTouchedBuildingCell();

			if (buildingCell)
				buildingCell.BuildTowerOnPlace(_towerData);
		}
		else
			_gameManager.NotEnoughGold();
	}
}
