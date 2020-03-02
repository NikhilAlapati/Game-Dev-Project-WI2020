using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerManager myScript = (PlayerManager)target;
        if (GUILayout.Button("Start New Round"))
        {
            myScript.StartNewRound();
        }
    }
}