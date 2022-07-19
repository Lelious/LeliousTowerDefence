using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private FPSCounter _fpsCounter;
    [SerializeField] private TextMeshProUGUI _counter;
    [SerializeField] private GameObject _platforms;
    [SerializeField] private GameObject _trees;
    [SerializeField] private GameObject _rocks;
    [SerializeField] private GameObject _etc;

    private protected void Update()
    {
        _counter.text = CachedStringValues.cachedStringValues[Mathf.Clamp(_fpsCounter.FPS, 0, 99)];
    }

    public void DisablePlatforms()
    {
        if (_platforms.activeInHierarchy == true)
        {
            _platforms.SetActive(false);
        }
        else
        {
            _platforms.SetActive(true);
        }
    }
    public void DisableTrees()
    {
        if (_trees.activeInHierarchy == true)
        {
            _trees.SetActive(false);
        }
        else
        {
            _trees.SetActive(true);
        }
    }
    public void DisableRocks()
    {
        if (_rocks.activeInHierarchy == true)
        {
            _rocks.SetActive(false);
        }
        else
        {
            _rocks.SetActive(true);
        }
    }
    public void DisableEtc()
    {
        if (_etc.activeInHierarchy == true)
        {
            _etc.SetActive(false);
        }
        else
        {
            _etc.SetActive(true);
        }
    }
}
