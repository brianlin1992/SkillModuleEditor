using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class EM_InitialAnchorOffset : EM_ModuleBase
{
    public Vector3 initAnchorOffset
    {
        get { return fl_initAnchorOffset.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_initAnchorOffset.SetValue(value); }
    }
    public FlexibleVector3 fl_initAnchorOffset = new FlexibleVector3(Vector3.zero);
    void Awake()
    {
        moduleType = ModuleType.InitialAnchorOffset;
    }
    public override string GetDisplayName()
    {
        return "Initial Anchor Offset";
    }
    public Vector3 GetInitAnchorOffset()
    {
        return initAnchorOffset;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleVector3Field("Offset", fl_initAnchorOffset, skin);
        FieldInspectorHelper.EndSection();
    }
#endif
}
