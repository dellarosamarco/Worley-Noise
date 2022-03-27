using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorleyNoiseTexture : MonoBehaviour
{
    [Header("Components")]
    private GameObject worleyNoise;
    private SpriteRenderer worleyNoiseSpriteRenderer;
    private Texture2D worleyNoiseTexture;

    [Header("Configuration")]
    public Vector2Int gridSize;
    public Vector2Int totalChunks;

    [Header("Settings")]
    [Range(0, 100)]
    public int chunkDensity = 100;
    public float noiseMultiplier;
    public Color baseColor = Color.white;
    public bool colorInversion = false;

    [Header("Advanced Settings")]
    public int pixelsPerUnit = 10;

    private List<Chunk<Vector2>> chunks;
    private List<Vector2> points;

    private void Start()
    {
        generateWorleyNoiseTexture();

        generateCells();

        generateChunks();

        generatePoints();

        cellsIteration();
    }

    void generateWorleyNoiseTexture()
    {
        worleyNoise = new GameObject("Worley Noise");

        worleyNoiseTexture = new Texture2D(gridSize.x, gridSize.y, TextureFormat.RGBA32, false, linear:false);
        worleyNoiseTexture.filterMode = FilterMode.Trilinear;

        var sprite = Sprite.Create(worleyNoiseTexture, new Rect(0, 0, worleyNoiseTexture.width, worleyNoiseTexture.height), Vector2.zero, pixelsPerUnit);
        worleyNoiseSpriteRenderer = worleyNoise.AddComponent<SpriteRenderer>();
        worleyNoiseSpriteRenderer.sprite = sprite;

        worleyNoise.transform.position = new Vector2(-worleyNoiseTexture.width / 2f / pixelsPerUnit, -worleyNoiseTexture.height / 2f / pixelsPerUnit);
    }

    void generateCells()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                worleyNoiseTexture.SetPixel(x, y, new Color(baseColor.r, baseColor.g, baseColor.b, Random.Range(0.0f, 1f)));
                worleyNoiseTexture.Apply();
            }
        }
    }

    void generateChunks()
    {
        int _totalChunks = totalChunks.x * totalChunks.y;

        chunks = new List<Chunk<Vector2>>();

        for (int i = 0; i < _totalChunks; i++)
        {
            chunks.Add(new Chunk<Vector2>());
        }
    }

    void generatePoints()
    {
        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;
        Vector2 tempVector;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);
                int chunkIndex = cellToChunkIndex(cellPosition, xChunkSize, yChunkSize, totalChunks);
                tempVector.x = x;
                tempVector.y = y;
                chunks[chunkIndex].addCell(tempVector);
            }
        }

        points = new List<Vector2>();

        foreach (Chunk<Vector2> chunk in chunks)
        {
            if (chunkDensity > UnityEngine.Random.Range(0, 100))
            {
                points.Add(chunk.setPoint());
            }
        }
    }

    void cellsIteration()
    {
        if (points.Count == 0)
            return;

        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;

        Vector2 tempCell = Vector2.zero;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tempCell.x = x;
                tempCell.y = y;
                float distance = Vector2.Distance(tempCell, points[0]);

                for (int n = 1; n < points.Count; n++)
                {
                    tempCell.x = x;
                    tempCell.y = y;
                    float _distance = Vector2.Distance(tempCell, points[n]);

                    if (_distance < distance)
                    {
                        distance = _distance;
                    }
                }

                distance = colorInversion ? 1 - ((distance / xChunkSize / pixelsPerUnit) * noiseMultiplier) : ((distance / xChunkSize / pixelsPerUnit) * noiseMultiplier);
                worleyNoiseTexture.SetPixel(x,y, new Color(baseColor.r, baseColor.g, baseColor.b, distance));
            }
        }

        worleyNoiseTexture.Apply();
    }

    private int cellToChunkIndex(Vector2Int cellPosition, int xChunkSize, int yChunkSize, Vector2Int totalChunks)
    {
        int finalIndex = 0;
        int xIndex = 0;
        int yIndex = 0;
        int x = cellPosition.x;
        int y = cellPosition.y;
        int temp = 1;

        while (x >= xChunkSize * temp)
        {
            temp += 1;
        }
        xIndex = temp;

        temp = 1;

        while (y >= yChunkSize * temp)
        {
            temp += 1;
        }
        yIndex = temp;

        finalIndex = (yIndex - 1) * totalChunks.x + xIndex;

        return finalIndex - 1;
    }

    private float timer=0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > 0.1f)
        {
            timer = 0;
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = getNewCellInsideChunk(points[i]);
            }

            cellsIteration();
        }
    }

    private Vector2 tempVector;
    private Vector2 getNewCellInsideChunk(Vector2 startingPoing)
    {
        tempVector.x = startingPoing.x + Random.Range(-1, 2);
        tempVector.y = startingPoing.y + Random.Range(-1, 2);

        if (tempVector.x < 0)
            tempVector.x = 0;
        else if (tempVector.x > gridSize.x - 1)
            tempVector.x = gridSize.x - 1;

        if (tempVector.y < 0)
            tempVector.y = 0;
        else if (tempVector.y > gridSize.y - 1)
            tempVector.y = gridSize.y - 1;

        return tempVector;
    }

    
}
