using UnityEngine;

[CreateAssetMenu(fileName = "GameMap", menuName = "ScriptableObjects/GameMap/GameMapColorScheme", order = 1)]
public class GameMapColorScheme : ScriptableObject
{
    public Texture2D MaskTexture;
    public Texture2D BuildPlaneTexture;
    public Texture2D CliffTexture;
    public Texture2D RoadTexture;
    public Texture2D FloorEmissionTexture;
    public Color FogColor;
    public bool IsGlowingFloor;
    [ColorUsage(true, true)]
    public Color InnerFloorGlow;
    [ColorUsage(true, true)]
    public Color OuterFloorGlow;
    public Color TreeColor1;
    public Color TreeColor2;
    public bool TreeLeaf;
}
