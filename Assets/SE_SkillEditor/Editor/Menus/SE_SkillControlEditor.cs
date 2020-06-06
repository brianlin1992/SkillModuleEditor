using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SE_SkillControl))]
public class SE_SkillControlEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SE_SkillControl myScript = (SE_SkillControl)target;
        if (GUILayout.Button("Open Editor"))
        {
            SE_SkillEditorWindow window = EditorWindow.GetWindow(typeof(SE_SkillEditorWindow), false) as SE_SkillEditorWindow;
            window.SetTarget(myScript);
        }
        if (GUILayout.Button("Restart All Emitters"))
        {
            myScript.RestartAllEmitters();
        }

        var centeredStyle = new GUIStyle(EditorStyles.boldLabel);
        centeredStyle.alignment = TextAnchor.UpperCenter;
        GUILayout.Label("Save Load", centeredStyle);
        if (GUILayout.Button("Save To Asset File"))
        {
            SE_SkillUtils.SaveSkillControlToScriptableObject(myScript);
        }
        if (GUILayout.Button("Load Asset File"))
        {
            SE_SkillUtils.LoadSkillControlFromScriptableObject(myScript);
        }
    }
}
