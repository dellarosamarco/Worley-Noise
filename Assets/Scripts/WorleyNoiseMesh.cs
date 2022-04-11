using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class WorleyNoiseMesh : MonoBehaviour
{
    private static WorleyNoiseMesh instance;

    [Header("Settings")]
    public Vector3 squareSize;

    [Header("Components")]
    public Transform meshTransform;
    public MeshFilter meshFilter;

    private Mesh mesh;
    private Vector3[] vertices;
    private Dictionary<Vector2, int> verticesMap = new Dictionary<Vector2, int>();
    private int[] triangles;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator GenerateMesh(
        int gridSizeX, 
        int gridSizeZ,
        Texture2D worleyNoiseTexture,
        int pixelsPerUnit
    )
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

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
                verticesMap.Add(new Vector2(x, z), index);
                index++;
            }
        }

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

        StartCoroutine(setWorleyNoise(worleyNoiseTexture, pixelsPerUnit));
    }

    public IEnumerator setWorleyNoise(Texture2D worleyNoiseTexture, int pixelsPerUnit)
    {
        int resolutionScale = pixelsPerUnit;

        int textureWidth = worleyNoiseTexture.width / resolutionScale;
        int textureHeight = worleyNoiseTexture.height / resolutionScale;

        Vector2 vertexIndex = Vector2.zero;

        for (int x = 0; x < worleyNoiseTexture.width; x += resolutionScale / (int)(1 / squareSize.x))
        {
            for (int y = 0; y < worleyNoiseTexture.height; y += resolutionScale / (int)(1 / squareSize.z))
            {
                float alpha = worleyNoiseTexture.GetPixel(x, y).a;
                vertices[verticesMap[vertexIndex]].y += alpha;
                updateMesh();
                vertexIndex.y += squareSize.z;
                yield return null;
            }

            vertexIndex.x += squareSize.x;
            vertexIndex.y = 0;
        }

        //for (int x = 0; x < worleyNoiseTexture.width; x += resolutionScale)
        //{
        //    for (int y = 0; y < worleyNoiseTexture.height; y += resolutionScale)
        //    {
        //        float alpha = worleyNoiseTexture.GetPixel(x, y).a;
        //        vertices[verticesMap[vertexIndex]].y += alpha;
        //        updateMesh();
        //        vertexIndex.y += squareSize.z;
        //        yield return null;
        //    }

        //    vertexIndex.x += squareSize.x;
        //    vertexIndex.y = 0;
        //}

        //for (int x = 0; x < textureWidth; x++)
        //{
        //    for (int z = 0; z < textureHeight; z++)
        //    {
        //        float alpha = worleyNoiseTexture.GetPixel(x * resolutionScale, z * resolutionScale).a;
        //        vertices[verticesMap[vertexIndex]].y += alpha;
        //        updateMesh();
        //        vertexIndex.y += squareSize.z;
        //        yield return null;
        //    }

        //    vertexIndex.x += squareSize.x;
        //    vertexIndex.y = 0;
        //}
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
        }
    }
}
