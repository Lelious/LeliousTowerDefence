using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInformationMenu : MonoBehaviour
{
	[SerializeField] private RectTransform _emptyCellMenuRect;
	[SerializeField] private RectTransform _gameMenuRect;
	[SerializeField] private List<UIButton> _emptyButtons = new List<UIButton>();
	[SerializeField] private List<TowerData> _startingTowers = new List<TowerData>();

	private GameManager _gameManager;
	private BuildingCell _buildingCell;
	private float _emptyCellMenuHeight;
	private float _gameMenuHeight;

	[Inject]
	private void Construct(GameManager gameManager) =>	
		_gameManager = gameManager;

	private protected void Awake()
	{
		_emptyCellMenuHeight = _emptyCellMenuRect.rect.height;
		_gameMenuHeight = _gameMenuRect.rect.height;

		InitializeEmptyBuildButtons();

		InputService.OnEmptyTapRegistered += HideGameMenu;
		InputService.OnEmptyTapRegistered += HideEmptyCellMenu;
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

	public void SetBuildingCell(BuildingCell cell) => _buildingCell = cell;
	public BuildingCell GetLastTouchedBuildingCell() => _buildingCell;


	private void InitializeEmptyBuildButtons()
	{
		for (int i = 0; i < _emptyButtons.Count; i++)
		{
			_emptyButtons[i].SetButton(_startingTowers[i], _gameManager, this);
		}
	}

	private void OnDestroy()
	{
		InputService.OnEmptyTapRegistered -= HideGameMenu;
		InputService.OnEmptyTapRegistered -= HideEmptyCellMenu;
	}
}
