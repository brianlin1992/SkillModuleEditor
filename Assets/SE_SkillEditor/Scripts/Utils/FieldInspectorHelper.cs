#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static partial class FieldInspectorHelper {

    public static GUIStyle BoldLabel = EditorStyles.boldLabel;
    public static GUILayoutOption FieldTitleGUIOption = GUILayout.MaxWidth(128);
    public static GUILayoutOption FieldLabelGUIOption = GUILayout.Width(110);
    public static GUILayoutOption FieldLabelShrinkGUIOption = GUILayout.Width(70);
    public static GUILayoutOption IndexLabelGUIOption = GUILayout.Width(50);
    public static GUILayoutOption FieldDefaultHeightGUIOption = GUILayout.Height(28);
    public static GUILayoutOption FieldMaxWidthGUIOption = GUILayout.MaxWidth(800);
    public static GUILayoutOption FieldCurveHeightGUIOption = GUILayout.Height(23);
    public static GUILayoutOption TextfieldMinWidthGUIOption = GUILayout.MinWidth(3);

    public static GUILayoutOption RowMinWidthGUIOption = GUILayout.MinWidth(175);

    public static void StartSection()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        GUILayout.BeginVertical();
        GUILayout.Space(15);
    }
    public static void EndSection()
    {
        GUILayout.Space(15);
        GUILayout.EndVertical();
        GUILayout.Space(15);
        GUILayout.EndHorizontal();
    }
    public static void StartRow(float space = 15)
    {
        GUILayout.BeginHorizontal(RowMinWidthGUIOption, FieldMaxWidthGUIOption);
        GUILayout.Space(space);
    }
    public static void EndRow(float space = 15)
    {
        GUILayout.Space(space);
        GUILayout.EndHorizontal();
    }
    public static void StartSubSection()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        GUILayout.BeginVertical();
    }
    public static void EndSubSection()
    {
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    public static Rect AddSpace(float minWidth, float maxWidth)
    {
        return EditorGUILayout.GetControlRect(GUILayout.MinWidth(minWidth), GUILayout.MaxWidth(maxWidth));
    }
    public static void ShowTitle(string title, GUISkin skin, bool isInspectorTitle = false)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(title, skin.GetStyle("InspectorTitle"), GUILayout.ExpandWidth(true), GUILayout.Height(skin.GetStyle("InspectorTitle").fixedHeight));
        GUILayout.EndHorizontal();
    }
    public static void ShowText(string text, GUISkin skin, float? minWidth = null)
    {
        GUILayoutOption minWidthOption = minWidth == null ? TextfieldMinWidthGUIOption : GUILayout.MinWidth(minWidth.Value);
        EditorGUILayout.LabelField(text, skin.label, minWidthOption, FieldDefaultHeightGUIOption);
    }
    public static void ShowLabel(string text, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(text, skin.label, TextfieldMinWidthGUIOption, FieldDefaultHeightGUIOption);
        GUILayout.EndHorizontal();
    }
    public static string ShowTextField(string fieldName, string value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.TextField(value, skin.textField, FieldMaxWidthGUIOption, TextfieldMinWidthGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    public static void ShowFloatRangeField(string fieldName, StatRangeFloat rangeVal, GUISkin skin, string text = " to ")
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelShrinkGUIOption, FieldDefaultHeightGUIOption);

        rangeVal.min = EditorGUILayout.FloatField(rangeVal.min, skin.textField, TextfieldMinWidthGUIOption);

        EditorGUILayout.LabelField(text, skin.label);
        rangeVal.max = EditorGUILayout.FloatField(rangeVal.max, skin.textField, TextfieldMinWidthGUIOption);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
    public static float ShowFloatField(float value, GUISkin skin, float? minWidth = null)
    {
        GUILayoutOption minWidthOption = minWidth == null ? TextfieldMinWidthGUIOption : GUILayout.MinWidth(minWidth.Value);
        var output = EditorGUILayout.FloatField(value, skin.textField, FieldMaxWidthGUIOption, minWidthOption);
        return output;
    }
    public static float ShowFloatField(string fieldName, float value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.FloatField(value, skin.textField, FieldMaxWidthGUIOption, TextfieldMinWidthGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    public static int ShowIntField(int value, GUISkin skin, float? minWidth = null)
    {
        GUILayoutOption minWidthOption = minWidth == null ? TextfieldMinWidthGUIOption : GUILayout.MinWidth(minWidth.Value);
        var output = EditorGUILayout.IntField(value, skin.textField, FieldMaxWidthGUIOption, minWidthOption);
        return output;
    }
    public static int ShowIntField(string fieldName, int value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.IntField(value, skin.textField, FieldMaxWidthGUIOption, TextfieldMinWidthGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    public static Vector3 ShowVector3Field(string fieldName, Vector3 value, GUISkin skin, bool bRotation = false)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelShrinkGUIOption, FieldDefaultHeightGUIOption);
        var output = value;
        EditorGUILayout.LabelField(bRotation ? "X" : "X", skin.label);
        output.x = EditorGUILayout.FloatField(output.x, skin.textField, TextfieldMinWidthGUIOption);
        EditorGUILayout.LabelField(bRotation ? "Y" : "Y", skin.label);
        output.y = EditorGUILayout.FloatField(output.y, skin.textField, TextfieldMinWidthGUIOption);
        EditorGUILayout.LabelField(bRotation ? "Z" : "Z", skin.label);
        output.z = EditorGUILayout.FloatField(output.z, skin.textField, TextfieldMinWidthGUIOption);

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        return output;
    }
    public static bool ShowBoolField(string fieldName, bool value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.Toggle(value, skin.toggle, FieldMaxWidthGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    public static bool ShowExpandField(string fieldName, bool value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        var output = EditorGUILayout.Toggle(value, skin.GetStyle("ExpandToggle"), GUILayout.Width(skin.GetStyle("ExpandToggle").fixedWidth));
        EditorGUILayout.LabelField(fieldName, skin.label, IndexLabelGUIOption, FieldDefaultHeightGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    /** EXAMPLE FOR ENUM
     * //emitFrom = (EmitFrom) FieldInspectorHelper.ShowDropdownField("Emit From", System.Enum.GetNames(typeof(EmitFrom)), (int)emitFrom, skin);
     */
    public static int ShowDropdownField(string fieldName, string[] valueList, int value, GUISkin skin)
    {
        for (int i = 0; i < valueList.Length; i++)
        {
            valueList[i] = TransformUtility.AddSpacesToSentence(valueList[i]);
        }
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.Popup(value, valueList,skin.GetStyle("Dropdown"), FieldMaxWidthGUIOption);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        return output;
    }
    public static System.Enum ShowEnumField(string fieldName, System.Enum value, GUISkin skin)
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.EnumPopup(value, skin.GetStyle("Dropdown"), FieldMaxWidthGUIOption);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        return output;
    }
    public static T ShowObjectField<T>(string fieldName, GameObject value, GUISkin skin) where T:Object
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        T output = EditorGUILayout.ObjectField(value, typeof(T), true, FieldMaxWidthGUIOption) as T;
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        return output;
    }

    public static void ShowImageField(string fieldName, Texture2D texture, GUISkin skin, Vector2 rectSize)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(rectSize.x), GUILayout.Height(rectSize.y));
        EditorGUI.DrawPreviewTexture(rect, texture);
        GUILayout.EndHorizontal();
    }

    public static AnimationCurve ShowCurveField(string fieldName, AnimationCurve value, GUISkin skin, bool shrinkLabel = false)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, shrinkLabel ? FieldLabelShrinkGUIOption : FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.CurveField(value, FieldCurveHeightGUIOption, FieldMaxWidthGUIOption, TextfieldMinWidthGUIOption);
        GUILayout.EndHorizontal();
        return output;
    }
    public static LayerMask ShowLayerField(string fieldName, int value, GUISkin skin)
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        var output = EditorGUILayout.LayerField(value, skin.GetStyle("Dropdown"), FieldMaxWidthGUIOption);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        return output;
    }
    public static LayerMask ShowMaskField(string fieldName, int value, GUISkin skin)
    {
        List<string> layers = new List<string>();
        List<int> layerNumbers = new List<int>();

        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            if (layerName != "")
            {
                layers.Add(layerName);
                layerNumbers.Add(i);
            }
        }
        int maskWithoutEmpty = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if (((1 << layerNumbers[i]) & value) > 0)
                maskWithoutEmpty |= (1 << i);
        }


        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(fieldName, skin.label, FieldLabelGUIOption, FieldDefaultHeightGUIOption);
        maskWithoutEmpty = EditorGUILayout.MaskField(maskWithoutEmpty, layers.ToArray(), skin.GetStyle("Dropdown"), FieldMaxWidthGUIOption);
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();

        int mask = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if ((maskWithoutEmpty & (1 << i)) > 0)
                mask |= (1 << layerNumbers[i]);
        }

        return mask;
    }
}
#endif