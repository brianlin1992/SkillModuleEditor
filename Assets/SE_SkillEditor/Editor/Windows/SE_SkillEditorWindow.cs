using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SE_SkillEditorWindow : EditorWindow
{

    #region Variables
    public static SE_SkillEditorWindow curWindow;

    public SE_SkillPropertyView propertyView;
    public SE_WorkView workView;

    public SE_SkillControl curSkillControl = null;

    public float viewPercentage = 0.75f;

    public SE_SkillEditorData editorData;
    public SE_SkillUIEditorSetting editorSetting;


    #endregion

    #region Main Methods
    public static void InitEditorWindow()
    {
        curWindow = (SE_SkillEditorWindow)EditorWindow.GetWindow<SE_SkillEditorWindow>();
        curWindow.titleContent = new GUIContent("Skill Editor");
        CreateViews();
    }
    // Use this for initialization
    void OnEnable()
    {
        Debug.Log("Enabled Window!");
    }

    void OnDestroy()
    {
        Debug.Log("Disabled Window!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTarget(SE_SkillControl target)
    {
        this.curSkillControl = target;
        RefreshEditor();
    }
    public void RefreshEditor()
    {

    }
    void OnGUI()
    {
        if (editorSetting == null)
        {
            editorSetting = (SE_SkillUIEditorSetting)Resources.Load<SE_SkillUIEditorSetting>("SkillEditorSetting/DarkKnight");
        }
        //Check for null views
        if (propertyView == null || workView == null)
        {
            CreateViews();
            return;
        }
        if(editorData == null)
        {
            editorData = (SE_SkillEditorData)Resources.Load<SE_SkillEditorData>("SkillEditorSetting/SkillEditorData");
            workView.curWindow = this;
            propertyView.curWindow = this;
        }

        //get and process the current event
        Event e = Event.current;
        ProcessEvents(e);

        //Update views
        if (curSkillControl != null)
        {
            workView.UpdateView(position, new Rect(0f, 0f, viewPercentage, 1f), e, curSkillControl, editorSetting);
            propertyView.UpdateView(new Rect(position.width, position.y, position.width, position.height)
                , new Rect(viewPercentage, 0f, 1 - viewPercentage, 1f), e, curSkillControl, editorSetting);
        }
        Repaint();
    }
    #endregion

    #region Utility Methods
    static void CreateViews()
    {
        if (curWindow != null)
        {
            curWindow.workView = new SE_WorkView();
            curWindow.workView.viewSkin = curWindow.editorSetting.ViewGUISkin;

            curWindow.propertyView = new SE_SkillPropertyView();
            curWindow.propertyView.viewSkin = curWindow.editorSetting.ViewGUISkin;
            curWindow.propertyView.inspectorSkin = curWindow.editorSetting.InspectorGUISkin;
        }
        else
        {
            curWindow = EditorWindow.GetWindow<SE_SkillEditorWindow>();
        }

    }
    void ProcessEvents(Event e)
    {
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftArrow)
        {
            viewPercentage -= 0.01f;
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.RightArrow)
        {
            viewPercentage += 0.01f;
        }
    }

    #endregion
}
