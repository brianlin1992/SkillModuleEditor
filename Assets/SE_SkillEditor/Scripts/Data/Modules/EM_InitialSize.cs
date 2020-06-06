using UnityEngine;


public class EM_InitialSize : EM_ModuleBase
{
    public bool lockXYZ = true;

    public FlexibleVector3 fl_size3D = new FlexibleVector3(Vector3.one);
    public FlexibleFloat fl_size = new FlexibleFloat(1);
    void Awake()
    {
        moduleType = ModuleType.InitialSize;
    }
    public override string GetDisplayName()
    {
        return "Initial Size";
    }
    public Vector3 GetInitSize()
    {
        return lockXYZ
                ? fl_size.GetValueByTimeRatio(emitter.timeRatio) * Vector3.one
                : fl_size3D.GetValueByTimeRatio(emitter.timeRatio);
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        lockXYZ = FieldInspectorHelper.ShowBoolField("Lock XYZ", lockXYZ, skin);
        if(!lockXYZ)
        {
            FieldInspectorHelper.ShowFlexibleVector3Field("Size", fl_size3D, skin);
        }
        else
        {
            FieldInspectorHelper.ShowFlexibleFloatField("Size", fl_size, skin);
        }
        
        FieldInspectorHelper.EndSection();

    }
#endif
}