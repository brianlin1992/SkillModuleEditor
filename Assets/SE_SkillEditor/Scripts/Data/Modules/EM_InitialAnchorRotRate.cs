using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InitialAnchorRotRate : EM_ModuleBase
{
    public FlexibleVector3 fl_rotRate = new FlexibleVector3();
    void Awake()
    {
        moduleType = ModuleType.InitialAnchorRotRate;
    }
    public override string GetDisplayName()
    {
        return "Initial Anchor RotRate";
    }
    public Vector3 GetInitAnchorRotationRate()
    {
        return fl_rotRate.GetValueByTimeRatio(emitter.timeRatio);
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleVector3Field("RotRate", fl_rotRate, skin, true);

        FieldInspectorHelper.EndSection();

    }
#endif
}