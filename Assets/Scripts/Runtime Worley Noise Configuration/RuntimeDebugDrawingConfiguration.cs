using UnityEngine;
using UnityEngine.UI;

public class RuntimeDebugDrawingConfiguration : MonoBehaviour
{
    [Header("Debug Drawing Configuration Components")]
    public Toggle viewChunks;
    public Toggle viewChunksPoints;
    public Toggle viewChunksTargets;
    public Toggle viewMeshVertices;

    private void Start()
    {
        viewChunks.isOn = false;
        viewChunks.onValueChanged.AddListener(delegate
        {
            eventsHandler(viewChunks.isOn, EventDebugDrawing.VIEW_CHUNKS);
        });

        viewChunksPoints.isOn = false;
        viewChunksPoints.onValueChanged.AddListener(delegate
        {
            eventsHandler(viewChunksPoints.isOn, EventDebugDrawing.VIEW_CHUNKS_POINTS);
        });

        viewChunksTargets.isOn = false;
        viewChunksTargets.onValueChanged.AddListener(delegate
        {
            eventsHandler(viewChunksTargets.isOn, EventDebugDrawing.VIEW_CHUNKS_TARGETS);
        });

        viewMeshVertices.isOn = true;
        viewMeshVertices.onValueChanged.AddListener(delegate
        {
            eventsHandler(viewMeshVertices.isOn, EventDebugDrawing.VIEW_MESH_VERTICES);
        });
    }

    //DEBUG DRAWING CHANGE CONFIGURATION EVENTS
    #region
    public void eventsHandler(bool value, EventDebugDrawing _event)
    {
        switch (_event)
        {
            case EventDebugDrawing.VIEW_CHUNKS:
                WorleyNoiseTexture.instance.viewChunks = value;
                break;
            case EventDebugDrawing.VIEW_CHUNKS_POINTS:
                WorleyNoiseTexture.instance.viewChunksPoints = value;
                break;
            case EventDebugDrawing.VIEW_CHUNKS_TARGETS:
                WorleyNoiseTexture.instance.renderTargets = value;
                break;
            case EventDebugDrawing.VIEW_MESH_VERTICES:
                WorleyNoiseMesh.instance.renderGizmosVertices = value;
                break;
        }

        if (!WorleyNoiseTexture.instance.visualizeCellsIteration)
            WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion

    //ACTIONS
    #region
    public void restart()
    {
        WorleyNoiseTexture.instance.init();
    }

    public void randomGeneration()
    {
        WorleyNoiseTexture.instance.noiseMultiplier = Random.Range(4f, 17.5f);
        WorleyNoiseTexture.instance.colorInversion = Random.Range(0, 10) > 5;
        WorleyNoiseTexture.instance.dynamicChunks = Random.Range(0, 10) > 5;
        WorleyNoiseTexture.instance.dynamicChunksSpeed = Random.Range(0f, 100f);
        WorleyNoiseTexture.instance.dynamicBaseColor = Random.Range(0, 10) > 5;
        WorleyNoiseTexture.instance.baseColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        WorleyNoiseTexture.instance.generateMesh = Random.Range(0, 10) > 4;
        WorleyNoiseTexture.instance.dynamicMesh = Random.Range(0, 10) > 5;
        WorleyNoiseMesh.instance.noiseMultiplier = Random.Range(-5f, 5f);

        if (WorleyNoiseTexture.instance.generateMesh)
            WorleyNoiseTexture.instance.generateWorleyNoiseMesh();
        else
            WorleyNoiseMesh.instance.clear();


        WorleyNoiseTexture.instance.init();
    }
    #endregion
}
