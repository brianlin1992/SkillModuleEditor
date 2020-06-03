using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ModuleInspector<T> : EditorWindow where T:Module
{
    public static T Target;
    public static ModuleInspector<T> window;
    private static GameObject obj;

    public static void OpenWindow(T target)
    {
        Target = target;
        window = (ModuleInspector<T>)EditorWindow.GetWindow(typeof(ModuleInspector<T>)); //create a window
        window.titleContent = new GUIContent(window.GetWindowName()); //set a window title
    }

    public abstract string GetWindowName();
    public abstract void DisplayInspector();

    void OnGUI()
    {
        if (Target == null)
            return;
        DisplayInspector();
    }

    void Update()
    {
        Repaint();
    }
}