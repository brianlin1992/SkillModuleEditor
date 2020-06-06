using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class EM_Lifetime : EM_ModuleBase {

    public float lifetime
    {
        get { return fl_lifetime.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_lifetime.SetValue(value); }
    }
    public FlexibleFloat fl_lifetime = new FlexibleFloat(1);
    void Awake()
    {
        moduleType = ModuleType.Lifetime;
    }
    public override string GetDisplayName()
    {
        return "Lifetime";
    }
    public float GetInitLifetime()
    {
        return lifetime;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleFloatField("Lifetime", fl_lifetime, skin);
        FieldInspectorHelper.EndSection();

    }
#endif
}
