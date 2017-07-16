using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// VOBankEditor - Description:
/// Custom inspector editor for VOBank.
/// </summary>

// set the custom editor type to VOBank script
[CustomEditor(typeof(VOBank))]
public class VOBankEditor : Editor
{
    // private variables
    private VOBank voBank;   // reference to the VOBank

    private void OnEnable()
    {
        // set the reference to the current inspected object
        voBank = (VOBank)target;
    }

    // overrite inspector interface
    public override void OnInspectorGUI()
    {
        // record any changes done within the VOBank
        Undo.RecordObjects(targets, "VOBank");

        EditorGUILayout.Space();

        // info box to inform user if they have no clips in the sound bank
        if (voBank.bank.Count == 0)
            EditorGUILayout.HelpBox("You currently have no sound clips in your sound bank.", MessageType.Info);

        // if the add clip button is pressed
        if (GUILayout.Button("Add Clip"))
            voBank.AddClip();   // add a clip to the bank

        // for each clip inside the bank starting from the highest to lowest (order)
        for (int i = (voBank.bank.Count - 1); i > -1; i--)
        {
            // record any changes done within the VOBank bank list
            Undo.RecordObjects(targets, "ClipProperties");

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            // if the current clip has no name
            if (voBank.bank[i].strName == null)
                // set the name to clip plus it's id number (auto assigned via i)
                voBank.bank[i].strName = "Clip " + i;

            // expand or collapse the clip's panel depending on the boolean status
            // and name the panel based on its id and clip name
            voBank.bank[i].blnExpandClipPnl = EditorGUILayout.Foldout(voBank.bank[i].blnExpandClipPnl, "[ID " + i + "] " + voBank.bank[i].strName);

            // if the remove button is clicked on
            if (GUILayout.Button("Remove Clip"))
                voBank.RemoveClip(i);   // remove that current audio clip from the bank

            EditorGUILayout.EndHorizontal();

            // try and run the following
            try
            {
                // if the clip's panel is expanded
                if (voBank.bank[i].blnExpandClipPnl)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Clip Properties", EditorStyles.boldLabel);

                    voBank.bank[i].id = EditorGUILayout.IntField("ID", i);  // set the clip's id (auto / non-changeable)
                    voBank.bank[i].strName = EditorGUILayout.TextField("Clip Name", voBank.bank[i].strName);   // show the name field

                    // the the subtitle text field
                    EditorGUILayout.LabelField("Subtitle Text");
                    voBank.bank[i].strSubtitle = EditorGUILayout.TextArea(voBank.bank[i].strSubtitle);

                    // show the audio clip field
                    voBank.bank[i].audClpDialogue = (AudioClip)EditorGUILayout.ObjectField("Dialogue Clip", voBank.bank[i].audClpDialogue, typeof(AudioClip), true);

                    EditorGUILayout.EndVertical();
                }
            }
            catch
            {
                // throw a warning if there is more then one item in the bank
                if (voBank.bank.Count > 1)
                    Debug.LogError("There is an error reading the bank due to the custom editor.");

                return;
            }
        }

        EditorGUILayout.Space();
    }
}