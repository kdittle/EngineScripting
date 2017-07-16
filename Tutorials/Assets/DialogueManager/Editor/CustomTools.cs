using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomTools : MonoBehaviour
{
    [MenuItem("Tools/Dialogue Editor")]
    private static void ShowDialogueEditor()
    {
        DialogueEditor.ShowDialogueEditorWindow();
    }
}
