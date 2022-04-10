using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

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

        StartCoroutine(GenerateWorleyNoiseMesh(12, 9));
    }

    IEnumerator GenerateWorleyNoiseMesh(int gridSizeX, int gridSizeZ)
    {
        Vector3 squareSize = new Vector3(0.5f, 0f, 0.5f);

        float xOffset = -gridSizeX / 2;
        float yOffset = -4;

        meshTransform.position = Vector3.zero;

        int xSize = gridSizeX * (int)(1 / squareSize.x) + 1;
        int zSize = gridSizeZ * (int)(1 / squareSize.z) + 1;

        vertices = new Vector3[xSize * zSize];
        int index = 0;

        for (float z = 0; z <= gridSizeZ; z+=squareSize.z)
        {
            for (float x = 0; x <= gridSizeX; x+=squareSize.x)
            {
                vertices[index] = new Vector3(x + xOffset, yOffset, z);
                index++;
            }
        }

        Debug.Log(vertices.Length);

        triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize-1; z++)
        {
            for (int x = 0; x < xSize-1; x++)
            {
                triangles = triangles.Concat(generateSquareMesh(z * (xSize - 1) + z + x, (xSize - 1))).ToArray();
                updateMesh();
                yield return null;
            }
        }

        updateMesh();
        yield return null;
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

    void updateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.05f);

            if(i > vertices.Length - 45)
            {
                //Debug.Log(vertices[i]);
            }
        }
    }
}
