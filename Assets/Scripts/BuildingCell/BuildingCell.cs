using UnityEngine;

public class BuildingCell : MonoBehaviour
{
	public NewTower placedTower;
	public bool _isEmpty = true;

	[SerializeField] private BottomMenuInformator _menuUpdater;
	[SerializeField] private Sprite _image;
	[SerializeField] private string _name;
	[SerializeField] private int _minDamage;
	[SerializeField] private int _maxDamage;
	[SerializeField] private float _attackSpeed;

	private BuildCellInitializer _buildCellChanger;
	private string _towerHealth;
	private Color _color;
	private bool _selected;
	private protected void Awake()
	{
		_buildCellChanger = FindObjectOfType<BuildCellInitializer>();
	}
	private protected void FixedUpdate()
	{
		if (!_isEmpty)
		{
			if (_buildCellChanger.Selected == gameObject)
			{
				if (!placedTower.GetBuildStatus())
				{
					UpgradeHealth();
				}

				_selected = true;
				placedTower.ShowRange();
				UpgradeHealth();
			}
			else
			{
				if (_selected)
				{
					_selected = false;
					placedTower.DisableRange();
				}
			}
		}
	}

	public void UpgradeInfo()
	{
		_attackSpeed = placedTower.GetAttackSpeed();
		_minDamage = placedTower.GetMinDamage();
		_maxDamage = placedTower.GetMaxDamage();
		_image = placedTower.GetTowerImage();
		_name = placedTower.GetTowerName();
		_towerHealth = placedTower.GetHealth();
		_color = placedTower.GetHealthColor();
		_menuUpdater.UpgradeInformation(_image, _name, _minDamage, _maxDamage, 0,  _attackSpeed, _towerHealth, _color);
	}
	private void UpgradeHealth()
	{
		_towerHealth = placedTower.GetHealth();
		_color = placedTower.GetHealthColor();
		_menuUpdater.UpgradeInformation(_image, _name, _minDamage, _maxDamage, 0, _attackSpeed, _towerHealth, _color);
	}
}
