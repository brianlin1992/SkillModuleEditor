using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public partial class SE_Emitter : MonoBehaviour {

    //order of emitter inside skillControl
    [HideInInspector]
    public int index;
    [HideInInspector]
    public SE_SkillControl skillControl;
    public List<EM_ModuleBase> modules = new List<EM_ModuleBase>();

    public T AddModule<T>() where T : EM_ModuleBase
    {
        var module = gameObject.AddComponent<T>();
        module.emitter = this;
        modules.Add(module);
        module.index = modules.IndexOf(module);
        return module;
    }
    public void RemoveModule(EM_ModuleBase module)
    {
        module.emitter = null;
        modules.Remove(module);
        DestroyImmediate(module);
        UpdateModulesIndex();
    }
    public void MoveModuleUp(EM_ModuleBase module)
    {
        int moduleIndex = modules.IndexOf(module);
        if(moduleIndex > 1)
        {
            EM_ModuleBase switchModule = modules[moduleIndex - 1];
            modules[moduleIndex - 1] = module;
            modules[moduleIndex] = switchModule;
        }
    }
    public void MoveModuleDown(EM_ModuleBase module)
    {
        int moduleIndex = modules.IndexOf(module);
        if (moduleIndex < modules.Count - 1)
        {
            EM_ModuleBase switchModule = modules[moduleIndex + 1];
            modules[moduleIndex + 1] = module;
            modules[moduleIndex] = switchModule;
        }
    }
    public void UpdateModulesIndex()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].index = i;
        }
    }
    public EM_ModuleBase AddModuleByModuleType(ModuleType type)
    {
        switch (type)
        {
            case ModuleType.Spawn:
                Debug.LogError("Spawn module cannot be add manually");
                break;
            case ModuleType.Lifetime:
                return AddModule<EM_Lifetime>();
            case ModuleType.InitialSize:
                return AddModule<EM_InitialSize>();
            case ModuleType.InitialVelocity:
                return AddModule<EM_InitialVelocity>();
            case ModuleType.VelocityScaleOverLife:
                return AddModule<EM_VelocityScaleOverLife>();
            case ModuleType.VelocityNoise:
                return AddModule<EM_VelocityNoise>();
            case ModuleType.Cone:
                return AddModule<EM_Cone>();
            case ModuleType.Circle:
                return AddModule<EM_Circle>();
            case ModuleType.Line:
                return AddModule<EM_Line>();
            case ModuleType.InitialRotation:
                return AddModule<EM_InitialRotation>();
            case ModuleType.InitialRotRate:
                return AddModule<EM_InitialRotRate>();
            case ModuleType.InitialAnchorRotRate:
                return AddModule<EM_InitialAnchorRotRate>();
            case ModuleType.Homing:
                return AddModule<EM_Homing>();
            case ModuleType.AlignToVelocity:
                return AddModule<EM_AlignToVelocity>();
            case ModuleType.SelectTarget:
                return AddModule<EM_SelectTarget>();
            case ModuleType.OnCollision:
                return AddModule<EM_OnCollision>();
            case ModuleType.OnDestroy:
                return AddModule<EM_OnDestroy>();
            case ModuleType.InitialAnchorOffset:
                return AddModule<EM_InitialAnchorOffset>();
            case ModuleType.AnchorOffsetOverLife:
                return AddModule<EM_AnchorOffsetOverLife>();
            default:
                break;
        }
        return null;
    }
}
#if UNITY_EDITOR
public partial class SE_Emitter
{
    #region Public variables
    
    bool _isSelected;
    public bool isSelected
    {
        get { return _isSelected; }
        set
        {
            _isSelected = value;
            if (_isSelected == false)
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    modules[i].isSelected = false;
                }
            }
        }
    }
    //public string emitterName;
    [HideInInspector]
    public Vector2 nodePos;
    [HideInInspector]
    public Rect nodeRect;
    #endregion

    #region Protected Variables
    protected GUISkin guiSkin;
    #endregion

    #region Main Methods
    public virtual void InitNode()
    {

    }
    public virtual void UpdateEmitter(Event e, Rect viewRect)
    {

    }
    public virtual Rect UpdateEmitterGUI(Event e, Rect viewRect, Vector2 offset, GUISkin viewSkin, SE_NodeDesignData designData, Action<SE_Emitter ,List<EM_ModuleBase>> UpdateModulesGUI)
    {
        ProcessEvents(e, viewRect);
        nodeRect = new Rect(nodePos + offset, designData.size);
        nodeRect.position += index * designData.gap;
        nodeRect.position += new Vector2(designData.size.x * index, 0);
        GUI.Box(nodeRect, gameObject.name, viewSkin.GetStyle(isSelected ? "NodeSelected" : "NodeDefault"));

        EditorUtility.SetDirty(this);

        if (UpdateModulesGUI != null)
            UpdateModulesGUI(this, modules);

        return nodeRect;
    }
    public virtual void DrawnNodeProperties(GUISkin skin)
    {
        FieldInspectorHelper.ShowTitle(gameObject.name, skin, true);
        FieldInspectorHelper.StartSection();
        template = FieldInspectorHelper.ShowObjectField<GameObject>("Template", template, skin);
        duration = FieldInspectorHelper.ShowFloatField("Duration", duration, skin);
        looping = FieldInspectorHelper.ShowBoolField("Looping", looping, skin);
        startDelay = FieldInspectorHelper.ShowFloatField("Start Delay", startDelay, skin);
        simulationSpace = (SpaceType)FieldInspectorHelper.ShowEnumField("Simulation Space", simulationSpace, skin);
        FieldInspectorHelper.EndSection();
    }
    #endregion

    #region Utility Methods
    public bool RectContains(Vector2 mousePosition)
    {
        return nodeRect.Contains(mousePosition);
    }
    void ProcessEvents(Event e, Rect viewRect)
    {
        if (!isSelected)
            return;

        if (RectContains(e.mousePosition))
        {
            if (e.type == EventType.MouseDrag)
            {
                this.nodeRect.x += e.delta.x;
                this.nodeRect.y += e.delta.y;
            }
        }
    }
    public object MouseOverComponent(Vector2 mousePosition)
    {
        if (RectContains(mousePosition))
            return this;
        else
            for (int i = 0; i < modules.Count; i++)
            {
                EM_ModuleBase module = modules[i];
                if (module.RectContains(mousePosition))
                    return module;
            }
        return null;
    }
    #endregion

}
#endif