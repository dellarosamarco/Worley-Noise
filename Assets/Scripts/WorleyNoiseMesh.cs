using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WorleyNoiseMesh : MonoBehaviour
{
    [Header("Components")]
    public MeshFilter meshFilter;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        GenerateWorleyNoiseMesh(10, 10);
    }

    public void GenerateWorleyNoiseMesh(int gridSizeX, int gridSizeZ)
    {
        vertices = new Vector3[(gridSizeX + 1) * (gridSizeZ + 1)];
        int index = 0;

        for (int x = 0; x <= gridSizeX; x++)
        {
            for (int z = 0; z <= gridSizeZ; z++)
            {
                vertices[index] = new Vector3(x, Random.Range(0f,0f), z);
                index++;
            }
        }

        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                triangles = triangles.Concat(generateSquareMesh(gridSizeX * x + x + z, gridSizeX)).ToArray();
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
