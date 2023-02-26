using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIButton : MonoBehaviour
{
	[SerializeField] private Image _image;
	[SerializeField] private Text _name;

    [Space]
    [Header("Tower Button Parameters")]
    [SerializeField] private Text _attackDamage;
    [SerializeField] private Text _attackRange;
    [SerializeField] private Text _attackSpeed;
    [SerializeField] private Text _buildingTime;
    [SerializeField] private Text _goldCost;	

    [Inject] private GameUIService _gameInformationMenu;
    [Inject] private GameManager _gameManager;
    [Inject] private TapRegisterService _tapRegistrator;
    private StringBuilder _stringBuilder = new StringBuilder();
    [SerializeField] private TowerData _towerData;
	private string[] _preparedColors = new string[3] { "#28F533", "#F5E14B", "#FF0000" };

	public void SetButton(TowerData data)
	{
		_towerData = data;
		_name.text = _towerData.Name;
		_image.sprite = _towerData.MainImage;

		ValidateDamage(data.MinimalDamage, data.MaximumDamage);
        ValidateAttackRange(data.AttackRadius * 100);
		ValidateAttackSpeed(1 / data.AttackSpeed);
		ValidateBuildingTime(data.BuildingTime);
		ValidateCost(data.Cost);
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

	private void ValidateDamage(int minDamage, int maxDamage)
	{
        _stringBuilder.Clear();
        _stringBuilder.Append("Attack Damage : <color=");
		_stringBuilder.Append($"{_preparedColors[0]}> ");
		_stringBuilder.Append($"{minDamage}</color> - <color={_preparedColors[0]}>{maxDamage}</color>");
        _attackDamage.text = _stringBuilder.ToString();
    }

	private void ValidateAttackRange(float value)
	{
        _stringBuilder.Clear();
        _stringBuilder.Append("Attack Range : <color=");

        if (value < 700f)
            _stringBuilder.Append($"{_preparedColors[2]}>");
        if (value >= 700f && value < 1000f)
            _stringBuilder.Append($"{_preparedColors[1]}>");
        if (value >= 1000f)
            _stringBuilder.Append($"{_preparedColors[0]}>");
        string trim = string.Format("{0:f0}", value);
        _stringBuilder.Append($"{trim}</color>");
		_attackRange.text = _stringBuilder.ToString();
    }

	private void ValidateAttackSpeed(float value)
	{
        _stringBuilder.Clear();
        _stringBuilder.Append("Attack Speed : <color=");

        if (value < 1f)
            _stringBuilder.Append($"{_preparedColors[2]}>");
        if (value >= 1f && value < 1.5f)
            _stringBuilder.Append($"{_preparedColors[1]}>");
        if (value >= 1.5)
            _stringBuilder.Append($"{_preparedColors[0]}>");

		string trim = string.Format("{0:f2}", value);

        _stringBuilder.Append($"{trim}</color> ");
		_stringBuilder.Append("p/s");
		_attackSpeed.text = _stringBuilder.ToString();
    }

	private void ValidateBuildingTime(int value)
	{
        _stringBuilder.Clear();
        _stringBuilder.Append("Building Time : <color=");
        if (value < 4)
            _stringBuilder.Append($"{_preparedColors[0]}>");
        if (value >= 4 && value < 7)
            _stringBuilder.Append($"{_preparedColors[1]}>");
        if (value >= 7)
            _stringBuilder.Append($"{_preparedColors[2]}>");
        _stringBuilder.Append($"{value}</color> ");
        _stringBuilder.Append("sec");
        _buildingTime.text = _stringBuilder.ToString();
    }

	private void ValidateCost(int value)
	{
        _stringBuilder.Clear();
        _stringBuilder.Append("Gold Cost : <color=");
		_stringBuilder.Append($"{_preparedColors[1]}>{value}</color>");
		_goldCost.text = _stringBuilder.ToString();
    }
}
