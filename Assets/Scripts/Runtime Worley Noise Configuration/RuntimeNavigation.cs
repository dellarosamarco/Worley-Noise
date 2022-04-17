using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeNavigation : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject[] content;
    public RectTransform panelRectTransform;
    public RectTransform arrowTransform;

    private float initialPanelRectTransformHeight;

    private void Start()
    {
        initialPanelRectTransformHeight = panelRectTransform.sizeDelta.y;
    }

    public void onToggle()
    {
        bool isOpened = content[0].activeSelf;

        foreach (GameObject _object in content)
        {
            _object.SetActive(!isOpened);
        }

        panelRectTransform.sizeDelta =
            new Vector2(
                panelRectTransform.sizeDelta.x,
                isOpened ? 75 : initialPanelRectTransformHeight
            );

        arrowTransform.rotation = Quaternion.Euler(0, 0, isOpened ? 0 : -90);
    }
}
