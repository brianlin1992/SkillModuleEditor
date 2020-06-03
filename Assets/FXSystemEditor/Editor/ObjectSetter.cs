using UnityEditor;
using UnityEngine;
using System.Collections;

public class ObjectSetter : EditorWindow
{

    public static ObjectSetter window;
    private static GameObject obj;

    [MenuItem("Tools/ObjectSetter")]
    public static void OpenWindow()
    {
        window = (ObjectSetter)EditorWindow.GetWindow(typeof(ObjectSetter)); //create a window
        window.titleContent = new GUIContent( "Object Setter"); //set a window title
    }

    void OnGUI()
    {
        if (window == null)
            OpenWindow();

        if (Selection.activeGameObject != null)
        {
            //gets the object you currently have selected in the scene view
            obj = Selection.activeGameObject;
            GUI.Label(new Rect(5, 5, position.width - 10, 25), "Current selected object: " + obj.name);

            //make sure to only show the interface
            DataHolder comp = obj.GetComponent<DataHolder>();
            if (comp != null)
            {
                comp.health = EditorGUI.IntField(
                    new Rect(5, 30, position.width - 10, 16),
                    "Health",
                    comp.health
                );
                comp.username = EditorGUI.TextField(
                    new Rect(5, 50, position.width - 10, 16),
                    "User Name",
                    comp.username
                );
                //comp.cam = (GameObject)EditorGUI.ObjectField(
                //    new Rect(5, 70, position.width - 10, 16),
                //    "Camera GameObject",
                //    comp.cam,
                //    typeof(GameObject)
                //);
            }
            else
            {
                if (GUI.Button(new Rect(5, 30, position.width - 10, position.height - 40),
                "Add DataHolder"))
                {
                    obj.AddComponent<DataHolder>();
                }
            }
        }
    }

    void Update()
    {
        Repaint();
    }
}