using System;
using UnityEngine;
using Zenject;

public class BuildingCell : MonoBehaviour, ITouchable
{
	private GameInformationMenu _gameInformationMenu;
	private TowerFactory _towerFactory;
	private NewTower _placedTower;
	private TowerData _towerData;

	[Inject]
	private void Construct(GameInformationMenu gameInformationMenu, TowerFactory towerFactory)
	{
		_gameInformationMenu = gameInformationMenu;
		_towerFactory = towerFactory;
	}

	private protected void FixedUpdate()
	{
		//if (!_isEmpty)
		//{
		//	if (_buildCellChanger.Selected == gameObject)
		//	{
		//		if (!_placedTower.GetBuildStatus())
		//		{
		//			UpgradeHealth();
		//		}

		//		_selected = true;
		//		_placedTower.ShowRange();
		//		UpgradeHealth();
		//	}
		//	else
		//	{
		//		if (_selected)
		//		{
		//			_selected = false;
		//			_placedTower.DisableRange();
		//		}
		//	}
		//}
	}

	private void UpgradeInfo()
	{
		//_attackSpeed = _placedTower.GetAttackSpeed();
		//_minDamage = _placedTower.GetMinDamage();
		//_maxDamage = _placedTower.GetMaxDamage();
		//_image = _placedTower.GetTowerImage();
		//_name = _placedTower.GetTowerName();
		//_towerHealth = _placedTower.GetHealth();
		//_color = _placedTower.GetHealthColor();
		//_menuUpdater.UpgradeInformation(_image, _name, _minDamage, _maxDamage, 0, _attackSpeed, _towerHealth, _color);
	}
	//private void UpgradeHealth()
	//{
	//	_towerHealth = _placedTower.GetHealth();
	//	_color = _placedTower.GetHealthColor();
	//	_menuUpdater.UpgradeInformation(_image, _name, _minDamage, _maxDamage, 0, _attackSpeed, _towerHealth, _color);
	//}

	public void Touch()
	{
		if (_placedTower)
		{
			_placedTower.ShowRange();
			_gameInformationMenu.ShowGameMenu();
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
		if (_placedTower)				
			_placedTower.HideRange();
	}

	public Vector3 GetPosition()
	{
		return transform.position;
	}	
}
