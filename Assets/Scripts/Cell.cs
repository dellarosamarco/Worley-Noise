using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private GameObject cell;
    private SpriteRenderer spriteRenderer;
    private Transform transform;
    private Color color;

    public Cell(
        Vector2 position, 
        float color,
        Vector2 scale
    ){
        //Create cell
        cell = new GameObject("Cell");
        transform = cell.transform;
        transform.position = position;

        //Create sprite
        spriteRenderer = cell.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Global.instance.DEFAULT_CELL_SPRITE;

        //Set color
        this.color = new Color(255f, 255f, 255f, color);
        spriteRenderer.color = this.color;

        //Set scale
        transform.localScale = scale;
    }
}
