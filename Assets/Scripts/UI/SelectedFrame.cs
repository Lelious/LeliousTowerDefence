using UnityEngine;

public class SelectedFrame : MonoBehaviour
{
	[SerializeField] private GameObject _frame;
	public void EnableFrame()
	{
		_frame.SetActive(true);
	}

	public void DisableFrame()
	{
		_frame.SetActive(false);
	}
}
