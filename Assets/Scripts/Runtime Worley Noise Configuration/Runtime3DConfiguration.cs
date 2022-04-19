using UnityEngine.UI;
using UnityEngine;

public class Runtime3DConfiguration : MonoBehaviour
{
    [Header("2D Configuration Components")]
    public Toggle generateMesh;
    public Toggle dynamicMesh;
    public Slider noiseMultiplier;

    private void Start()
    {
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
                    WorleyNoiseMesh.instance.meshFilter.mesh.Clear();
                break;
            case Event3D.DYNAMIC_MESH:
                WorleyNoiseTexture.instance.dynamicMesh = value;
                break;
        }

        WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
