using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameUIService : MonoBehaviour
{
	[SerializeField] private RectTransform _emptyCellMenuRect;
	[SerializeField] private RectTransform _gameMenuRect;
	[SerializeField] private List<UIButton> _emptyButtons = new List<UIButton>();
	[SerializeField] private List<UIButton> _upgradablesButtons = new List<UIButton>();
	[SerializeField] private List<TowerData> _startingTowers = new List<TowerData>();
	[SerializeField] private BottomGameMenu _bottomMenuInformator;
	[SerializeField] private GameObject _bottomBuildMenu;
	[SerializeField] private GameObject _bottomGameMenu;

	private TapRegisterService _tapRegisterService;
	private GameManager _gameManager;
	private BuildingCell _buildingCell;
	private float _emptyCellMenuHeight;
	private float _gameMenuHeight;

	[Inject]
	private void Construct(GameManager gameManager, TapRegisterService tapRegisterService)
	{	
		_gameManager = gameManager;
		_tapRegisterService = tapRegisterService;
	}

	private protected void Awake()
	{
		_emptyCellMenuHeight = _emptyCellMenuRect.rect.height;
		_gameMenuHeight = _gameMenuRect.rect.height;

		InitializeEmptyBuildButtons();

		TapRegisterService.OnEmptyTapRegistered += HideGameMenu;
		TapRegisterService.OnEmptyTapRegistered += HideEmptyCellMenu;
	}

	public void ShowEmptyCellMenu()
	{
		_bottomBuildMenu.SetActive(true);
		_emptyCellMenuRect.DOAnchorPos(Vector2.zero, 0.25f);

		HideGameMenu();
	}

	public void HideEmptyCellMenu()
	{
		_emptyCellMenuRect.DOAnchorPos(new Vector2(0f, -_emptyCellMenuHeight), 0.25f).OnComplete(() => _bottomBuildMenu.SetActive(false));
	}

	public void ShowGameMenu(TowerData data = null)
	{
		_bottomGameMenu.SetActive(true);
		_gameMenuRect.DOAnchorPos(Vector2.zero, 0.25f);

		HideEmptyCellMenu();
	}

	public void HideGameMenu()
	{
		_gameMenuRect.DOAnchorPos(new Vector2(0f, -_gameMenuHeight), 0.25f).OnComplete(() => _bottomGameMenu.SetActive(false));
	}

	public void RegisterTapOnUI()
	{
		_tapRegisterService.RegisterUITap();
	}

	public void SetBuildingCell(BuildingCell cell) => _buildingCell = cell;
	public BuildingCell GetLastTouchedBuildingCell() => _buildingCell;
	public BottomGameMenu GetBottomMenuInformator() => _bottomMenuInformator;


	private void InitializeEmptyBuildButtons()
	{
		for (int i = 0; i < _emptyButtons.Count; i++)
		{
			_emptyButtons[i].SetButton(_startingTowers[i]);
		}
	}

	private void OnDestroy()
	{
		TapRegisterService.OnEmptyTapRegistered -= HideGameMenu;
		TapRegisterService.OnEmptyTapRegistered -= HideEmptyCellMenu;
	}
}
