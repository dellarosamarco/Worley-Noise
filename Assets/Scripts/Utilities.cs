using UnityEngine;

public static class Utilities
{
    public static Color tempColor = new Color(0f, 0f, 0f, 1f);

    public static Color reachColor(Color color, Color endColor, Vector3 RGBoffset)
    {
        if (Mathf.Abs(color.r - endColor.r) < RGBoffset.x)
        {
            color.r = endColor.r;
        }
        else
        {
            if (color.r > endColor.r)
                color.r -= RGBoffset.x;
            else
                color.r += RGBoffset.x;
        }

        if (Mathf.Abs(color.g - endColor.g) < RGBoffset.y)
        {
            color.g = endColor.g;
        }
        else
        {
            if (color.g > endColor.g)
                color.g -= RGBoffset.y;
            else
                color.g += RGBoffset.y;
        }

        if (Mathf.Abs(color.b - endColor.b) < RGBoffset.z)
        {
            color.b = endColor.b;
        }
        else
        {
            if (color.b > endColor.b)
                color.b -= RGBoffset.z;
            else
                color.b += RGBoffset.z;
        }

        return color;
    }

    public static Color randomColor()
    {
        tempColor.r = Random.Range(0f, 1f);
        tempColor.g = Random.Range(0f, 1f);
        tempColor.b = Random.Range(0f, 1f);

        return tempColor;
    }
}
