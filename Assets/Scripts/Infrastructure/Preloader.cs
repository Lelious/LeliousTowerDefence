using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Preloader : MonoBehaviour
{
    [Inject] private SceneLoaderService _sceneLoader;

    private async void Start()
    {
        await _sceneLoader.LoadScene("StartScene");
        _sceneLoader.SwitchScenes("Preload");
    }
}
