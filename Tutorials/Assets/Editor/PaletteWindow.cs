using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PaletteWindow : EditorWindow
{
    public static PaletteWindow instance;

    public static void ShowPalette()
    {
        instance = (PaletteWindow)EditorWindow.GetWindow(typeof(PaletteWindow));
        instance.titleContent = new GUIContent("Palette");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable called.");
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable called.");
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy called.");
    }

    private void OnGUI()
    {
        Debug.Log("OnGUI called.");
    }

    private void Update()
    {
        
    }
}
