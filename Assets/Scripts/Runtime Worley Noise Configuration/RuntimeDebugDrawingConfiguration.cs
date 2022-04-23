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

        WorleyNoiseTexture.instance.cellsIteration();
    }
    #endregion
}
