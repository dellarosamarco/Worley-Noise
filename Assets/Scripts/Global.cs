using UnityEngine;

public class Global : MonoBehaviour
{
    public Sprite DEFAULT_CELL_SPRITE;

    public static Global instance;

    private void Awake()
    {
        instance = this;
    }
}
