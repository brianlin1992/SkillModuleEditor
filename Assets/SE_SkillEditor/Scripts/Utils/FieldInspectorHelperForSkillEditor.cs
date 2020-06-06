#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

public static partial class FieldInspectorHelper {

    public static GUILayoutOption FlexibleOptionHeightGUIOption = GUILayout.Height(23);

    public static void ShowFlexibleFloatField(string fieldName, FlexibleFloat value, GUISkin skin)
    {
        GUILayout.BeginHorizontal();
        switch (value.type)
        {
            case FlexibleEditType.Uniform:
                value.SetValue(ShowFloatField(fieldName, value.uniformValue, skin));
                break;
            case FlexibleEditType.RangeTween:
                ShowFloatRangeField(fieldName, value.rangeValue, skin);
                break;
            case FlexibleEditType.RangeRandom:
                ShowFloatRangeField(fieldName, value.rangeValue, skin, " rand ");
                break;
            case FlexibleEditType.Curve:
                value.curveValue = ShowCurveField(fieldName, value.curveValue, skin);
                break;
            default:
                break;
        }
        if (EditorGUILayout.DropdownButton(new GUIContent(), FocusType.Passive, skin.GetStyle("minipulldown")))
        {
            GenericMenu menu = new GenericMenu();
            if (value.type != FlexibleEditType.Uniform)
                menu.AddItem(new GUIContent("Float Constant"), false, () => { value.type = FlexibleEditType.Uniform; });
            else
                menu.AddDisabledItem(new GUIContent("Float Constant"));

            if (value.type != FlexibleEditType.RangeTween)
                menu.AddItem(new GUIContent("Float Linear Tween"), false, () => { value.type = FlexibleEditType.RangeTween; });
            else
                menu.AddDisabledItem(new GUIContent("Float Linear Tween"));

            if (value.type != FlexibleEditType.RangeRandom)
                menu.AddItem(new GUIContent("Float Random"), false, () => { value.type = FlexibleEditType.RangeRandom; });
            else
                menu.AddDisabledItem(new GUIContent("Float Random"));

            if (value.type != FlexibleEditType.Curve)
                menu.AddItem(new GUIContent("Float Curve"), false, () => { value.type = FlexibleEditType.Curve; });
            else
                menu.AddDisabledItem(new GUIContent("Float Curve"));
            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();

    }
    public static void ShowFlexibleVector3Field(string fieldName, FlexibleVector3 value, GUISkin skin, bool bRotation = false)
    {
        GUILayout.BeginHorizontal();
        switch (value.type)
        {
            case FlexibleEditType.Uniform:
                value.SetValue(ShowVector3Field(fieldName, value.uniformValue, skin, bRotation));
                break;
            case FlexibleEditType.RangeTween:
                GUILayout.BeginVertical();
                ShowLabel(fieldName, skin);
                FieldInspectorHelper.StartSubSection();
                ShowFloatRangeField(bRotation ? "Pitch(X)" : "X", value.rangeX, skin);
                ShowFloatRangeField(bRotation ? "Yaw (Y)" : "Y", value.rangeY, skin);
                ShowFloatRangeField(bRotation ? "Roll (Z)" : "Z", value.rangeZ, skin);
                FieldInspectorHelper.EndSubSection();
                GUILayout.EndVertical();
                break;
            case FlexibleEditType.RangeRandom:
                GUILayout.BeginVertical();
                ShowLabel(fieldName, skin);
                FieldInspectorHelper.StartSubSection();
                ShowFloatRangeField(bRotation ? "Pitch(X)" : "X", value.rangeX, skin, " rand ");
                ShowFloatRangeField(bRotation ? "Yaw (Y)" : "Y", value.rangeY, skin, " rand ");
                ShowFloatRangeField(bRotation ? "Roll (Z)" : "Z", value.rangeZ, skin, " rand ");
                FieldInspectorHelper.EndSubSection();
                GUILayout.EndVertical();
                break;
            case FlexibleEditType.Curve:
                GUILayout.BeginVertical();
                ShowLabel(fieldName, skin);
                FieldInspectorHelper.StartSubSection();
                value.curveX = ShowCurveField(bRotation ? "Pitch(X)" : "X", value.curveX, skin, true);
                value.curveY = ShowCurveField(bRotation ? "Yaw (Y)" : "Y", value.curveY, skin, true);
                value.curveZ = ShowCurveField(bRotation ? "Roll (Z)" : "Z", value.curveZ, skin, true);
                FieldInspectorHelper.EndSubSection();
                GUILayout.EndVertical();
                break;
            default:
                break;
        }
        if (EditorGUILayout.DropdownButton(new GUIContent(), FocusType.Passive, skin.GetStyle("minipulldown")))
        {
            GenericMenu menu = new GenericMenu();
            if (value.type != FlexibleEditType.Uniform)
                menu.AddItem(new GUIContent("Vector3 Constant"), false, () => { value.type = FlexibleEditType.Uniform; });
            else
                menu.AddDisabledItem(new GUIContent("Vector3 Constant"));

            if (value.type != FlexibleEditType.RangeTween)
                menu.AddItem(new GUIContent("Vector3 Linear Tween"), false, () => { value.type = FlexibleEditType.RangeTween; });
            else
                menu.AddDisabledItem(new GUIContent("Vector3 Linear Tween"));

            if (value.type != FlexibleEditType.RangeRandom)
                menu.AddItem(new GUIContent("Vector3 Random"), false, () => { value.type = FlexibleEditType.RangeRandom; });
            else
                menu.AddDisabledItem(new GUIContent("Vector3 Random"));

            if (value.type != FlexibleEditType.Curve)
                menu.AddItem(new GUIContent("Vector3 Curve"), false, () => { value.type = FlexibleEditType.Curve; });
            else
                menu.AddDisabledItem(new GUIContent("Vector3 Curve"));
            menu.ShowAsContext();
        }
        GUILayout.EndHorizontal();

    }
    
    public static void ShowAddSubButton(bool showAdd, bool showSub, GUISkin skin, Action onAddClick, Action onSubClick)
    {
        GUIStyle addStyle = skin.GetStyle("addbtn");
        GUIStyle subStyle = skin.GetStyle("subbtn");
        GUILayout.BeginVertical(GUILayout.Width(addStyle.fixedWidth), FieldDefaultHeightGUIOption);
        if(showAdd)
        if (EditorGUILayout.DropdownButton(new GUIContent(), FocusType.Passive, addStyle))
        {
            onAddClick();
        }
        if (showSub)
        if (EditorGUILayout.DropdownButton(new GUIContent(), FocusType.Passive, subStyle))
        {
            onSubClick();
        }
        GUILayout.EndVertical();
    }
}
#endif