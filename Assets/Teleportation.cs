using UnityEngine;
using UnityEngine.AI;

public class Teleportation : MonoBehaviour
{
	[SerializeField] private Transform _startPoint;

	private protected void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 3)
		{
			var navMesh = other.GetComponent<NavMeshAgent>();
			navMesh.Warp(_startPoint.transform.position);
			other.transform.rotation = Quaternion.identity;
			navMesh.SetDestination(transform.position);
		}
	}
}
