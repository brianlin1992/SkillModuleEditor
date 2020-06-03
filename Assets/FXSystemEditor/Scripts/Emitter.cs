using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class Emitter : MonoBehaviour
{
    public EffectSystemControl emitterControl;
    public List<Module> modules = new List<Module>();

    public GameObject Template;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public T AddModule<T>() where T:Module
    {
        var module = gameObject.AddComponent<T>();
        module.emitter = this;
        modules.Add(module);
        module.index = modules.IndexOf(module);
        return module;
    }
    public void UpdateModulesIndex()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].index = i;
        }
    }
}

public partial class Emitter {

    [Header("Editor Params")]
    public Rect rectOrigin;
    public Rect rect;
    public int index;

    Vector2 posOffset;
    static Vector2 hozMargin = new Vector3(5, 0);

    public string title = "fx emitter";
    public bool isSelected;

    public GUIStyleData styleData;

    public void  SetGUIEditor(float width, float height, Vector2 offset, GUIStyleData styleData)
    {
        rectOrigin = new Rect(0, 0, width, height);
        this.styleData = styleData;
        this.posOffset = offset;
    }

    public void Drag(Vector2 delta)
    {
        this.posOffset = delta;
    }
    public void Deselect()
    {
        isSelected = false;
        foreach (var module in modules)
        {
            module.isSelected = false;
        }
    }
    public void Draw()
    {
        rect = rectOrigin;
        Vector2 pos = rectOrigin.position + posOffset + index * hozMargin;
        pos.x += rectOrigin.size.x * index;
        rect.position = pos;
        GUIContent content = new GUIContent(gameObject.name);
        GUI.Box(rect, content, isSelected ? styleData.selectedStyle : styleData.style);
        foreach (var module in modules)
        {
            module.Draw();
        }
    }
    public bool ProcessEvents(Event e, Action<Emitter> emitterClickAction, Action<Module> moduleClickAction)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    isSelected = rect.Contains(e.mousePosition);
                    if(isSelected)
                    {
                        emitterClickAction(this);
                        foreach (var module in modules)
                        {
                            module.isSelected = false;
                        }
                    }
                    else
                    {
                        foreach (var module in modules)
                        {
                            bool moduleIsSelected = module.ProcessEvents(e);
                            isSelected = moduleIsSelected || isSelected;
                            if (moduleIsSelected)
                                moduleClickAction(module);
                        }
                    }                 
                    return isSelected;
                }

                if (e.button == 1 && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Delete Emitter"), false, () => { emitterControl.DeleteEmitter(this); });
        genericMenu.AddSeparator("");
        genericMenu.AddItem(new GUIContent("Add Module"), false, ()=> { this.AddModule<EM_Cone>(); });
        
        genericMenu.ShowAsContext();
    }
    //menu.AddSeparator("SubMenu/");
}
