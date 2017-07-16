using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// VOManagerEditor - Description:
/// Custom inspector editor for VOManager.
/// </summary>

// set the custom editor type to VOManager script
[CustomEditor(typeof(VOManager))]
public class VOManagerEditor : Editor
{
    // private variables
    private VOManager voManager;   // reference to the VOManager

    private void OnEnable()
    {
        // set the reference to the current inspected object
        voManager = (VOManager)target;
    }

    // overrite inspector interface
    public override void OnInspectorGUI()
    {
        // record any changes done within the VOManager
        Undo.RecordObjects(targets, "VOManager");

        EditorGUILayout.Space();

        // if subtitle text component is empty
        if (voManager.txtSubtitle == null)
            // display a warning message
            EditorGUILayout.HelpBox("No UI Text component is found. Please set the subtitle Text component.", MessageType.Warning);

        // subtitle text component field
        voManager.txtSubtitle = (Text)EditorGUILayout.ObjectField("Subtitle Object", voManager.txtSubtitle, typeof(Text), true);

        EditorGUILayout.Space();
    }
}