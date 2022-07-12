using UnityEngine;

public class RagdollDelayedDeath : MonoBehaviour
{
	private protected void Awake()
	{
		Destroy(gameObject, 1.5f);
	}
}
