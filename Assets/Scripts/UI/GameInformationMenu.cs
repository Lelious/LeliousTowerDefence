using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInformationMenu : MonoBehaviour
{
	[SerializeField] private RectTransform _emptyCellMenuRect;
	[SerializeField] private RectTransform _gameMenuRect;
	[SerializeField] private List<UIButton> _emptyButtons = new List<UIButton>();
	[SerializeField] private List<TowerData> _startingTowers = new List<TowerData>();
	[SerializeField] private BottomMenuInformator _bottomMenuInformator;

	private TapRegistrator _tapRegistrator;
	private GameManager _gameManager;
	private BuildingCell _buildingCell;
	private float _emptyCellMenuHeight;
	private float _gameMenuHeight;

	[Inject]
	private void Construct(GameManager gameManager, TapRegistrator tapRegistrator)
	{	
		_gameManager = gameManager;
		_tapRegistrator = tapRegistrator;
	}

	private protected void Awake()
	{
		_emptyCellMenuHeight = _emptyCellMenuRect.rect.height;
		_gameMenuHeight = _gameMenuRect.rect.height;

		InitializeEmptyBuildButtons();

		TapRegistrator.OnEmptyTapRegistered += HideGameMenu;
		TapRegistrator.OnEmptyTapRegistered += HideEmptyCellMenu;
	}

	public void ShowEmptyCellMenu()
	{
		_emptyCellMenuRect.DOAnchorPos(Vector2.zero, 0.25f);

		HideGameMenu();
	}

	public void HideEmptyCellMenu()
	{
		_emptyCellMenuRect.DOAnchorPos(new Vector2(0f, -_emptyCellMenuHeight), 0.25f);
	}

	public void ShowGameMenu()
	{
		_gameMenuRect.DOAnchorPos(Vector2.zero, 0.25f);

		HideEmptyCellMenu();
	}

	public void HideGameMenu()
	{
		_gameMenuRect.DOAnchorPos(new Vector2(0f, -_gameMenuHeight), 0.25f);
	}

	public void RegisterTapOnUI()
	{
		_tapRegistrator.RegisterUITap();
	}

	public void SetBuildingCell(BuildingCell cell) => _buildingCell = cell;
	public BuildingCell GetLastTouchedBuildingCell() => _buildingCell;
	public BottomMenuInformator GetBottomMenuInformator() => _bottomMenuInformator;


	private void InitializeEmptyBuildButtons()
	{
		for (int i = 0; i < _emptyButtons.Count; i++)
		{
			_emptyButtons[i].SetButton(_startingTowers[i], _gameManager, this, _tapRegistrator);
		}
	}

	private void OnDestroy()
	{
		TapRegistrator.OnEmptyTapRegistered -= HideGameMenu;
		TapRegistrator.OnEmptyTapRegistered -= HideEmptyCellMenu;
	}
}
