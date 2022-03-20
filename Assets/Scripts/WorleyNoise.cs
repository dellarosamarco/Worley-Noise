using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WorleyNoise : MonoBehaviour
{
    public Vector2Int gridSize;
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
                cells[x, y] = new Cell(cellPosition, 1,cellSize);
            }
        }
    }


    //void FixedUpdate()
    //{
    //    Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), Color.white);
    //}
}
