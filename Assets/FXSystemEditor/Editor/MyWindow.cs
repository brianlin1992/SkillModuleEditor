using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

public class MyWindow : EditorWindow
{
    AnimBool m_ShowExtraFields;
    string m_String;
    Color m_Color = Color.white;
    int m_Number = 0;

    [MenuItem("Window/My Window")]
    static void Init()
    {
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
    }

    void OnEnable()
    {
        m_ShowExtraFields = new AnimBool(true);
        m_ShowExtraFields.valueChanged.AddListener(Repaint);
    }

    void OnGUI()
    {
        m_ShowExtraFields.target = EditorGUILayout.ToggleLeft("Show extra fields", m_ShowExtraFields.target);

        //Extra block that can be toggled on and off.
        if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFields.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PrefixLabel("Color");
            m_Color = EditorGUILayout.ColorField(m_Color);
            EditorGUILayout.PrefixLabel("Text");
            m_String = EditorGUILayout.TextField(m_String);
            EditorGUILayout.PrefixLabel("Number");
            m_Number = EditorGUILayout.IntSlider(m_Number, 0, 10);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndFadeGroup();
    }
}