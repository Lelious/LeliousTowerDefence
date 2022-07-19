using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
	[SerializeField, Range(0, 100f)] private float _speed;
	public void InitializePath(Vector3[] newPath)
	{
		transform.DOPath(newPath, _speed, PathType.Linear).SetLookAt(0.01f).SetLoops(-1).SetSpeedBased();
	}
}
