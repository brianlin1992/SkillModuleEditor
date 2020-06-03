using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectSystemControl))]
public class EffectSystemControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EffectSystemControl myScript = (EffectSystemControl)target;
        if (GUILayout.Button("Open Editor"))
        {
            //myScript.OpenEditor();
            FXSystemEditor window = EditorWindow.GetWindow(typeof(FXSystemEditor), false) as FXSystemEditor;
            window.SetTarget(myScript);
        }
    }
}
