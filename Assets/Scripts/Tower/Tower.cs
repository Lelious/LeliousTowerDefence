using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour
{
	public Sprite mainImage;
	public string towerName;
	public GameObject towerUpgrade;

	private Vector3 _endPosition;

	private void Awake()
	{
		_endPosition = gameObject.transform.position;
		_endPosition.y += 1.5f;
	}

	public void TowerBuild()
	{
		gameObject.transform.DOMove(_endPosition, 2f);
	}
}
