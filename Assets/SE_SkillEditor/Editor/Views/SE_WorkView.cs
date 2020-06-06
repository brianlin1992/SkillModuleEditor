using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class SE_WorkView : SE_ViewBase
{
    #region Public variables
    #endregion

    #region Protected Variables
    Vector2 mousePos;

    private SE_Emitter clickEmitter = null;
    private EM_ModuleBase clickModule = null;

    private Vector2 offset;
    private Vector2 drag;
    #endregion

    #region Constructors
    public SE_WorkView() : base("Work View") { }
    #endregion

    #region Main Methods
    public override void UpdateView(Rect editorRect, Rect percentageRect, Event e, SE_SkillControl curSkillControl, SE_SkillUIEditorSetting editorSetting)
    {
        base.UpdateView(editorRect, percentageRect, e, curSkillControl, editorSetting);

        GUI.Box(viewRect, viewTitle, viewSkin.GetStyle("ViewBG"));


        //draw grid
        SE_SkillUtils.DrawGrid(viewRect, 60f, .15f, Color.white, offset);
        SE_SkillUtils.DrawGrid(viewRect, 20f, .05f, Color.white, offset);

        GUILayout.BeginArea(viewRect);
        if (curSkillControl != null)
        {
            curSkillControl.UpdateEditorGUI(e, viewRect, offset, viewSkin, editorSetting);
        }
        GUILayout.EndArea();

        ProcessEvents(e);
    }

    public override void ProcessEvents(Event e)
    {
        base.ProcessEvents(e);
        mousePos = e.mousePosition;

        if (viewRect.Contains(e.mousePosition))
        {
            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown)
                {

                }
                if (e.type == EventType.MouseDrag)
                {
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                }
                if (e.type == EventType.MouseUp)
                {

                }
            }
            if (e.button == 1)
            {
                if (e.type == EventType.MouseDown)
                {
                    mousePos = e.mousePosition;
                    int contextId = 0;
                    clickEmitter = null;
                    clickModule = null;

                    if (skillControl != null)
                    {

                        for (int i = 0; i < skillControl.emitters.Count; i++)
                        {
                            object mouseClickObj = skillControl.emitters[i].MouseOverComponent(mousePos);
                            if (mouseClickObj != null)
                            {
                                if (mouseClickObj is SE_Emitter)
                                {
                                    clickEmitter = mouseClickObj as SE_Emitter;
                                    contextId = 1;
                                }
                                if (mouseClickObj is EM_ModuleBase)
                                {
                                    clickModule = mouseClickObj as EM_ModuleBase;
                                    contextId = 2;
                                }
                            }
                        }
                    }
                    ProcessContextMenu(e, contextId);
                }
            }
        }
    }
    #endregion

    #region Utility Methods
    void ProcessContextMenu(Event e, int contextID)
    {
        GenericMenu menu = new GenericMenu();
        if (contextID == 0)
        {
            menu.AddItem(new GUIContent("Add Mew Emitter"), false, ContextCallBack, "0");
        }
        if (contextID == 1)
        {
            menu.AddItem(new GUIContent("Emitter/Delete Emitter"), false, ContextCallBack, "1");
            menu.AddSeparator("");
            CreateAddModuleMenu(menu);
        }
        if (contextID == 2)
        {
            menu.AddItem(new GUIContent("Delete Module"), false, ContextCallBack, "2");
            menu.AddItem(new GUIContent("Move Up"), false, ContextCallBack, "3");
            menu.AddItem(new GUIContent("Move Down"), false, ContextCallBack, "4");
        }
        menu.ShowAsContext();
        e.Use();
    }
    void CreateAddModuleMenu(GenericMenu menu)
    {
        for (int i = 0; i < curWindow.editorData.moduleMenuList.Count; i++)
        {
            var moduleMenu = curWindow.editorData.moduleMenuList[i];
            menu.AddItem(new GUIContent(moduleMenu.subType.ToString()+"/"+ moduleMenu.moduleType.ToString()), false, ()=> {
                clickEmitter.AddModuleByModuleType(moduleMenu.moduleType);
            });
        }
    }
    void ContextCallBack(object obj)
    {
        switch (obj.ToString())
        {
            case "0": skillControl.AddEmitter(); break;
            case "1": skillControl.DeleteEmitter(clickEmitter); break;
            case "2": clickModule.emitter.RemoveModule(clickModule); break;
            case "3": clickModule.emitter.MoveModuleUp(clickModule); break;
            case "4": clickModule.emitter.MoveModuleDown(clickModule); break;
            default:
                break;
        }
    }
    private void OnDrag(Vector2 delta)
    {
        //drag = delta;
        //offset += drag;
        //GUI.changed = true;
    }
    #endregion

}
