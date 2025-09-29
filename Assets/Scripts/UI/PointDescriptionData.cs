using UnityEngine;

[CreateAssetMenu(fileName = "MiniMap", menuName = "ScriptableObjects/MiniMapPointDescriptionData", order = 1)]
public class PointDescriptionData : ScriptableObject
{
    public string PointName;
    public Sprite WaveImage;
    public string WaveName;
    public int Count;
    public int Health;
    public int Reward;
    public string GameMapName;
    public GameMapColorScheme ColorScheme;
}
