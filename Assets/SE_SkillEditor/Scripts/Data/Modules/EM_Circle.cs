using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Circle : EM_SpawnLocation
{
    public float radius
    {
        get { return fl_radius.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_radius.SetValue(value); }
    }
    public FlexibleFloat fl_radius = new FlexibleFloat(1);
    public float radiusThickness
    {
        get { return fl_radiusThickness.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_radiusThickness.SetValue(value); }
    }
    public FlexibleFloat fl_radiusThickness = new FlexibleFloat(0);
    public float arc
    {
        get { return fl_arc.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_arc.SetValue(value); }
    }
    public FlexibleFloat fl_arc = new FlexibleFloat(360);
    public float arcOffset
    {
        get { return fl_arcOffset.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_arcOffset.SetValue(value); }
    }
    public FlexibleFloat fl_arcOffset = new FlexibleFloat(0);
    public SpawnLocationMode locationMode;
    public bool bUseSpawnTotalCount = true;
    public int unitPerLoop = 5;
    public bool bReverseDirection = false;


    float oldRadius = 0;
    float oldRadiusThickness = 0;
    float oldArc = 0;
    float oldArcOffset = 0;
    Vector3 baseLocation;


    public static Queue<EM_SpawnLocationData> debugLine = new Queue<EM_SpawnLocationData>();
    void Awake()
    {
        moduleType = ModuleType.Circle;
    }
    void Update()
    {

    }
    public override string GetDisplayName()
    {
        return "Circle";
    }
    public override EM_SpawnLocationData GetSpawnLocation(SE_SkillObject skillObj)
    {
        updateParamsChanged();

        var data = new EM_SpawnLocationData();
        float spanAngle = -(arcOffset / 360) * (Mathf.PI * 2f);
        spanAngle -= getSpanAngleByLocationMode(skillObj);

        Vector3 a = transform.up;
        float radiusByType = radius - (radiusThickness) * radius * Random.value;

        //zero radius cannot generate correct rotation
        if (radiusByType == 0) radiusByType = 0.001f;
        spanAngle *= bReverseDirection ? -1 : 1;
        baseLocation = TransformUtility.Get3DCirclePoint(spanAngle, radiusByType, transform.position, a, transform.right);

        data.Location = baseLocation;
        data.Foward = (baseLocation - transform.position).normalized;


        debugLine.Enqueue(data);
        if (debugLine.Count > 10)
            debugLine.Dequeue();
        return data;
    }
    public float getSpanAngleByLocationMode(SE_SkillObject skillObj)
    {
        var spawnModule = GetComponent<EM_Spawn>();
        float fullSpan = Mathf.PI * 2f * (arc / 360f);
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
                {
                    if (skillObj.brustCount == 0) return 0;
                    if (arc == 360)
                        return skillObj.brustCount == 0 ? 0 : ((skillObj.brustIndex * 1.0f / skillObj.brustCount) % 1) * fullSpan;
                    else
                        if (skillObj.brustIndex == skillObj.brustCount-1)
                            return fullSpan;
                        else
                            return skillObj.brustCount == 0 ? 0 : ((skillObj.brustIndex * 1.0f / (skillObj.brustCount-1)) % 1) * fullSpan;

                }
                
        }
        return 0;
    }
    bool updateParamsChanged()
    {
        bool changed = false;
        if (oldRadius != radius
            || oldRadiusThickness != radiusThickness
            || oldArc != arc
            || oldArcOffset != arcOffset)
        {
            changed = true;
            oldRadius = radius;
            oldRadiusThickness = radiusThickness;
            oldArc = arc;
            oldArcOffset = arcOffset;
        }
        return changed;
    }
#if UNITY_EDITOR
    public override void DrawModuleHelper()
    {
        updateParamsChanged();

        UnityEditor.Handles.color = Color.white;
        foreach (var item in debugLine)
        {
            UnityEditor.Handles.DrawLine(item.Location, item.Location + item.Foward);
        }

        UnityEditor.Handles.color = Color.green;
        if (arc == 0)
            return;
        if (arc == 360)
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, radius);
            UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, radius * (1 - radiusThickness));
        }
        else
        {
            
            float spanAngle = -Mathf.PI * 2f * (arcOffset / 360);
            if (radiusThickness != 0)
            {
                int drawLineCount = 2;
                Vector3[] points = new Vector3[2];

                for (int i = 0; i < drawLineCount; i++)
                {
                    Vector3 from = TransformUtility.Get3DCirclePoint(spanAngle, radius, transform.position, transform.up, transform.right);
                    Vector3 to = TransformUtility.Get3DCirclePoint(spanAngle, radius * (1 - radiusThickness), transform.position, transform.up, transform.right);
                    UnityEditor.Handles.DrawLine(from, to);
                    spanAngle -= Mathf.PI * 2f * (arc / 360);
                    if (i == 0)
                    {
                        points[0] = from;
                        points[1] = to;
                    }
                }
                UnityEditor.Handles.DrawWireArc(transform.position, transform.up, points[0] - points[1], arc, radius);
                UnityEditor.Handles.DrawWireArc(transform.position, transform.up, points[0] - points[1], arc, radius * (1 - radiusThickness));
            }else
            {
                Vector3 from = TransformUtility.Get3DCirclePoint(spanAngle, radius, transform.position, transform.up, transform.right);
                UnityEditor.Handles.DrawWireArc(transform.position, transform.up, from - transform.position, arc, radius);
            }

            
        }

    }
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();

        FieldInspectorHelper.ShowFlexibleFloatField("Radius", fl_radius, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Radius Thickness", fl_radiusThickness, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Arc", fl_arc, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Arc Offset", fl_arcOffset, skin);
        
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
                    bReverseDirection = FieldInspectorHelper.ShowBoolField("Reversed Dir", bReverseDirection, skin);
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