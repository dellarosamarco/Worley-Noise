using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runtime2DConfiguration : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject content;
    public RectTransform panelRectTransform;

    [Header("2D Configuration Components")]
    public Slider NoiseMultiplier2D;

    private float initialPanelRectTransformHeigh;

    private void Start()
    {
        initialPanelRectTransformHeigh = panelRectTransform.sizeDelta.y;
        NoiseMultiplier2D.onValueChanged.AddListener(delegate { sliderHandler(NoiseMultiplier2D.value); });
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

    //2D CHANGE CONFIGURATION EVENTS
    #region
    public void sliderHandler(float value)
    {
        WorleyNoiseTexture.instance.noiseMultiplier = value;
        WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
