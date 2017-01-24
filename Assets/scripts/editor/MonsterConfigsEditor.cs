using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MonsterConfigs))]
public class MonsterConfigsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Apply Monster Prefabs"))
        {
            ((MonsterConfigs)target).UpdatePrefabs();
        }
    }
}