using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIButton : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Text _name;

	private GameInformationMenu _gameInformationMenu;
	private TapRegistrator _tapRegistrator;
	private GameManager _gameManager;
	private TowerData _towerData;	

	public void SetButton(TowerData data, GameManager gameManager, GameInformationMenu gameInformationMenu, TapRegistrator tapRegistrator)
	{
		_towerData = data;
		_gameManager = gameManager;
		_gameInformationMenu = gameInformationMenu;
		_tapRegistrator = tapRegistrator;
		_name.text = _towerData.Name;
		_image.sprite = _towerData.MainImage;
	}

	public void TryPlaceTower()
	{
		_tapRegistrator.RegisterUITap();

		if (_gameManager.CheckForGoldAvalability(_towerData.Cost))
		{
			var buildingCell = _gameInformationMenu.GetLastTouchedBuildingCell();

			if (buildingCell)
				buildingCell.BuildTowerOnPlace(_towerData);
			_gameInformationMenu.HideEmptyCellMenu();
			_gameInformationMenu.HideGameMenu();
			_tapRegistrator.DisableSelectedFrame();
		}
	}
}
