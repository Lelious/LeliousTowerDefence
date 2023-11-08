using System.Collections.Generic;
using UnityEngine;

public class BottomMenuIconsContainer : MonoBehaviour
{
	[SerializeField] private List<UIIconsKeyValuePair> _pairsList = new List<UIIconsKeyValuePair>();

	private Dictionary<UIMenuIcons, GameObject> _menuIconsDictionary;

	private void Awake()
	{
		_menuIconsDictionary = new Dictionary<UIMenuIcons, GameObject>();

		foreach (var kvp in _pairsList)		
			_menuIconsDictionary[kvp.Key] = kvp.Value;		
	}

	public void RemoveUnusedIcons(List<UIMenuIcons> unusedTypesList)
	{
		EnableAllIcons();

		foreach (var item in unusedTypesList)
		{
			_menuIconsDictionary.TryGetValue(item, out var unusedType);
			unusedType.SetActive(false);
		}
	}

	private void EnableAllIcons()
	{
		foreach (var item in _menuIconsDictionary.Values)		
			item.SetActive(true);		
	}
}
