using UnityEngine;

[ExecuteAlways]
public class VertexColorsCopier : MonoBehaviour
{
    [SerializeField] private MeshFilter sourceMeshFilter;           
    [SerializeField] private SkinnedMeshRenderer targetSkinnedMesh;

    [ContextMenu("Copy Vertex Colors")]
    public void CopyColorsByPosition()
    {
        if (sourceMeshFilter == null || targetSkinnedMesh == null)
        {
            Debug.LogError("Укажи Source и Target!");
            return;
        }

        Mesh sourceMesh = sourceMeshFilter.sharedMesh;
        Mesh targetMesh = targetSkinnedMesh.sharedMesh;

        Vector3[] sourceVertices = sourceMesh.vertices;
        Color[] sourceColors = sourceMesh.colors;

        Vector3[] targetVertices = targetMesh.vertices;
        Color[] targetColors = new Color[targetVertices.Length];

        if (sourceColors == null || sourceColors.Length == 0)
        {
            Debug.LogError("В исходном меше нет цветов вершин!");
            return;
        }

        // для каждой вершины в target ищем ближайшую в source
        for (int i = 0; i < targetVertices.Length; i++)
        {
            float minDist = float.MaxValue;
            int closestIndex = 0;

            for (int j = 0; j < sourceVertices.Length; j++)
            {
                float dist = (targetVertices[i] - sourceVertices[j]).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = j;
                }
            }

            targetColors[i] = sourceColors[closestIndex];
        }

        targetMesh.colors = targetColors;
        Debug.Log("Vertex Colors скопированы через сопоставление по позициям!");
    }
}
