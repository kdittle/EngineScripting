using UnityEngine;
using UnityEditor;

public static class MenuItems
{
    [MenuItem("Tools/Show Palette")]
    private static void ShowPallete()
    {
        PaletteWindow.ShowPalette();
    }
}