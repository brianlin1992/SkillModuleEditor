using System.Collections.Generic;
using UnityEngine;

public class EM_Cone : EM_SpawnLocation
{
    public bool bReverseEmitBase;
    public EmitFrom emitFrom;
    public float angle
    {
        get { return fl_angle.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_angle.SetValue(value); }
    }
    public FlexibleFloat fl_angle = new FlexibleFloat(30);
    public float radius
    {
        get { return fl_radius.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_radius.SetValue(value); }
    }
    public FlexibleFloat fl_radius = new FlexibleFloat(1);
    public float length
    {
        get { return fl_length.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_length.SetValue(value); }
    }
    public FlexibleFloat fl_length = new FlexibleFloat(1);
    public Vector3 topOffset
    {
        get { return fl_topOffset.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_topOffset.SetValue(value); }
    }
    public FlexibleVector3 fl_topOffset = new FlexibleVector3(Vector3.zero);
    public SpawnLocationMode locationMode;
    public bool bUseSpawnTotalCount = true;
    public int unitPerLoop = 5;

    float oldAngle = 0;
    float oldRadius = 0;
    float oldLength = 0;
    Vector3 coneTopLocation;
    Vector3 coneBaseLocation;

    float coneTopRadius;


    public static Queue<EM_SpawnLocationData> debugLine = new Queue<EM_SpawnLocationData>();
    void Awake()
    {
        moduleType = ModuleType.Cone;
    }
    void Update()
    {
        
    }
    public override string GetDisplayName()
    {
        return "Cone";
    }
    public override EM_SpawnLocationData GetSpawnLocation(SE_SkillObject skillObj)
    {
        updateParamsChanged();

        var data = new EM_SpawnLocationData();

        float spanAngle = -Mathf.PI * 2f;
        spanAngle -= getSpanAngleByLocationMode(skillObj);

        Vector3 a = transform.forward;
        float radiusByType = getRadiusByEmitType();
        float lengthRatioByType = getLengthRatioByEmitType();

        coneBaseLocation = TransformUtility.Get3DCirclePoint(spanAngle, radiusByType, transform.position, a, transform.right);
        coneTopLocation = TransformUtility.Get3DCirclePoint(spanAngle, coneTopRadius * radiusByType/radius, transform.position + a * length, a, transform.right);
        coneTopLocation += topOffset;

        if (!bReverseEmitBase)
        {
            data.Location = coneBaseLocation + (coneTopLocation - coneBaseLocation) * lengthRatioByType;
            data.Foward = (coneTopLocation - coneBaseLocation).normalized;
        }
        else
        {
            data.Location = coneBaseLocation + (coneTopLocation - coneBaseLocation) * (1 - lengthRatioByType);
            data.Foward = (coneBaseLocation - coneTopLocation).normalized;
        }

        debugLine.Enqueue(data);
        if(debugLine.Count > 10)
            debugLine.Dequeue();
        return data;
    }
    public float getSpanAngleByLocationMode(SE_SkillObject skillObj)
    {
        var spawnModule = GetComponent<EM_Spawn>();
        float fullSpan = Mathf.PI * 2f;
        switch (locationMode)
        {
            case SpawnLocationMode.Random:
                return Random.value * fullSpan;
            case SpawnLocationMode.LoopByTimeRatio:
                return emitter.timeRatio * fullSpan;
            case SpawnLocationMode.LoopByCount:
                float totalSpawnOverDuration = bUseSpawnTotalCount ? spawnModule.GetSpawnCountOverEmitterDuration() : unitPerLoop;
                //Invalid spawn count
                if (totalSpawnOverDuration == 0) return 0;
                return ((emitter.spawnCount / totalSpawnOverDuration) % 1) * fullSpan;
            case SpawnLocationMode.LoopByBrust:
                return skillObj.brustCount == 0 ? 0 : ((skillObj.brustIndex * 1.0f / skillObj.brustCount) % 1) * fullSpan;
        }
        return 0;
    }
    float getRadiusByEmitType()
    {
        switch (emitFrom)
        {
            case EmitFrom.Base:
            case EmitFrom.Volume:
                return Random.value * radius;
            case EmitFrom.BaseSide:
            case EmitFrom.VolumeSide:
                return radius;
            default:
                return radius;
        }
    }
    float getLengthRatioByEmitType()
    {
        switch (emitFrom)
        {
            case EmitFrom.Base:
            case EmitFrom.BaseSide:
                return 0;
            case EmitFrom.Volume:
            case EmitFrom.VolumeSide:
                return Random.value;
            default:
                return 0;
        }
    }

    bool updateParamsChanged()
    {
        bool changed = false;
        if (oldRadius != radius
            || oldAngle != angle
            || oldLength != length)
        {
            changed = true;
            oldRadius = radius;
            oldAngle = angle;
            oldLength = length;
            coneTopRadius = radius + Mathf.Tan(angle * Mathf.PI / 180) * length;
        }
        return changed;
    }
#if UNITY_EDITOR
    public override void DrawModuleHelper()
    {
        Vector3 _topOffset = topOffset;
        updateParamsChanged();
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, radius);
        UnityEditor.Handles.DrawWireDisc(transform.position + _topOffset + transform.forward * length, transform.forward, coneTopRadius);

        float spanAngle = 0;
        int drawLineCount = 4;
        for (int i = 0; i < drawLineCount; i++)
        {
            spanAngle += Mathf.PI * 2f / drawLineCount;
            coneBaseLocation = TransformUtility.Get3DCirclePoint(spanAngle, radius, transform.position, transform.forward, transform.right);
            coneTopLocation = TransformUtility.Get3DCirclePoint(spanAngle, coneTopRadius, transform.position + _topOffset + transform.forward * length, transform.forward, transform.right);
            UnityEditor.Handles.DrawLine(coneBaseLocation, coneTopLocation);
        }
        UnityEditor.Handles.color = Color.white;
        foreach (var item in debugLine)
        {
            UnityEditor.Handles.DrawLine(item.Location, item.Location + item.Foward);
        }
    }


    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        bReverseEmitBase = FieldInspectorHelper.ShowBoolField("Reverse Base", bReverseEmitBase, skin);
        emitFrom = (EmitFrom) FieldInspectorHelper.ShowEnumField("Emit From", emitFrom, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Angle", fl_angle, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Radius", fl_radius, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Length", fl_length, skin);
        FieldInspectorHelper.ShowFlexibleVector3Field("Top Offset", fl_topOffset, skin);
        locationMode = (SpawnLocationMode)FieldInspectorHelper.ShowEnumField("Mode", locationMode, skin);

        FieldInspectorHelper.EndSection();


        switch (locationMode)
        {
            case SpawnLocationMode.Random:
                break;
            case SpawnLocationMode.LoopByTimeRatio:
                break;
            case SpawnLocationMode.LoopByCount:
                {
                    FieldInspectorHelper.ShowTitle("Loop By Count", skin, false);
                    FieldInspectorHelper.StartSection();
                    bUseSpawnTotalCount = FieldInspectorHelper.ShowBoolField("Spawn Count", bUseSpawnTotalCount, skin);
                    if (!bUseSpawnTotalCount)
                        unitPerLoop = FieldInspectorHelper.ShowIntField("Unit Per Loop", unitPerLoop, skin);
                    FieldInspectorHelper.EndSection();
                }
                break;
            case SpawnLocationMode.LoopByBrust:
                {

                }
                break;
            default:
                break;
        }
    }

#endif
}