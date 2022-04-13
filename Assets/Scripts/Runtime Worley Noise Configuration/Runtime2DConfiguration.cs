using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runtime2DConfiguration : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject content;
    public RectTransform panelRectTransform;

    private float initialPanelRectTransformHeigh;

    private void Start()
    {
        initialPanelRectTransformHeigh = panelRectTransform.sizeDelta.y;
    }

    public void onToggle()
    {
        bool isOpened = content.activeSelf;
        content.SetActive(!isOpened);

        panelRectTransform.sizeDelta = 
            new Vector2(
                panelRectTransform.sizeDelta.x, 
                isOpened ? 75 : initialPanelRectTransformHeigh
            );
    }
}
