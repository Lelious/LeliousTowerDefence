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
		_towerData = data;

        if (_placedTower != null)
        {
            if (_type != _towerData.Type)
            {
				_placedTower.ClearAllUnusedBullets();
				Destroy(_placedTower.gameObject);
				_placedTower = _towerFactory.CreateNewTower(data, transform.position);
				_placedTower.SetTowerData(data);
				_placedTower.TowerBuild(this);
			}
            else
            {
				_placedTower.SetTowerData(data);
				_placedTower.RebuildTower();
				_placedTower.TowerBuild(this);
			}
		}
        else
        {
			_placedTower = _towerFactory.CreateNewTower(data, transform.position);
			_placedTower.SetTowerData(data);
			_placedTower.TowerBuild(this);
			_type = data.Type;
        }

		InitializeInfoContainer();
	}

	public void EnableUpgrades()
    {
		InitializeInfoContainer(true);

		if (_isTouched)
        {
			_bottomMenuInformator.SetEntityToPannelUpdate(_containerInfo);
		}
	}

	public void Untouch()
	{
		_isTouched = false;
		Debug.Log("Untouch");
		if (_placedTower != null)
			_placedTower.HideRange();
	}

	public bool IsTouched() => _isTouched;

	private void InitializeInfoContainer(bool _isNeedUpgrades = false)
	{
		_containerInfo = new GamePannelUdaterInfoContainer
		{
			Touchable = this,
			PreviewImage = _towerData.MainImage,
			CurrentHealth = _placedTower.Health,
			Name = _towerData.Name,
			MaxHealth = _towerData.BuildingTime,
			MinDamage = _towerData.MinimalDamage,
			MaxDamage = _towerData.MaximumDamage,
			Armor = null,
			AttackSpeed = _towerData.AttackSpeed,
		};

        if (_isNeedUpgrades)
        {
			_containerInfo.UpgradesList = _towerData.UpgradablesList;
		}
    }
}
