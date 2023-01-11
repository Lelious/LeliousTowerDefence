using System.Collections.Generic;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
	[SerializeField] private CapsuleCollider _collider;
	[SerializeField] private Transform _selection;

	private List<IDamagable> _enemiesList = new List<IDamagable>();

	public List<IDamagable> EnemiesList
	{
		get { return _enemiesList; }
		set { _enemiesList.RemoveAll(x => x.CanBeAttacked() == false);}
	}

	public void SetAttackRange(float value)
	{
		_collider.radius = value;
		_selection.localScale = Vector3.one * value;
    }

	private protected void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamagable source))
		{
			if (!EnemiesList.Contains(source) && source != null)
			{
				EnemiesList.Add(source);
			}
		}		
	}

	private protected void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IDamagable source))
		{
			if (EnemiesList.Contains(source))
			{
				EnemiesList.Remove(source);
			}
		}
	}
}
