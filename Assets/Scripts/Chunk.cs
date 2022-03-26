using System.Collections.Generic;
using UnityEngine;

public class Chunk<T>
{
    private List<T> cells;

    public Chunk()
    {
        cells = new List<T>();
    }

    public void addCell(T cell)
    {
        cells.Add(cell);
    }

    public T setPoint()
    {
        int index = Random.Range(0, cells.Count);

        if(cells[index].GetType() == typeof(Cell))
        {
            (cells[index] as Cell).setPoint();
        }

        return cells[index];
    }

    public int getIndex(T element)
    {
        if (cells.Contains(element))
        {
            return cells.IndexOf(element);
        }
        else 
        {
            return -1;
        }
    }
}
