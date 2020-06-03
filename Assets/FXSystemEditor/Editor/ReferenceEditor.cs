using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReferenceEditor : EditorWindow
{

    Rect windowRect = new Rect(100 + 100, 100, 100, 100);
    Rect windowRect2 = new Rect(100, 100, 100, 100);


    [MenuItem("Window/ref Window")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(ReferenceEditor));
    }

    private void OnGUI()
    {
        Handles.BeginGUI();
        Handles.DrawBezier(windowRect.center, windowRect2.center, new Vector2(windowRect.xMax + 50f, windowRect.center.y), new Vector2(windowRect2.xMin - 50f, windowRect2.center.y), Color.red, null, 5f);
        Handles.EndGUI();

        BeginWindows();
        windowRect = GUI.Window(0, windowRect, WindowFunction, "Box1");
        windowRect2 = GUI.Window(1, windowRect2, WindowFunction, "Box2");

        EndWindows();



    }
    void WindowFunction(int windowID)
    {
        GUI.DragWindow();
    }
}
public class ExampleWindow : EditorWindow
{

    Color color;

    [MenuItem("Window/Colorizer")]
    public static void ShowWindow()
    {
        GetWindow<ExampleWindow>("Colorizer");
    }
    public int selGridInt = 0;
    public string[] selStrings = new string[] { "Grid 1", "Grid 2", "Grid 3", "Grid 4" };
    void OnGUI()
    {
        selGridInt = GUI.SelectionGrid(new Rect(25, 25, 100, 30), selGridInt, selStrings, 2);
    }
    //public int toolbarInt = 0;
    //public string[] toolbarStrings = new string[] { "Toolbar1", "Toolbar2", "Toolbar3" };
    //void OnGUI()
    //{
    //    toolbarInt = GUI.Toolbar(new Rect(25, 25, 250, 30), toolbarInt, toolbarStrings);
        //GUILayout.Label("Color the selected objects!", EditorStyles.boldLabel);

        //color = EditorGUILayout.ColorField("Color", color);

        //if (GUILayout.Button("COLORIZE!"))
        //{
        //    Colorize();
        //}

        //GUI.BeginGroup(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 300, 800, 600));
        //GUI.Box(new Rect(0, 0, 800, 600), "This box is now centered! - here you would put your main menu");
        //GUI.EndGroup();
    //}

    void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial.color = color;
            }
        }
    }

}