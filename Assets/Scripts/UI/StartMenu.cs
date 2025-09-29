using UnityEngine;
using DG.Tweening;
using Zenject;
using Cysharp.Threading.Tasks;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadeGroup;
    [Inject] private SceneLoaderService _sceneLoader;

    private void Start()
    {
        _fadeGroup.DOFade(0f, 1f);
    }

    public async void StartGameButton()
    {
        await _fadeGroup.DOFade(1f, 1f).AsyncWaitForCompletion().AsUniTask();
        await _sceneLoader.LoadScene("GameScene");
        _sceneLoader.SwitchScenes();
    }
}
