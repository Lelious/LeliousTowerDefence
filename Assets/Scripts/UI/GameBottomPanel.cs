using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameBottomPanel : MonoBehaviour
{
	[SerializeField] private RectTransform _emptyCellMenu;
	[SerializeField] private RectTransform _gameMenu;
	[SerializeField, Range(0, 1000)]
	private float _emptyCellMenuHeight;
	[SerializeField, Range(0, 1000)]
	private float _emptyGameMenuHeight;

	public void ShowEmptyCellMenu()
	{
		_emptyCellMenu.DOAnchorPos(Vector2.zero, 0.25f);
		HideGameMenu();
	}

	public void HideEmptyCellMenu()
	{
		_emptyCellMenu.DOAnchorPos(new Vector2(0f, -_emptyCellMenuHeight), 0.25f);
	}

	public void ShowGameMenu()
	{
		_gameMenu.DOAnchorPos(Vector2.zero, 0.25f);
		HideEmptyCellMenu();
	}

	public void HideGameMenu()
	{
		_gameMenu.DOAnchorPos(new Vector2(0f, -_emptyGameMenuHeight), 0.25f);
	}
}
