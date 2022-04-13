using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorleyNoiseTexture))]
public class InspectorRuntimeEditor : Editor
{
    override public void OnInspectorGUI()
    {
        WorleyNoiseTexture worleyNoiseTexture = (WorleyNoiseTexture)target;
        if (GUILayout.Button("Generate"))
        {
            worleyNoiseTexture.init();
        }
        DrawDefaultInspector();
    }
}
