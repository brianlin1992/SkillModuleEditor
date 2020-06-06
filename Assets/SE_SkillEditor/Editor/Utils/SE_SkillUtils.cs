using UnityEngine;
using UnityEditor;

public static class SE_SkillUtils {

    public static void DrawGrid(Rect viewRect, float gridSpacing, float gridOpacity, Color gridColor, Vector2 offset)
    {
        int widthDivs = Mathf.CeilToInt(viewRect.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(viewRect.height / gridSpacing);

        Vector3 newOffsetX = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);
        newOffsetX.x = 0;
        Vector3 newOffsetY = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);
        newOffsetY.y = 0;
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        for (int x = 0; x < widthDivs; x++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * x, 0f, 0f) + newOffsetY, new Vector3(gridSpacing * x, viewRect.height, 0f) + newOffsetY);
        }
        for (int y = 0; y < heightDivs; y++)
        {
            Handles.DrawLine(new Vector3(0f, gridSpacing * y, 0f) + newOffsetX, new Vector3(viewRect.width, gridSpacing * y, 0f) + newOffsetX);
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    public static void SaveSkillControlToScriptableObject(SE_SkillControl skillControl)
    {
        ScriptableObjectUtility.CreateAsset<SE_SaveObject>(
            "Save Skill Control To Asset File", 
            skillControl.gameObject.name,
            (obj)=> {
                obj.skillControlName = skillControl.gameObject.name;
        });
    }
    public static void LoadSkillControlFromScriptableObject(SE_SkillControl skillControl)
    {
        string path = EditorUtility.OpenFilePanel("Overwrite Skill Control From Asset File", "", "asset");
        path = path.Replace(Application.dataPath, "Assets");
        SE_SaveObject loadObj = AssetDatabase.LoadAssetAtPath<SE_SaveObject>(path);
        skillControl.gameObject.name = loadObj.skillControlName;
    }
}
