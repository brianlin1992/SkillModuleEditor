using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

[Serializable]
public class SE_ViewBase {

    #region Public variables
    public string viewTitle;
    public Rect viewRect;
    public GUISkin viewSkin;
    public SE_SkillEditorWindow curWindow;
    #endregion

    #region Protected Variables
    protected SE_SkillControl skillControl;
    #endregion

    #region Constructors
    public SE_ViewBase(string title)
    {
        viewTitle = title;
    }
    #endregion

    #region Main Methods
    public virtual void UpdateView(Rect editorRect, Rect percentageRect, Event e, SE_SkillControl curSkillControl, SE_SkillUIEditorSetting editorSetting)
    {

        this.skillControl = curSkillControl;
        viewTitle = curSkillControl != null ? curSkillControl.skillName : "No Graph";

        //Update View Rectangle
        viewRect = new Rect(editorRect.x * percentageRect.x,
            editorRect.y * percentageRect.y,
            editorRect.width * percentageRect.width,
            editorRect.height * percentageRect.height);

    }
    public virtual void ProcessEvents(Event e)
    {

    }
    #endregion

    #region Utility Methods
    #endregion
}
