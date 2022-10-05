using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIButton : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Text _name;

	private GameInformationMenu _gameInformationMenu;
	private GameManager _gameManager;
	private TowerBuilder _towerBuilder;
	private InputService _inputService;
	private TowerData _towerData;	

	public void SetButton(TowerData data, TowerBuilder towerBuilder, InputService inputService, GameManager gameManager, GameInformationMenu gameInformationMenu)
	{
		_towerData = data;
		_towerBuilder = towerBuilder;
		_inputService = inputService;
		_gameManager = gameManager;
		_gameInformationMenu = gameInformationMenu;
		_image.sprite = _towerData.MainImage;
		_name.text = _towerData.Name;
	}

	public void TryPlaceTower()
	{
		if (_gameManager.CheckForGoldAvalability(_towerData.Cost))
			Place();
		else
			_gameManager.NotEnoughGold();
	}

	private void Place()
	{
		var buildingCell = _gameInformationMenu.GetLastTouchedBuildingCell();
		_towerBuilder.BuildTower(_towerData.TowerPrefab, buildingCell.GetPosition(), buildingCell);
	}
}
