using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public int FPS { get; private set; }

    private protected void Update()
    {
        FPS = (int)(1 / Time.unscaledDeltaTime);
    }
}
