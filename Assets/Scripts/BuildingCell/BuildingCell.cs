using UnityEngine;
using Zenject;

public class BuildingCell : MonoBehaviour, ITouchable
{
	private BottomMenuInformator _bottomMenuInformator;
	private GameInformationMenu _gameInformationMenu;
	private SelectedFrame _selectedFrame;
	private TowerFactory _towerFactory;
	private NewTower _placedTower;
	private TowerData _towerData;
	private bool _isTouched;

	[Inject]
	private void Construct(GameInformationMenu gameInformationMenu, TowerFactory towerFactory, SelectedFrame selectedFrame)
	{
		_gameInformationMenu = gameInformationMenu;
		_towerFactory = towerFactory;
		_selectedFrame = selectedFrame;
		_bottomMenuInformator = _gameInformationMenu.GetBottomMenuInformator();
	}

	public void Touch()
	{
		_isTouched = true;
		_selectedFrame.EnableFrame();
		_selectedFrame.transform.position = transform.position;

		if (_placedTower)
		{
			_placedTower.ShowRange();
			_gameInformationMenu.ShowGameMenu();
			_bottomMenuInformator.SetEntityToPannelUpdate(this, _towerData.MainImage, _placedTower.Health, _towerData.Name, _towerData.BuildingTime, _towerData.MinimalDamage, _towerData.MaximumDamage, 0, _towerData.AttackSpeed);
		}
		else
		{
			_gameInformationMenu.ShowEmptyCellMenu();
			_gameInformationMenu.SetBuildingCell(this);
		}
	}

	public void BuildTowerOnPlace(TowerData data)
	{
		_placedTower = _towerFactory.CreateNewTower(data, transform.position);
		_towerData = _placedTower.GetTowerData();
	}

	public void Untouch()
	{
		_isTouched = false;

		if (_placedTower)				
			_placedTower.HideRange();
	}

	public bool IsTouched() => _isTouched;
}
