using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BeatController))]
public class BeatControllerEditor : Editor
{

    int curPattern = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        BeatController beats = (BeatController) target;
        if(beats.patterns == null ||  beats.patterns.Count == 0)
        {
            beats.patterns = new List<List<int>>();
            beats.patterns.Add(new List<int>());
            curPattern = 0;
        }
        
        GUILayout.Label("Patter Editor:");
        GUILayout.BeginHorizontal("", GUILayout.Height(30));
        for (int i = 0; i < 16; i++)
        {
            if (beats.patterns[curPattern].Contains(i))
            {
                if (GUILayout.Button("x"))
                {
                    beats.patterns[curPattern].Remove(i);
                }
            }
            else
            {
                if (GUILayout.Button(" "))
                {
                    beats.patterns[curPattern].Add(i);
                }
            }

        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("", GUILayout.Height(30));
        if (GUILayout.Button("Prev"))
        {
            curPattern--;
            curPattern = Mathf.Max(0, curPattern);
            for(int i = beats.patterns.Count -1; i > curPattern; i--)
            {
                if(beats.patterns[i].Count == 0)
                {
                    beats.patterns.RemoveAt(i);
                }
            }
        }

        if (GUILayout.Button(curPattern.ToString())) { }
        if (GUILayout.Button("Next"))
        {
            curPattern++;
            if(curPattern >= beats.patterns.Count)
            {
                beats.patterns.Add(new List<int>());
            }
        }
        GUILayout.EndHorizontal();

        for(int i = 0; i < beats.patterns.Count; i++)
        {
            GUILayout.Label(i.ToString() + ": " + beats.patterns[i].Count.ToString());
        }
    }
}