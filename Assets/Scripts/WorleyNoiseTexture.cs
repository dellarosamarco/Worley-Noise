using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorleyNoiseTexture : MonoBehaviour
{
    [Header("Components")]
    public WorleyNoiseMesh worleyNoiseMesh;
    private GameObject worleyNoise;
    private SpriteRenderer worleyNoiseSpriteRenderer;
    private Texture2D worleyNoiseTexture;

    [Header("Configuration")]
    public Vector2Int gridSize;
    public Vector2Int totalChunks;

    [Header("Settings")]
    [Range(0, 100)]
    public int chunkDensity = 100;
    public int pixelsPerUnit = 10;
    public float noiseMultiplier;
    public Color baseColor = Color.white;
    public bool colorInversion = false;

    [Header("Advanced Settings")]
    public float movementDelay = 0.1f;
    public bool dynamicChunks = false;
    public float dynamicChunksSpeed = 15f;
    public bool dynamicBaseColor = false;
    public float dynamicBaseColorChangeDelay = 0.0f;
    public bool renderTargets = false;
    public bool viewChunks = false;
    public bool viewChunksPoints = false;

    [Header("Cells Iteration Visualization")]
    public bool visualizeCellsIteration = false;
    public bool singleCellRendering = false;
    public bool columnRendering = false;
    public int totalColumnRendering = 0;
    public bool visualizeChunkPointToCellLine = false;

    private List<Chunk<Vector2>> chunks;
    private List<Vector2> points;
    private List<Vector2> pointsTargets;

    // Others
    private float dynamicBaseColorTimer = 0.0f;
    private Color dynamicBaseColorTarget = new Color();
    private Coroutine _cellsIterationCoroutine;
    private Vector3 colorIncreaser = new Vector3(0.01f, 0.01f, 0.01f);

    // Temp variables
    private Color tempColor = new Color();
    private Vector2 tempVector;
    private Vector2Int tempIntVector;
    private Vector3 tempVector3 = Vector3.zero;

    private void Start()
    {
        dynamicBaseColorTarget = baseColor;

        generateWorleyNoiseTexture();

        generateCells();

        generateChunks();

        generatePoints();

        generatePointsTargets();

        startCellIteration();

        generateWorleyNoiseMesh();
    }

    void generateWorleyNoiseTexture()
    {
        worleyNoise = new GameObject("Worley Noise");
        worleyNoise.transform.rotation = Quaternion.Euler(90, 0, 0);

        worleyNoiseTexture = new Texture2D(gridSize.x, gridSize.y, TextureFormat.RGBA32, false, linear:false);
        worleyNoiseTexture.filterMode = FilterMode.Trilinear;

        var sprite = Sprite.Create(worleyNoiseTexture, new Rect(0, 0, worleyNoiseTexture.width, worleyNoiseTexture.height), Vector2.zero, pixelsPerUnit);
        worleyNoiseSpriteRenderer = worleyNoise.AddComponent<SpriteRenderer>();
        worleyNoiseSpriteRenderer.sprite = sprite;

        worleyNoise.transform.position = new Vector2(-worleyNoiseTexture.width / 2f / pixelsPerUnit, -worleyNoiseTexture.height / 2f / pixelsPerUnit);
    }

    void generateCells()
    {
        Color tempColor;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tempColor.r = baseColor.r;
                tempColor.g = baseColor.g;
                tempColor.b = baseColor.b;
                tempColor.a = Random.Range(0.0f, 1.0f);

                worleyNoiseTexture.SetPixel(x, y, tempColor);
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
        Vector2Int cellPosition = Vector2Int.zero;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellPosition.x = x;
                cellPosition.y = y;

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

    void startCellIteration()
    {
        if (
            !visualizeCellsIteration || 
            (!singleCellRendering && !columnRendering)
            )
            cellsIteration();
        else
            _cellsIterationCoroutine = StartCoroutine(cellsIterationCoroutine());
    }

    void cellsIteration()
    {
        if (points.Count == 0)
            return;

        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;

        Vector2 tempCell = Vector2.zero;

        if (dynamicBaseColor && dynamicBaseColorTimer > dynamicBaseColorChangeDelay)
        {
            baseColor = Utilities.reachColor(baseColor, dynamicBaseColorTarget, colorIncreaser);

            dynamicBaseColorTimer = 0f;

            if(baseColor == dynamicBaseColorTarget)
            {
                dynamicBaseColorTarget = Utilities.randomColor();
            }
        }

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

                tempColor.r = baseColor.r;
                tempColor.g = baseColor.g;
                tempColor.b = baseColor.b;
                tempColor.a = distance;

                worleyNoiseTexture.SetPixel(x, y, tempColor);
            }
        }

        worleyNoiseTexture.Apply();
    }

    IEnumerator cellsIterationCoroutine()
    {
        if (points.Count == 0)
            StopCoroutine(_cellsIterationCoroutine);

        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;

        Vector2 tempCell = Vector2.zero;

        if (dynamicBaseColor && dynamicBaseColorTimer > dynamicBaseColorChangeDelay)
        {
            baseColor = Utilities.reachColor(baseColor, dynamicBaseColorTarget, colorIncreaser);

            dynamicBaseColorTimer = 0f;

            if (baseColor == dynamicBaseColorTarget)
            {
                dynamicBaseColorTarget = Utilities.randomColor();
            }
        }

        int columns = 0;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tempCell.x = x;
                tempCell.y = y;
                float distance = Vector2.Distance(tempCell, points[0]);

                Vector2 selectedPoint = points[0];

                for (int n = 1; n < points.Count; n++)
                {
                    tempCell.x = x;
                    tempCell.y = y;
                    float _distance = Vector2.Distance(tempCell, points[n]);

                    if (_distance < distance)
                    {
                        distance = _distance;
                        selectedPoint = points[n];
                    }
                }

                distance = colorInversion ? 1 - ((distance / xChunkSize / pixelsPerUnit) * noiseMultiplier) : ((distance / xChunkSize / pixelsPerUnit) * noiseMultiplier);

                tempColor.r = baseColor.r;
                tempColor.g = baseColor.g;
                tempColor.b = baseColor.b;
                tempColor.a = distance;

                worleyNoiseTexture.SetPixel(x, y, tempColor);

                if(singleCellRendering && !columnRendering)
                {
                    worleyNoiseTexture.Apply();

                    if (visualizeChunkPointToCellLine)
                    {
                        tempVector.x = tempCell.x / (float)pixelsPerUnit - gridSize.x / 2 / (float)pixelsPerUnit;
                        tempVector.y = tempCell.y / (float)pixelsPerUnit - gridSize.y / 2 / (float)pixelsPerUnit;

                        Vector3 left = tempVector;

                        tempVector.x = selectedPoint.x / (float)pixelsPerUnit - gridSize.x / 2 / (float)pixelsPerUnit;
                        tempVector.y = selectedPoint.y / (float)pixelsPerUnit - gridSize.y / 2 / (float)pixelsPerUnit;

                        Vector3 right = tempVector;
                        Debug.DrawLine(left, right, Color.white);
                    }
                    yield return null;
                }
            }

            if (columnRendering)
            {
                if(columns >= totalColumnRendering || totalColumnRendering > gridSize.x - x)
                {
                    columns = 0;
                    worleyNoiseTexture.Apply();
                    yield return null;
                }
                else
                {
                    columns++;
                }
            }
            
        }
    }

    void generatePointsTargets()
    {
        pointsTargets = new List<Vector2>();

        for (int i = 0; i < points.Count; i++)
        {
            pointsTargets.Add(getPointInsideTexture());
        }
    }

    void generateSinglePointsTarget(int index)
    {
        pointsTargets[index] = getPointInsideTexture();
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

    private void generateWorleyNoiseMesh()
    {
        StartCoroutine(
            worleyNoiseMesh.GenerateMesh(
                gridSize.x / pixelsPerUnit, 
                gridSize.y / pixelsPerUnit,
                worleyNoiseTexture,
                pixelsPerUnit
            )
        );
    }

    private void Update()
    {
        if (dynamicChunks) 
        {
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = Vector2.MoveTowards(points[i], pointsTargets[i], dynamicChunksSpeed * Time.deltaTime);

                if(points[i] == pointsTargets[i])
                {
                    generateSinglePointsTarget(i);
                }
            }

            startCellIteration();
        }

        if (dynamicBaseColor)
        {
            dynamicBaseColorTimer += Time.deltaTime;
        }
    }

    private Vector2 getNewCellInsideChunk(Vector2 startingPoint, int rangeX, int RangeY)
    {
        tempVector.x = startingPoint.x + Random.Range(-1, 2);
        tempVector.y = startingPoint.y + Random.Range(-1, 2);

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

    private Vector2Int getPointInsideTexture()
    {
        tempIntVector.x = Random.Range(0, gridSize.x);
        tempIntVector.y = Random.Range(0, gridSize.y);
        return tempIntVector;
    }

    private Vector3 startChunkLine;
    private Vector3 endChunkLine;
    private void FixedUpdate()
    {
        if(dynamicChunks && renderTargets)
        {
            for (int i = 0; i < points.Count; i++)
            {
                float pointsX = (points[i].x / (float)pixelsPerUnit) - gridSize.x / 2 / (float)pixelsPerUnit;
                float pointsY = (points[i].y / (float)pixelsPerUnit) - gridSize.y / 2 / (float)pixelsPerUnit;

                tempVector3.x = pointsX;
                tempVector3.y = pointsY;
                tempVector3.z = 0;

                Vector3 worldPosPoint = transform.TransformPoint(tempVector3);

                float targetX = (pointsTargets[i].x / (float)pixelsPerUnit) - gridSize.x / 2 / (float)pixelsPerUnit;
                float targetY = (pointsTargets[i].y / (float)pixelsPerUnit) - gridSize.y / 2 / (float)pixelsPerUnit;

                tempVector3.x = targetX;
                tempVector3.y = targetY;
                tempVector3.z = 0;

                Vector3 worldPosTarget = transform.TransformPoint(tempVector3);

                Color lineColor = Color.white;
                lineColor.a = Vector3.Distance(worldPosPoint, worldPosTarget) / pixelsPerUnit;

                Debug.DrawLine(worldPosPoint, worldPosTarget, lineColor);
            }
        }

        if (viewChunks)
        {
            float xChunkSize = (gridSize.x / (float)pixelsPerUnit) / totalChunks.x;
            float yChunkSize = (gridSize.y / (float)pixelsPerUnit) / totalChunks.y;

            for (int i = 0; i < totalChunks.x + 1; i++)
            {
                startChunkLine.x = -gridSize.x / 2 / (float)pixelsPerUnit + xChunkSize * i;
                startChunkLine.y = gridSize.y / 2 / (float)pixelsPerUnit;

                endChunkLine.x = startChunkLine.x;
                endChunkLine.y = -startChunkLine.y;

                Debug.DrawLine(startChunkLine, endChunkLine, Color.yellow);
            }

            for (int i = 0; i < totalChunks.y + 1; i++)
            {
                startChunkLine.x = -gridSize.x / 2 / (float)pixelsPerUnit;
                startChunkLine.y = -gridSize.y / 2 / (float)pixelsPerUnit + yChunkSize * i;

                endChunkLine.x = -startChunkLine.x;
                endChunkLine.y = startChunkLine.y;

                Debug.DrawLine(startChunkLine, endChunkLine, Color.yellow);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && viewChunksPoints)
        {
            for (int i = 0; i < points.Count; i++)
            {
                tempVector3.x = points[i].x / (float)pixelsPerUnit - gridSize.x / 2 / (float)pixelsPerUnit;
                tempVector3.y = points[i].y / (float)pixelsPerUnit - gridSize.y / 2 / (float)pixelsPerUnit;
                tempVector3.z = 0;

                Gizmos.DrawSphere(tempVector3, 0.1f);
            }
        }
    }
}
