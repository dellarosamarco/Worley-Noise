using System.Collections.Generic;
using UnityEngine;

public class Chunk 
{
    private List<Cell> cells;

    public Chunk()
    {
        cells = new List<Cell>();
    }

    public void addCell(Cell cell)
    {
        cells.Add(cell);
    }

    public Cell setPoint()
    {
        int index = Random.Range(0, cells.Count);
        cells[index].setPoint();
        return cells[index];
    }
}
