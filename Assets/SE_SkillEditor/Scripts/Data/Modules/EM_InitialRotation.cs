using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InitialRotation : EM_ModuleBase
{
    public FlexibleVector3 fl_rotation = new FlexibleVector3();
    void Awake()
    {
        moduleType = ModuleType.InitialRotation;
    }
    public override string GetDisplayName()
    {
        return "Initial Rotation";
    }
    public Vector3 GetInitRotation()
    {
        return fl_rotation.GetValueByTimeRatio(emitter.timeRatio);
    }

#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleVector3Field("Rotation", fl_rotation, skin, true);
        FieldInspectorHelper.EndSection();

    }
#endif
}