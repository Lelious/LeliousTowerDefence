using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]

public class SceneSaver : MonoBehaviour
{
	[SerializeField, Range(1, 6000)] private int _seconds = 10;
	private bool _isActivated;
	private void Update()
	{
		if (!_isActivated)
		{
			StartCoroutine(SaveScene());
			_isActivated = true;
		}

	}
	private IEnumerator SaveScene()
	{
		while (true)
		{
			EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
			yield return new WaitForSeconds(_seconds);
			Debug.Log("Scene sucseccfuly saved!");
		}
	}
}
