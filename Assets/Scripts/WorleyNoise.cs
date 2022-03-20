using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WorleyNoise : MonoBehaviour
{
    public Vector2Int gridSize;
    public Vector2Int totalChunks;
    public Vector2 cellSize;
    private float[,] map;
    private Cell[,] cells;

    private void Start()
    {
        map = new float[gridSize.x, gridSize.y];
        cells = new Cell[gridSize.x, gridSize.y];

        Vector2 cellPosition = new Vector2(0,0);

        int xOffset = (int)gridSize.x/2;
        int yOffset = (int)gridSize.y/2;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cellPosition.x = (x * cellSize.x) - (xOffset * cellSize.x);
                cellPosition.y = (y * cellSize.y) - (yOffset * cellSize.y);

                map[x, y] = 1;
                cells[x, y] = new Cell(cellPosition, Random.Range(0.0f,0.2f), cellSize);
            }
        }

        generatePoints();
    }

    void generatePoints()
    {
        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;
        int _totalChunks = totalChunks.x * totalChunks.y;

        List<Chunk> chunks = new List<Chunk>();

        for (int i = 0; i < _totalChunks; i++)
        {
            chunks.Add(new Chunk());
        }

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);
                int chunkIndex = cellToChunkIndex(cellPosition, xChunkSize, yChunkSize, totalChunks);
                chunks[chunkIndex].addCell(cells[x, y]);
            }
        }

        foreach(Chunk chunk in chunks)
        {
            chunk.setPoint();
        }
    }

    private int cellToChunkIndex(Vector2Int cellPosition, int xChunkSize, int yChunkSize, Vector2Int totalChunks)
    {
        int finalIndex = 0;
        int xIndex = 0;
        int yIndex = 0;
        int x = cellPosition.x;
        int y = cellPosition.y;
        int temp = 1;

        while(x >= xChunkSize * temp)
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

        return finalIndex-1;
    }

    Vector3 left_temp;
    Vector3 right_temp;
    void FixedUpdate()
    {
        int xChunkSize = gridSize.x / totalChunks.x;
        int yChunkSize = gridSize.y / totalChunks.y;

        float xOffset = gridSize.x / 2 * cellSize.x;
        float yOffset = gridSize.y / 2 * cellSize.y;

        left_temp.y = (gridSize.y / 2) * cellSize.y - cellSize.y / 2; ;
        right_temp.y = -(gridSize.y / 2) * cellSize.y - cellSize.y / 2; ;

        for (int x = 0; x <= totalChunks.x; x++)
        {
            left_temp.x = (x * xChunkSize * cellSize.x) - xOffset - cellSize.x / 2;
            right_temp.x = (x * xChunkSize * cellSize.x) - xOffset - cellSize.x / 2;

            Debug.DrawLine(left_temp, right_temp, Color.yellow);
        }

        left_temp.x = (gridSize.x / 2) * cellSize.x - cellSize.x / 2; ;
        right_temp.x = -(gridSize.x / 2) * cellSize.x - cellSize.x / 2; ;

        for (int y = 0; y <= totalChunks.y; y++)
        {
            left_temp.y = (y * yChunkSize * cellSize.y) - yOffset - cellSize.y / 2;
            right_temp.y = (y * yChunkSize * cellSize.y) - yOffset - cellSize.y / 2;

            Debug.DrawLine(left_temp, right_temp, Color.yellow);
        }
    }
}
