using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class BuildingCell : MonoBehaviour, ITouchable	
{
	private GamePannelUdaterInfoContainer _containerInfo;
	private BottomGameMenu _bottomMenuInformator;
	private GameUIService _gameInformationMenu;
	private SelectedFrame _selectedFrame;
	private TowerFactory _towerFactory;
	private Tower _placedTower;
	private TowerData _towerData;
	private TowerStats _towerStats;
	private bool _isTouched;
	private TowerType _type;

    GameObject ITouchable.gameObject { get => gameObject;}

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
		_gameInformationMenu.SetBuildingCell(this);

		if (_placedTower)
		{
			_placedTower.ShowRange();
			_gameInformationMenu.ShowGameMenu();
			_bottomMenuInformator.SetEntityToPannelUpdate(_containerInfo);
		}
		else
		{
			_gameInformationMenu.ShowEmptyCellMenu();			
		}
	}

	public void BuildTowerOnPlace(TowerData data)
	{
		_towerStats = new TowerStats();
		_towerStats.InitializeStats(data);

		if (_placedTower != null)
        {
            if (_type != data.Type)
            {
				_placedTower.ClearAllUnusedBullets();
				_towerFactory.ClearTowerData(_placedTower);
				_placedTower = _towerFactory.CreateNewTower(data, transform.position);
				_placedTower.SetStats(_towerStats);
				_placedTower.TowerBuild(this);
				_placedTower.SetUpgradeLevel(data.UpgradeNumber);
				_type = data.Type;
			}
            else
            {
				_placedTower.SetStats(_towerStats);
				_placedTower.SetUpgradeLevel(data.UpgradeNumber);
				_placedTower.RebuildTower();
				_placedTower.TowerBuild(this);
			}
		}
        else
        {
			_placedTower = _towerFactory.CreateNewTower(data, transform.position);
			_placedTower.SetStats(_towerStats);
			_placedTower.SetUpgradeLevel(data.UpgradeNumber);
			_placedTower.TowerBuild(this);
			_type = data.Type;
        }

		InitializeInfoContainer();
		Untouch();
	}

	public void EnableUpgrades(List<TowerData> towerUpgrades)
    {
		_containerInfo.UpgradesList = towerUpgrades;

		if (_isTouched)
        {
			_bottomMenuInformator.UpdateUpgradesInfo(_containerInfo);
		}
	}

	public void Untouch()
	{
		_isTouched = false;

		if (_placedTower != null)
			_placedTower.HideRange();
	}

	public bool IsTouched() => _isTouched;

	private void InitializeInfoContainer()
	{
		_towerStats.InitializeInfoContainer();
		_containerInfo = _towerStats.GetContainer();
		_containerInfo.Touchable = this;
    }
}
