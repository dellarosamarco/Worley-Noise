using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Runtime2DConfiguration : MonoBehaviour
{
    [Header("2D Configuration Components")]
    public Slider noiseMultiplier2D;
    public Slider cellularSpeed;
    public Toggle colorInversion;
    public Toggle dynamicCellular;
    public Toggle dynamicColor;
    public Slider RBaseColor;
    public Slider GBaseColor;
    public Slider BBaseColor;

    private void Start()
    {
        noiseMultiplier2D.value = 8.0f;
        noiseMultiplier2D.onValueChanged.AddListener(delegate { 
            eventsHandler(noiseMultiplier2D.value, Event2D.NOISE_MULTIPLIER); 
        });

        cellularSpeed.onValueChanged.AddListener(delegate {
            eventsHandler(cellularSpeed.value, Event2D.CELLULAR_SPEED);
        });

        colorInversion.onValueChanged.AddListener(delegate { 
            eventsHandler(colorInversion.isOn, Event2D.COLOR_INVERSION); 
        });

        dynamicCellular.isOn = false;
        dynamicCellular.onValueChanged.AddListener(delegate {
            eventsHandler(dynamicCellular.isOn, Event2D.DYNAMIC_CELLULAR);
        });

        dynamicColor.isOn = false;
        dynamicColor.onValueChanged.AddListener(delegate {
            eventsHandler(dynamicColor.isOn, Event2D.DYNAMIC_COLOR);
        });

        RBaseColor.onValueChanged.AddListener(delegate {
            eventsHandler(RBaseColor.value, Event2D.R_BASE_COLOR);
        });

        GBaseColor.onValueChanged.AddListener(delegate {
            eventsHandler(GBaseColor.value, Event2D.G_BASE_COLOR);
        });

        BBaseColor.onValueChanged.AddListener(delegate {
            eventsHandler(BBaseColor.value, Event2D.B_BASE_COLOR);
        });
    }

    //2D CHANGE CONFIGURATION EVENTS
    #region
    public void eventsHandler(float value, Event2D _event)
    {
        switch (_event)
        {
            case Event2D.NOISE_MULTIPLIER:
                WorleyNoiseTexture.instance.noiseMultiplier = value;
                break;
            case Event2D.CELLULAR_SPEED:
                WorleyNoiseTexture.instance.dynamicChunksSpeed = value;
                break;
            case Event2D.R_BASE_COLOR:
                WorleyNoiseTexture.instance.baseColor.r = value / 255f;
                break;
            case Event2D.G_BASE_COLOR:
                WorleyNoiseTexture.instance.baseColor.g = value / 255f;
                break;
            case Event2D.B_BASE_COLOR:
                WorleyNoiseTexture.instance.baseColor.b = value / 255f;
                break;
        }

        if(!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }

    public void eventsHandler(bool value, Event2D _event)
    {
        switch (_event)
        {
            case Event2D.COLOR_INVERSION:
                WorleyNoiseTexture.instance.colorInversion = value;
                break;
            case Event2D.DYNAMIC_CELLULAR:
                WorleyNoiseTexture.instance.dynamicChunks = value;
                break;
            case Event2D.DYNAMIC_COLOR:
                WorleyNoiseTexture.instance.dynamicBaseColor = value;
                break;
        }

        if (!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
