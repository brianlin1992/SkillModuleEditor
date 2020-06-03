using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class Module : MonoBehaviour {

    public Emitter emitter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public abstract partial class Module
{

    [Header("Editor Params")]
    public Rect rectOrigin;
    public Rect rect;
    public int index;

    public string title = "fx emitter";
    public bool isSelected;

    public GUIStyleData styleData;

    public abstract string GetDisplayName();

    public void SetGUIEditor(float width, float height, GUIStyleData styleData)
    {
        rectOrigin = new Rect(0, height * index, width, height);
        this.styleData = styleData;
    }

    public void Draw()
    {
        rect = rectOrigin;
        rect.position += emitter.rect.position;
        rect.position += new Vector2(0, emitter.rect.size.y);
        GUIContent content = new GUIContent(GetDisplayName());
        GUI.Box(rect, content, isSelected ? styleData.selectedStyle : styleData.style);
    }
    public bool ProcessEvents(Event e)
    {

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    isSelected = rect.Contains(e.mousePosition);
                    return isSelected;
                }
                break;
        }

        return false;
    }
}
