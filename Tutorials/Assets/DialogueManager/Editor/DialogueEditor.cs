using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class DialogueEditor : EditorWindow
{
    public static DialogueEditor dialogueEditor;

    //Create new Dialogue Instance
    private Rect inputRect;
    private const float inputPosX = 10.0f;
    private const float inputPosY = 10.0f;
    private const float inputWidth = 135.0f;
    private const float inputHeight = 20.0f;    
    
    private Rect createButtonRect;
    private const float buttonXPos = 145.0f;
    private const float buttonYPos = 10.0f;
    private const float buttonWidth = 135.0f;
    private const float buttonHeight = 20.0f;
    private string newInstanceName = "";
    private GUIContent createButtonContent = new GUIContent();


    //List of dialogue instances
    public static List<DialogueInstance> dialogueInstances;

    //Need a scroll view
    private Vector2 scrollPos;

    public static void ShowDialogueEditorWindow()
    {
        dialogueEditor = (DialogueEditor)GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        dialogueEditor.Show();
    }

    private void OnEnable()
    {
        dialogueInstances = new List<DialogueInstance>();
    }

    private void OnDisable()
    {
        
    }

    private void OnGUI()
    {
        inputRect = new Rect(inputPosX, inputPosY, inputWidth, inputHeight);
        newInstanceName = GUI.TextArea(inputRect, newInstanceName, 200);
        createButtonRect = new Rect(buttonXPos, buttonYPos, buttonWidth, buttonHeight);
        createButtonContent.text = "Create New";
        if(GUI.Button(createButtonRect, createButtonContent))
        {
            //Create new dialogue instance
            dialogueInstances.Add(DialogueInstance.CreateNewInstance(newInstanceName));
            SaveDialogueInstance();
        }

        if(dialogueInstances.Count == 0)
            LoadDialogueDatabase();

        //begin scroll view
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.BeginVertical();

        GUILayout.BeginArea(new Rect(10.0f, 40.0f, Screen.width - 10.0f, Screen.height - 10.0f));

        foreach(DialogueInstance instance in dialogueInstances)
        {
            if (instance != null)
                PrintRowInfo(instance.name);
        }

        GUILayout.EndArea();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        //DrawScroll();
    }

    private static void SaveDialogueInstance()
    {
        //To save data:
        //1) Save the instance name
        //1) Tell each instance to save all nodes and connections.
    }

    private static void LoadDialogueDatabase()
    {
       //Find an instance and load it
       //Send that instance name and load node data
    }

    private void DrawScroll()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        for(int i = 0; i < dialogueInstances.Count; i++)
        {
            GUIContent guiContent = new GUIContent();
            guiContent.text = dialogueInstances[i].name;
        }

        GUILayout.EndScrollView();
    }

    private void PrintRowInfo(string nameText)
    {
        GUIStyle style = new GUIStyle();
        style.padding = new RectOffset(0, 0, 2, 2);

        if (GUILayout.Button("Edit", GUILayout.MinWidth(50.0f), GUILayout.MaxWidth(50.0f)))
        {
            DialogueInstance inst = (DialogueInstance)EditorWindow.GetWindow(typeof(DialogueInstance), false, "");
        }

        EditorGUILayout.LabelField(nameText, GUILayout.MinWidth(50.0f), GUILayout.MinHeight(25.0f));
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}