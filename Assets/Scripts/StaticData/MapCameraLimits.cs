using UnityEngine;

[CreateAssetMenu(fileName = "CameraLimit", menuName = "ScriptableObjects/CameraLimitData", order = 1)]
public class MapCameraLimits : ScriptableObject
{
    public float LeftBoarderMin;
    public float LeftBoarderMax;
    public float RightBoarderMin;
    public float RightBoarderMax;
    public float UpLimitMin;
    public float UpLimitMax;
    public float DownLimitMin;
    public float DownLimitMax;
    public float HeightMin;
    public float HeightMax;
    public Vector3 InitialCameraPos;
}
