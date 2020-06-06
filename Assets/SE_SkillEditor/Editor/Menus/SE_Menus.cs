using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SE_Menus {

    [MenuItem("Skill Editor/Launch Editor")]
    public static void InitNodeEditor()
    {
        SE_SkillEditorWindow.InitEditorWindow();
    }
}
