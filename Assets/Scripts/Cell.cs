using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private GameObject cell;
    public SpriteRenderer spriteRenderer;
    public Transform transform;
    public Color color;
    private Color baseColor;

    public Cell(
        Vector2 position, 
        float color,
        Vector2 scale,
        Color baseColor
    ){
        //Create cell
        cell = new GameObject("Cell");
        transform = cell.transform;
        transform.position = position;

        //Create sprite
        spriteRenderer = cell.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Global.instance.DEFAULT_CELL_SPRITE;

        //Set color
        this.baseColor = baseColor;
        this.color = new Color(baseColor.r, baseColor.g, baseColor.b, color);
        spriteRenderer.color = this.color;

        //Set scale
        transform.localScale = scale;
    }

    public void setPoint()
    {
        spriteRenderer.color = Color.red;
    }

    public void setColor(float color)
    {
        this.color = new Color(baseColor.r, baseColor.g, baseColor.b, color);
        spriteRenderer.color = this.color;
    }
}
