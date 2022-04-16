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
    public Slider noiseMultiplier2D;
    public Slider cellularSpeed;
    public Toggle colorInversion;
    public Toggle dynamicCellular;
    public Toggle dynamicColor;

    private float initialPanelRectTransformHeigh;

    private void Start()
    {
        initialPanelRectTransformHeigh = panelRectTransform.sizeDelta.y;

        noiseMultiplier2D.onValueChanged.AddListener(delegate { 
            eventsHandler(noiseMultiplier2D.value, Event.NOISE_MULTIPLIER); 
        });

        cellularSpeed.onValueChanged.AddListener(delegate {
            eventsHandler(cellularSpeed.value, Event.CELLULAR_SPEED);
        });

        colorInversion.onValueChanged.AddListener(delegate { 
            eventsHandler(colorInversion.isOn, Event.COLOR_INVERSION); 
        });

        dynamicCellular.isOn = false;
        dynamicCellular.onValueChanged.AddListener(delegate {
            eventsHandler(dynamicCellular.isOn, Event.DYNAMIC_CELLULAR);
        });

        dynamicColor.isOn = false;
        dynamicColor.onValueChanged.AddListener(delegate {
            eventsHandler(dynamicColor.isOn, Event.DYNAMIC_COLOR);
        });
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
    public void eventsHandler(float value, Event _event)
    {
        switch (_event)
        {
            case Event.NOISE_MULTIPLIER:
                WorleyNoiseTexture.instance.noiseMultiplier = value;
                break;
            case Event.CELLULAR_SPEED:
                WorleyNoiseTexture.instance.dynamicChunksSpeed = value;
                break;
        }

        WorleyNoiseTexture.instance.cellsIteration();
    }

    public void eventsHandler(bool value, Event _event)
    {
        switch (_event)
        {
            case Event.COLOR_INVERSION:
                WorleyNoiseTexture.instance.colorInversion = value;
                break;
            case Event.DYNAMIC_CELLULAR:
                WorleyNoiseTexture.instance.dynamicChunks = value;
                break;
            case Event.DYNAMIC_COLOR:
                WorleyNoiseTexture.instance.dynamicBaseColor = value;
                break;
        }

        WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
