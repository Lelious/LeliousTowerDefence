using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
	[SerializeField] private List<GameObject> _enemiesList = new List<GameObject>();

	public List<GameObject> GetEnemies()
	{
		return _enemiesList;
	}

	private protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3 && !_enemiesList.Contains(other.gameObject))
		{
			_enemiesList.Add(other.gameObject);

			if (_enemiesList.Count > 0)
			{
				var enemy = _enemiesList[0];
				if (enemy == null)
				{
					_enemiesList.Remove(enemy);
				}
			}

		}
	}

	private protected void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			_enemiesList.Remove(other.gameObject);
		}
	}
}
