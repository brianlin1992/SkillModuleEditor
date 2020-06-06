using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public abstract partial class EM_ModuleBase : MonoBehaviour {

    //order of module inside emitter
    public int index;
    [HideInInspector]
    public SE_Emitter emitter;
    //[ReadOnly]
    public ModuleType moduleType;

    public abstract string GetDisplayName();
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
#if UNITY_EDITOR
public abstract partial class EM_ModuleBase
{
    #region Public variables
    //[ReadOnly]
    
    bool _isSelected;
    public bool isSelected {
        get { return _isSelected; }
        set {
            _isSelected = value;
            if (_isSelected == true)
            {
                emitter.isSelected = true;
            }
        }
    }
    //public string emitterName;
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
    public virtual void UpdateModule(Event e, Rect viewRect)
    {

    }
    public virtual Rect UpdateModuleGUI(Event e, Rect viewRect, Vector2 offset, GUISkin viewSkin, SE_NodeDesignData designData)
    {
        ProcessEvents(e, viewRect);
        guiSkin = viewSkin;
        nodeRect = new Rect(offset, designData.size);
        nodeRect.position += emitter.nodeRect.position;
        GUI.Box(nodeRect, GetDisplayName(), viewSkin.GetStyle(isSelected ? "ModuleSelected" : "ModuleDefault"));
        
        EditorUtility.SetDirty(this);
        return nodeRect;
    }
    public virtual void DrawnNodeProperties(GUISkin skin)
    {
        FieldInspectorHelper.ShowTitle(GetDisplayName(), skin, true);
    }
    void OnDrawGizmosSelected()
    {
        if(isSelected)
            DrawModuleHelper();
    }
    public virtual void DrawModuleHelper()
    {

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
        return null;
    }
    #endregion

}

#endif