using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorleyNoiseTexture))]
public class RuntimeEditor : Editor
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
