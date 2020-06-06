using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SE_SkillPropertyView : SE_ViewBase {

    #region Public variables
    public GUISkin inspectorSkin;
    #endregion

    #region Protected Variables
    #endregion

    #region Constructors
    public SE_SkillPropertyView() : base("Property View") { }
    #endregion

    #region Main Methods
    public void UpdateInspectorGUISkin(GUISkin skin)
    {
        inspectorSkin = skin;
    }
    public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, SE_SkillControl curSkillControl, SE_SkillUIEditorSetting editorSetting)
    {
        base.UpdateView(editorRect, percentageRect, e, curSkillControl, editorSetting);
        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("PropertyViewBG"));

        GUILayout.BeginArea(viewRect);
        GUILayout.BeginHorizontal();
        if (curSkillControl != null)
        {
            bool showProperty = curSkillControl.showProperties;
            if (!showProperty)
            {
                //EditorGUILayout.LabelField("None");
            }
            else
            {
                GUILayout.BeginVertical();
                curSkillControl.DrawnNodeProperties(inspectorSkin);
                GUILayout.EndVertical();
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        ProcessEvents(e);
    }
    #endregion

    #region Utility Methods
    #endregion

}
