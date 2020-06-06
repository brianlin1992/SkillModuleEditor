using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public partial class SE_SkillControl : MonoBehaviour
{

    #region Public variables
    public string skillName;
    public List<SE_Emitter> emitters = new List<SE_Emitter>();
    public List<Transform> targets = new List<Transform>();
    #endregion

    #region Protected Variables
    #endregion

    #region Constructors
    #endregion

    #region Main Methods
    public SE_Emitter AddEmitter()
    {
        var go = new GameObject("Emitter " + emitters.Count);
        go.transform.parent = transform;
        if (go.transform.parent != transform)
        {
            Debug.LogError("Cannot create new emitter inside prefab");
            DestroyImmediate(go);
            return null;
        }
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = new Quaternion();
        var emitter = go.AddComponent<SE_Emitter>();
        emitter.index = emitters.Count;
        emitters.Add(emitter);
        emitter.skillControl = this;
        emitter.AddModule<EM_Spawn>().index = 0;
        emitter.AddModule<EM_Lifetime>().index = 1;
        emitter.AddModule<EM_InitialVelocity>().index = 2;
        return emitter;
    }
    public void DeleteEmitter(SE_Emitter emitter)
    {
        emitter.skillControl = null;
        emitters.Remove(emitter);
        DestroyImmediate(emitter.gameObject);
        RefreshEmittersIndex();
    }
    public void RefreshEmittersIndex()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].index = i;
        }
    }
    public void RestartAllEmitters()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].ResetEmitter();
        }
    }
    public void StopAllEmitters()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].StopEmitter();
        }
    }
    #endregion

    #region Utility Methods

    void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += StateChange;
#endif
    }
#if UNITY_EDITOR
    private void StateChange(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.ExitingEditMode)
        {
            RestartAllEmitters();
        }
    }
#endif
#if UNITY_EDITOR

#endif
    #endregion

}
#if UNITY_EDITOR
public partial class SE_SkillControl
{
    #region Public variables
    [Header("Editor Params")]
    public SE_Emitter selectedEmitter;
    public EM_ModuleBase selectedModule;
    public bool showProperties = false;
    public bool previewSkill;
    #endregion

    #region Protected Variables
    #endregion

    #region Constructors
    #endregion

    #region Main Methods
    public void UpdateEditorGUI(Event e, Rect viewRect, Vector2 offset, GUISkin viewSkin, SE_SkillUIEditorSetting editorSetting)
    {
        if (emitters.Count > 0)
        {
            ProcessEvents(e, viewRect);

            var emitterDesignData = editorSetting.emitterDesignSetting;
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].UpdateEmitterGUI(e, viewRect, offset, viewSkin, emitterDesignData, (emitter, modules) => {
                    Vector2 moduleOffset = Vector2.zero;
                    moduleOffset.y = emitter.nodeRect.size.y;
                    for (int j = 0; j < modules.Count; j++)
                    {
                        var moduleDesignData = editorSetting.modulesDesignSetting.Find(m => m.moduleType == modules[j].moduleType).designData;
                        moduleOffset.y += modules[j].UpdateModuleGUI(e, viewRect, moduleOffset, viewSkin, moduleDesignData).size.y;
                    }
                });
            }
        }

        if (e.type == EventType.Layout)
        {
            if (selectedEmitter != null || selectedModule != null)
            {
                showProperties = true;
            }
        }

        //make sure to save all changes to scriptable object when updating in editor window
        EditorUtility.SetDirty(this);
    }
    #endregion

    #region Utility Methods
    void ProcessEvents(Event e, Rect viewRect)
    {
        if (viewRect.Contains(e.mousePosition))
        {
            if (e.button == 0)
            {
                if (e.type == EventType.MouseDown)
                {
                    DeselectAllNodes();

                    bool setNode = false;

                    selectedEmitter = null;
                    selectedModule = null;

                    showProperties = false;

                    for (int i = 0; i < emitters.Count; i++)
                    {
                        object mouseClickObj = emitters[i].MouseOverComponent(e.mousePosition);
                        if (mouseClickObj != null)
                        {
                            GUI.FocusControl(null);
                            if (mouseClickObj is SE_Emitter)
                            {
                                selectedEmitter = emitters[i];
                                selectedEmitter.isSelected = true;
                                setNode = true;
                            }
                            if (mouseClickObj is EM_ModuleBase)
                            {
                                selectedModule = mouseClickObj as EM_ModuleBase;
                                selectedModule.isSelected = true;
                                setNode = true;
                            }
                        }
    
                    }
                    if (!setNode)
                    {
                        DeselectAllNodes();
                    }
                }
            }
        }
    }
    void DeselectAllNodes()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].isSelected = false;
        }
    }
    public void DrawnNodeProperties(GUISkin skin)
    {
        if(selectedModule != null)
        {
            selectedModule.DrawnNodeProperties(skin);
            return;
        }
        if (selectedEmitter != null)
        {
            selectedEmitter.DrawnNodeProperties(skin);
            return;
        }
    }
    #endregion


}
#endif