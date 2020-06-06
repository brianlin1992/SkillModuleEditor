using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Line : EM_SpawnLocation
{
    public float length
    {
        get { return fl_length.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_length.SetValue(value); }
    }
    public FlexibleFloat fl_length = new FlexibleFloat(2);
    public float angle
    {
        get { return fl_angle.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_angle.SetValue(value); }
    }
    public FlexibleFloat fl_angle = new FlexibleFloat(0);

    public SpawnLocationMode locationMode = SpawnLocationMode.Random;
    public bool bUseSpawnTotalCount = true;
    public int unitPerLoop = 5;


    float oldLength = 0;
    float oldAngle = 0;
    Vector3 baseLocation;

    public static Queue<EM_SpawnLocationData> debugLine = new Queue<EM_SpawnLocationData>();
    void Awake()
    {
        moduleType = ModuleType.Line;
    }
    void Update()
    {

    }
    public override string GetDisplayName()
    {
        return "Line";
    }
    public override EM_SpawnLocationData GetSpawnLocation(SE_SkillObject skillObj)
    {
        updateParamsChanged();

        var data = new EM_SpawnLocationData();
        baseLocation = transform.position - transform.right * (length / 2);

        baseLocation += transform.right * (getPositionRatioByLocationMode(skillObj) * length);

        float spanAngle = -Mathf.PI * 2f * angle / 360;

        Vector3 dir = TransformUtility.Get3DCirclePoint(spanAngle, 1, Vector3.zero, -transform.right, transform.up);

        data.Location = baseLocation;
        data.Foward = dir;


        debugLine.Enqueue(data);
        if (debugLine.Count > 10)
            debugLine.Dequeue();
        return data;
    }
    public float getPositionRatioByLocationMode(SE_SkillObject skillObj)
    {
        var spawnModule = GetComponent<EM_Spawn>();
        switch (locationMode)
        {
            case SpawnLocationMode.Random:
                return Random.value;
            case SpawnLocationMode.LoopByTimeRatio:
                return emitter.timeRatio;
            case SpawnLocationMode.LoopByCount:
                float totalSpawnOverDuration = bUseSpawnTotalCount ? spawnModule.GetSpawnCountOverEmitterDuration() : unitPerLoop;
                //Invalid spawn count
                if (totalSpawnOverDuration == 0) return 0;
                return ((emitter.spawnCount / totalSpawnOverDuration) % 1) * (1f + 1f / (totalSpawnOverDuration-1));
            case SpawnLocationMode.LoopByBrust:
                Debug.Log(skillObj.brustCount);
                return skillObj.brustCount == 0 ? 0 : ((skillObj.brustIndex * 1.0f / skillObj.brustCount) % 1) * (1f + 1f / (skillObj.brustCount - 1));
        }
        return 0;
    }
    bool updateParamsChanged()
    {
        bool changed = false;
        if (oldLength != length
            || oldAngle != angle)
        {
            changed = true;
            oldLength = length;
            oldAngle = angle;
        }
        return changed;
    }
#if UNITY_EDITOR
    public override void DrawModuleHelper()
    {
        updateParamsChanged();
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawLine(transform.position - transform.right * (length / 2), transform.position + transform.right * (length / 2));

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

        FieldInspectorHelper.ShowFlexibleFloatField("Length", fl_length, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Angle", fl_angle, skin);
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