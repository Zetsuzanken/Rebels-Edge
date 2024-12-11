using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


public class SimpleEditorWindow : EditorWindow
{
    private string stringValue = "Hello, Unity!";
    private Color colorValue = Color.white;
    private float floatValue = 0.5f;

    // Add menu item to the Window menu
    [MenuItem("Tools/Simple Editor Window")]
    public static void ShowWindow()
    {
        // Show existing window instance. If none, create a new one.
        GetWindow<SimpleEditorWindow>("Simple Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Editor Window", EditorStyles.boldLabel);

        // Text Field
        stringValue = EditorGUILayout.TextField("Text Value", stringValue);

        // Color Picker
        colorValue = EditorGUILayout.ColorField("Color Value", colorValue);

        // Slider
        floatValue = EditorGUILayout.Slider("Float Value", floatValue, 0, 1);

        // Add a button
        if (GUILayout.Button("Print Values to Console"))
        {
            Debug.Log($"String: {stringValue}, Color: {colorValue}, Float: {floatValue}");
        }

        // Add spacing
        GUILayout.Space(10);

        // Button to reset values
        if (GUILayout.Button("Reset Values"))
        {
            stringValue = "Hello, Unity!";
            colorValue = Color.white;
            floatValue = 0.5f;
        }
    }
}
