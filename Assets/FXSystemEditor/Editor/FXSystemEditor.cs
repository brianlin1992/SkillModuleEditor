using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;



public class FXSystemEditor : EditorWindow
{
    public EffectSystemControl target;
    private List<Emitter> emitters
    {
        get { return target ? target.emitters : null; }
    }

    public const float kToolbarHeight = 20f;
    public const float kToolbarButtonWidth = 50f;

    public float emitterWidth;
    public float emitterHeight;

    private GUIStyle moduleStyle;
    private GUIStyle selectedModuleStyle;
    public float moduleHeight;

    private Vector2 offset;
    private Vector2 drag;

    [MenuItem("Window/Effect System Editor")]
    private static void OpenWindow()
    {
        FXSystemEditor window = GetWindow<FXSystemEditor>();
        window.titleContent = new GUIContent("FXSystem Editor");
    }

    private void OnEnable()
    {
        emitterWidth = 194;
        emitterHeight = 49;
        moduleHeight = 35;
        int padding = 15;

        moduleStyle = new GUIStyle();
        moduleStyle.normal.background = MakeTex(2, 2, TransformUtility.hexToColor("3b3e4a"));
        moduleStyle.border = new RectOffset(12, 12, 12, 12);
        moduleStyle.padding = new RectOffset(padding, padding, padding, padding);
        moduleStyle.normal.textColor = Color.white;

        refreshStaticReference();
    }
    public void refreshStaticReference()
    {
        
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }


    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawEmitters();
        drawToolbar();
        //drawInspector();

        bool hasEvent = ProcessEmitterEvents(Event.current);
        if(!hasEvent)
            ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawEmitters()
    {
        if (emitters != null)
        {
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].Draw();
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    //ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }
    public Emitter selectedEmitter;
    public Module selectedModule;
    private void selectEmitter(Emitter emitter)
    {
        selectedEmitter = emitter;
        Debug.Log("select emitter: "+ selectedEmitter.gameObject.name);
    }
    public void selectModule(Module module)
    {
        selectedModule = module;
        Debug.Log("select module: [" + selectedModule.emitter.gameObject.name +"] "+  selectedModule.GetDisplayName());
        //switch ()
        //{
        //    default:
        //        break;
        //}
    }

    private bool ProcessEmitterEvents(Event e)
    {
        if (target != null)
        {
            bool guiChanged = false;
            for (int i = emitters.Count - 1; i >= 0; i--)
            {
                if (!guiChanged)
                    guiChanged = emitters[i].ProcessEvents(e, selectEmitter, selectModule) || guiChanged;
                else
                    emitters[i].Deselect();
            }
            if (guiChanged)
            {
                Repaint();
                return true;
            }
        }
        return false;
    }
    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (emitters != null)
        {
            for (int i = 0; i < emitters.Count; i++)
            {
                emitters[i].Drag(offset);
            }
        }
        GUI.changed = true;
    }
    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Emitter"), false, () => OnClickAddEmitter(mousePosition));
        genericMenu.ShowAsContext();
    }
    #region Inspector
    Vector2 scrollPos;
    string t = "This is a string inside a Scroll view!";
    private void drawInspector()
    {
        //EditorGUILayout
        EditorGUILayout.BeginHorizontal();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label(t);
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Add More Text", GUILayout.Width(100), GUILayout.Height(100)))
            t += " \nAnd this is more text!";
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Clear"))
            t = "";
    }
    #endregion Inspector
    #region MenuToolbar
    private void drawToolbar()
    {
        EditorGUILayout.BeginHorizontal("Toolbar");

        if (DropdownButton("File", kToolbarButtonWidth))
        {
            createFileMenu();
        }

        if (DropdownButton("Edit", kToolbarButtonWidth))
        {
            createEditMenu();
        }

        if (DropdownButton("View", kToolbarButtonWidth))
        {
            createViewMenu();
        }

        if (DropdownButton("Settings", kToolbarButtonWidth + 10f))
        {
            createSettingsMenu();
        }

        if (DropdownButton("Tools", kToolbarButtonWidth))
        {
            createToolsMenu();
        }
        if (ToolbarButton("Refresh", kToolbarButtonWidth))
        {
            RefreshEditor();
        }

        // Make the toolbar extend all throughout the window extension.
        GUILayout.FlexibleSpace();
        drawGraphName();

        EditorGUILayout.EndHorizontal();
    }

    private void drawGraphName()
    {
        string graphName = "None";

        GUILayout.Label(graphName);
    }

    private void createFileMenu()
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Create New"), false, null);// _saveManager.RequestNew);
        menu.AddItem(new GUIContent("Load"), false, null);//_saveManager.RequestLoad);

        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Save"), false, null);//_saveManager.RequestSave);
        menu.AddItem(new GUIContent("Save As"), false, null);//_saveManager.RequestSaveAs);

        menu.DropDown(new Rect(5f, kToolbarHeight, 0f, 0f));
    }

    private void createEditMenu()
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Undo"), false, null);//actions.UndoAction);
        menu.AddItem(new GUIContent("Redo"), false, null);//actions.RedoAction);

        menu.DropDown(new Rect(55f, kToolbarHeight, 0f, 0f));
    }

    private void createViewMenu()
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Home"), false, null);//editor.HomeView);
        menu.AddItem(new GUIContent("Zoom In"), false, () => { /*editor.Zoom(-1);*/ });
        menu.AddItem(new GUIContent("Zoom Out"), false, () => {/* editor.Zoom(1);*/ });

        menu.DropDown(new Rect(105f, kToolbarHeight, 0f, 0f));
    }

    private void createSettingsMenu()
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Show Guide"), false, null);// editor.bDrawGuide, editor.ToggleDrawGuide);

        menu.DropDown(new Rect(155f, kToolbarHeight, 0f, 0f));
    }

    private void createToolsMenu()
    {
        var menu = new GenericMenu();

        menu.AddItem(new GUIContent("Add Test Nodes"), false, null);//, addTestNodes);
        menu.AddItem(new GUIContent("Clear Nodes"), false, null);//, clearNodes);

        menu.DropDown(new Rect(215f, kToolbarHeight, 0f, 0f));
    }

    public bool DropdownButton(string name, float width)
    {
        return GUILayout.Button(name, EditorStyles.toolbarDropDown, GUILayout.Width(width));
    }
    public bool ToolbarButton(string name, float width)
    {
        return GUILayout.Button(name, EditorStyles.toolbarButton, GUILayout.Width(width));
    }
    #endregion

    #region EffectSystemControl 
    public void RefreshEditor()
    {
        refreshStaticReference();
        target.RefreshEmittersIndex();
        foreach (var emitter in emitters)
        {
            UpdateEmitterGUI(emitter);
        }
    }
    public void SetTarget(EffectSystemControl target)
    {
        this.target = target;
        RefreshEditor();
    }
    private void OnClickAddEmitter(Vector2 mousePosition)
    {
        if (target == null)
        {
            return;
        }
        var emitter = this.target.AddEmitter();
        UpdateEmitterGUI(emitter);
    }
    public void UpdateEmitterGUI(Emitter emitter)
    {
        emitter.SetGUIEditor(emitterWidth, emitterHeight, offset, target.emitterGUIStyle);
        emitter.UpdateModulesIndex();
        foreach (var module in emitter.modules)
        {
            module.SetGUIEditor(emitterWidth, moduleHeight, target.moduleGUIStyle);
        }
    }
    #endregion EffectSystemControl;
}