using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorleyNoiseMesh : MonoBehaviour
{
    private static WorleyNoiseMesh instance;

    [Header("Components")]
    public Transform meshTransform;
    public MeshFilter meshFilter;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        GenerateWorleyNoiseMesh(12, 9);
    }

    public void GenerateWorleyNoiseMesh(int gridSizeX, int gridSizeZ)
    {
        float xOffset = -gridSizeX / 2;
        float yOffset = -4;
        meshTransform.position = Vector3.zero;

        vertices = new Vector3[(gridSizeX + 1) * (gridSizeZ + 1)];
        int index = 0;

        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z <= gridSizeZ; z++)
            {
                vertices[index] = new Vector3(x + xOffset, yOffset + Random.Range(-0.2f,0.2f), z);
                index++;
            }
        }

        triangles = new int[gridSizeX * gridSizeZ * 6];

        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                triangles = triangles.Concat(generateSquareMesh(x * gridSizeZ + x + z, gridSizeZ)).ToArray();
            }
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    int[] generateSquareMesh(int startingPoint, int xSize)
    {
        int[] square = new int[6]
        {
            startingPoint,
            startingPoint + 1 + xSize,
            startingPoint + 1,
            startingPoint + 1,
            startingPoint + 1 + xSize,
            startingPoint + 2 + xSize
        };
        return square;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);
        }
    }
}
