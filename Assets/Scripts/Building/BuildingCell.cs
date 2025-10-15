using System.Collections.Generic;
using UnityEngine;

public class BuildingCell	
{
	private GamePannelUdaterInfoContainer _containerInfo;
	private BottomGameMenu _bottomMenuInformator;
	private GameUIService _gameInformationMenu;
	private TowerFactory _towerFactory;
	private PoolService _poolService;
	private Tower _placedTower;
	private TowerData _towerData;
	private TowerStats _towerStats;
	private bool _isActiveCell;
	private TowerType _type;
	private Vector3 _position;

	public void Construct(GameUIService gameUIService, TowerFactory towerFactory, PoolService poolService)
	{
		_gameInformationMenu = gameUIService;
		_towerFactory = towerFactory;
		_poolService = poolService;
	}

	public BuildingCell(Vector3 position) => _position = position;
	public Vector3 GetPosition() => _position;
	public Tower GetPlacedTower() => _placedTower;
	public void SetPlacedTower(Tower tower) => _placedTower = tower;
	public GamePannelUdaterInfoContainer GetContainer() => _containerInfo;

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
				_placedTower = _towerFactory.CreateNewTower(data, _position);
				_placedTower.SetupPoolService(_poolService);
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
			_placedTower = _towerFactory.CreateNewTower(data, _position);
			_placedTower.SetStats(_towerStats);
			_placedTower.SetUpgradeLevel(data.UpgradeNumber);
			_placedTower.TowerBuild(this);
			_type = data.Type;
        }

		InitializeInfoContainer();
	}

	public void ReleaseTower() => _towerFactory.ClearTowerData(_placedTower);

	public void EnableUpgrades(List<TowerData> towerUpgrades)
    {
		_containerInfo.UpgradesList = towerUpgrades;

		if (_isActiveCell)
        {
			_gameInformationMenu.GetBottomMenuInformator().UpdateUpgradesInfo(_containerInfo);
		}
	}

	public void SetActiveCell(bool active)
	{
		_isActiveCell = active;

		if (_isActiveCell == false && _placedTower != null) _placedTower.HideRange();
	}
	public bool IsActive() => _isActiveCell;

	private void InitializeInfoContainer()
	{
		_towerStats.InitializeInfoContainer(_placedTower.GetEffects());
		_containerInfo = _towerStats.GetContainer();
    }
}
