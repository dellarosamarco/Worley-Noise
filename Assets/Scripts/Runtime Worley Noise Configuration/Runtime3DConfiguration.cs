using UnityEngine.UI;
using UnityEngine;

public class Runtime3DConfiguration : MonoBehaviour
{
    [Header("3D Configuration Components")]
    public Toggle camera3D;
    public Toggle generateMesh;
    public Toggle dynamicMesh;
    public Slider noiseMultiplier;

    //Gradient
    public Slider gradient0R;
    public Slider gradient0G;
    public Slider gradient0B;
    public Slider gradient1R;
    public Slider gradient1G;
    public Slider gradient1B;

    private void Start()
    {
        camera3D.isOn = false;
        camera3D.onValueChanged.AddListener(delegate
        {
            eventsHandler(camera3D.isOn, Event3D.CAMERA_3D);
        });

        generateMesh.isOn = false;
        generateMesh.onValueChanged.AddListener(delegate {
            eventsHandler(generateMesh.isOn, Event3D.GENERATE_MESH);
        });

        dynamicMesh.isOn = false;
        dynamicMesh.onValueChanged.AddListener(delegate {
            eventsHandler(dynamicMesh.isOn, Event3D.DYNAMIC_MESH);
        });

        noiseMultiplier.onValueChanged.AddListener(delegate
        {
            eventsHandler(noiseMultiplier.value, Event3D.NOISE_MULTIPLIER);
        });

        setGradientDelegates();
    }

    //3D CHANGE CONFIGURATION EVENTS
    #region
    public void eventsHandler(float value, Event3D _event)
    {
        switch (_event)
        {
            case Event3D.NOISE_MULTIPLIER:
                WorleyNoiseMesh.instance.noiseMultiplier = value;
                break;
        }

        if (WorleyNoiseTexture.instance.generateMesh)
            WorleyNoiseTexture.instance.generateWorleyNoiseMesh();
        else
            WorleyNoiseMesh.instance.clear();

        if (!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }

    public void eventsHandler(bool value, Event3D _event)
    {
        switch (_event)
        {
            case Event3D.GENERATE_MESH:
                WorleyNoiseTexture.instance.generateMesh = value;

                if (value)
                    WorleyNoiseTexture.instance.generateWorleyNoiseMesh();
                else
                    WorleyNoiseMesh.instance.clear();
                break;
            case Event3D.DYNAMIC_MESH:
                WorleyNoiseTexture.instance.dynamicMesh = value;
                break;
            case Event3D.CAMERA_3D:
                if (value) CameraHandler.instance.set3D();
                else CameraHandler.instance.set2D();
                break;
        }

        if (!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion

    //3D GRADIENT EVENT
    #region
    private void setGradientDelegates()
    {
        gradient0R.onValueChanged.AddListener(delegate { gradientEventsHandler(); }); 
        gradient0G.onValueChanged.AddListener(delegate { gradientEventsHandler(); });
        gradient0B.onValueChanged.AddListener(delegate { gradientEventsHandler(); });
        gradient1R.onValueChanged.AddListener(delegate { gradientEventsHandler(); });
        gradient1G.onValueChanged.AddListener(delegate { gradientEventsHandler(); });
        gradient1B.onValueChanged.AddListener(delegate { gradientEventsHandler(); });
    }

    public void gradientEventsHandler()
    {
        GradientColorKey[] colorKey = new GradientColorKey[2]; 
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];

        colorKey[0].color = new Color(gradient0R.value / 255f, gradient0G.value / 255f, gradient0B.value / 255f);
        colorKey[0].time = 0.0f;
        colorKey[1].color = new Color(gradient1R.value / 255f, gradient1G.value / 255f, gradient1B.value / 255f);
        colorKey[1].time = 1.0f;

        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        WorleyNoiseMesh.instance.setGradient(colorKey, alphaKey);

        if (!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
