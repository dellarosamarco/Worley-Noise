# Worley Noise 3D / 2D - C#

https://user-images.githubusercontent.com/96957446/162939701-a073c575-a89d-4e4f-9d7e-47f84b249cf6.mp4

https://user-images.githubusercontent.com/96957446/162946850-109efa1f-628b-4ec0-a6b2-a2837ccf2f7e.mp4

<details close>
  <summary>Code</summary>
  
  ```C#
    public IEnumerator GenerateMesh(int gridSizeX, int gridSizeZ, Texture2D worleyNoiseTexture,int pixelsPerUnit)
    {
        Mesh mesh = new Mesh();

        float xOffset = -gridSizeX / 2;
        float yOffset = -4;

        int xSize = gridSizeX * (int)(1 / squareSize.x) + 1;
        int zSize = gridSizeZ * (int)(1 / squareSize.z) + 1;

        Vector3[] vertices = new Vector3[xSize * zSize];

        int index = 0;

        for (float z = 0; z <= gridSizeZ; z+=squareSize.z)
        {
            for (float x = 0; x <= gridSizeX; x+=squareSize.x)
            {
                vertices[index] = new Vector3(x + xOffset, yOffset, z);
                index++;
            }
        }

        int[] triangles = new int[xSize * zSize * 6];

        for (int z = 0; z < zSize-1; z++)
        {
            for (int x = 0; x < xSize-1; x++)
            {
                triangles = triangles.Concat(generateSquareMesh(z * (xSize - 1) + z + x, (xSize - 1))).ToArray();
                mesh.Clear();

                mesh.vertices = vertices;
                mesh.triangles = triangles;

                mesh.RecalculateNormals();

                yield return null;
            }
        }
    }
  ```
  
</details>

https://user-images.githubusercontent.com/96957446/161838754-bb8a69a5-e4c0-4d43-bd40-52677e82d0e1.mp4

https://user-images.githubusercontent.com/96957446/161443156-21b11576-1765-4305-a97f-528a7e88204a.mp4

https://user-images.githubusercontent.com/96957446/161128273-55ed1b96-cf8d-44ab-b296-fca294e9d851.mp4

https://user-images.githubusercontent.com/96957446/161379548-b2fa014d-cdd5-48f9-ba01-596205733c35.mp4

https://user-images.githubusercontent.com/96957446/163035975-287f88bc-8065-4110-a5a2-316e5d58ac12.mp4

https://user-images.githubusercontent.com/96957446/163036515-fd4e5568-753a-4837-b67f-dcfbd800553a.mp4
