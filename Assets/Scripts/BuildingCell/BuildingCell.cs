using UnityEngine;
using Zenject;

public class BuildingCell : MonoBehaviour, ITouchable
{
	private GamePannelUdaterInfoContainer _containerInfo;
	private BottomGameMenu _bottomMenuInformator;
	private GameUIService _gameInformationMenu;
	private SelectedFrame _selectedFrame;
	private TowerFactory _towerFactory;
	private NewTower _placedTower;
	private TowerData _towerData;
	private bool _isTouched;

	[Inject]
	private void Construct(GameUIService gameInformationMenu, TowerFactory towerFactory, SelectedFrame selectedFrame)
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
			_bottomMenuInformator.SetEntityToPannelUpdate(_containerInfo);
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
		_placedTower.TowerBuild(this);
		_towerData = _placedTower.GetTowerData();

		InitializeInfoContainer();
	}

	public void Untouch()
	{
		_isTouched = false;

		if (_placedTower)				
			_placedTower.HideRange();
	}

	public bool IsTouched() => _isTouched;
	private void InitializeInfoContainer()
	{
		_containerInfo = new GamePannelUdaterInfoContainer();

		_containerInfo.Touchable = this;
		_containerInfo.PreviewImage = _towerData.MainImage;
		_containerInfo.CurrentHealth = _placedTower.Health;
		_containerInfo.Name = _towerData.Name;
		_containerInfo.MaxHealth = _towerData.BuildingTime;
		_containerInfo.MinDamage = _towerData.MinimalDamage;
		_containerInfo.MaxDamage = _towerData.MaximumDamage;
		_containerInfo.Armor = 0;
		_containerInfo.AttackSpeed = _towerData.AttackSpeed;
	}
}
