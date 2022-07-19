using UnityEngine;

public class Gizmo : MonoBehaviour
{
	private Vector3 _position => new Vector3(transform.position.x, transform.position.y - 0.36f, transform.position.z);
	private Vector3 _size => new Vector3(2, 0.73f, 2);
	private protected void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(_position, _size);
	}
}
