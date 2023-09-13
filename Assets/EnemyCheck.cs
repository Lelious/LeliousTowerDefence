using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
	[SerializeField] private CapsuleCollider _collider;
	[SerializeField] private Transform _selection;

	public List<IDamagable> _enemiesList = new List<IDamagable>();

	public List<IDamagable> GetEnemies()
	{
		_enemiesList.RemoveAll(x => x.CanBeAttacked() == false);		
		return _enemiesList.OrderBy(x => Vector3.Distance(transform.position, x.GetOrigin().position)).ToList();
	}

	public void SetAttackRange(float value)
	{
		_enemiesList.Clear();
		_collider.radius = 0.01f;
		_collider.radius = value;
		_selection.localScale = Vector3.one * value;
    }

	private protected void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamagable source))
		{
			if (!_enemiesList.Contains(source) && source != null)
			{
				_enemiesList.Add(source);
			}
		}		
	}

	private protected void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IDamagable source))
		{
			if (_enemiesList.Contains(source))
			{
				_enemiesList.Remove(source);
			}
		}
	}
}
