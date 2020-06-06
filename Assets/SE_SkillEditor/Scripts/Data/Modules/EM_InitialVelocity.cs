using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_InitialVelocity : EM_ModuleBase
{
    public Vector3 velocity
    {
        get { return fl_velocity.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_velocity.SetValue(value); }
    }
    public FlexibleVector3 fl_velocity = new FlexibleVector3(Vector3.forward * 5);
    void Awake()
    {
        moduleType = ModuleType.InitialVelocity;
    }

    public override string GetDisplayName()
    {
        return "Initial Velocity";
    }

    public Vector3 GetInitVelocity()
    {
        if (!enabled) return Vector3.zero;

        return velocity;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleVector3Field("Velocity", fl_velocity, skin);
        FieldInspectorHelper.EndSection();
    }
#endif
}
