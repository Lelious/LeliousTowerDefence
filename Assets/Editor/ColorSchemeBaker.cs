using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorSchemeBaker : EditorWindow
{
    private GameMapColorScheme colorScheme;
    private Material mapMaterial;
    private Material fogMaterial;
    private Material treeMaterial;
    private bool isGlowFloor;

    [MenuItem("CONTEXT/Material/Bake Scheme")]
    private static void Open(MenuCommand command)
    {
        var window = GetWindow<ColorSchemeBaker>();
        window.titleContent = new GUIContent("Color Scheme Baker");
        window.mapMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Enviroment/Lelious_EnvironmentUnlit.mat");
        window.fogMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/FogGameMap.mat");
        window.treeMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/Enviroment/TreeMat.mat");
        window.ShowModalUtility();
    }

    private void CreateGUI()
    {       
        var gameMapColorScheme = new ObjectField("ColorScheme")
        {
            objectType = typeof(GameMapColorScheme),
            value = null,
            allowSceneObjects = false
        };
        gameMapColorScheme.RegisterValueChangedCallback(OnColorSchemeChanged);

        Toggle glowFloorToggle = new Toggle("Is Glow Floor")
        {
            value = false
        };
        glowFloorToggle.RegisterValueChangedCallback(OnGlowFloor);

        rootVisualElement.Add(gameMapColorScheme);
        rootVisualElement.Add(glowFloorToggle);
        rootVisualElement.Add(new Button(CreateColorScheme) { text = "Create Color Scheme" });
    }

    private void OnGlowFloor(ChangeEvent<bool> evt)
    {
        isGlowFloor = evt.newValue;
    }

    private void OnColorSchemeChanged(ChangeEvent<Object> evt)
    {
        colorScheme = evt.newValue as GameMapColorScheme;
    }

    private void CreateColorScheme()
    {
        if (mapMaterial == null || fogMaterial == null || colorScheme == null)
        {
            Debug.LogWarning("Not all fields filled!");
            Close();
            return;
        }

        colorScheme.MaskTexture = mapMaterial.GetTexture("_Mask") as Texture2D;
        colorScheme.BuildPlaneTexture = mapMaterial.GetTexture("_Tex1") as Texture2D;
        colorScheme.CliffTexture = mapMaterial.GetTexture("_Tex2") as Texture2D;
        colorScheme.RoadTexture = mapMaterial.GetTexture("_Tex3") as Texture2D;
        colorScheme.FogColor = fogMaterial.GetColor("_BaseColor");
        colorScheme.IsGlowingFloor = isGlowFloor;

        if (isGlowFloor)
        {
            colorScheme.FloorEmissionTexture = mapMaterial.GetTexture("_TexEmission") as Texture2D;
            colorScheme.InnerFloorGlow = mapMaterial.GetColor("_EmissionColorH");
            colorScheme.OuterFloorGlow = mapMaterial.GetColor("_EmissionColorL");
        }

        colorScheme.TreeColor1 = treeMaterial.GetColor("_Color");
        colorScheme.TreeLeaf = treeMaterial.GetFloat("_RenderLeaf") > 0 ? true : false;

        EditorUtility.SetDirty(colorScheme);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Close();
    }
}
