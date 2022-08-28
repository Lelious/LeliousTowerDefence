using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
	private List<EnemyHealth> _enemiesList = new List<EnemyHealth>();

	public List<EnemyHealth> EnemiesList
	{
		get { return _enemiesList; }
		set { _enemiesList.RemoveAll(x => x.gameObject == null); }
	}

	private protected void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<EnemyHealth>(out var source))
		{
			if (!EnemiesList.Contains(source) && source != null)
			{
				EnemiesList.Add(source);
			}
		}		
	}

	private protected void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent<EnemyHealth>(out var source))
		{
			if (EnemiesList.Contains(source))
			{
				EnemiesList.Remove(source);
			}
		}
	}
}
