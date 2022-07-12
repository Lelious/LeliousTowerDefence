using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
	private NavMeshAgent _navMeshAgent;
	[SerializeField] private Transform _point;
	private protected void Awake()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.SetDestination(_point.position);
	}
}
