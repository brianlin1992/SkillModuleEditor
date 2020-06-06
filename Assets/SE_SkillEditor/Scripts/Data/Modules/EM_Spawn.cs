using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Spawn : EM_ModuleBase
{
   
    public float rateOverTime
    {
        get { return fl_rateOverTime.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_rateOverTime.SetValue(value); }
    }
    public FlexibleFloat fl_rateOverTime = new FlexibleFloat(3);
    public List<BrustData> brustList = new List<BrustData>();
    

    void Awake()
    {
        moduleType = ModuleType.Spawn;
    }
    // Use this for initialization
    void Start () {
		
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string GetDisplayName()
    {
        return "Spawn";
    }

    public float GetSpawnCountOverEmitterDuration()
    {
        float rateOverTimeCount = rateOverTime * emitter.duration;
        return rateOverTimeCount;
    }

#if UNITY_EDITOR
    float minWidth = 15;
    float addSubWidth = 0;
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleFloatField("Unit/s", fl_rateOverTime, skin);
        FieldInspectorHelper.EndSection();
        
        FieldInspectorHelper.ShowTitle("Brust", skin, true);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.StartRow(0);
        FieldInspectorHelper.ShowText("Time", skin, minWidth);
        FieldInspectorHelper.ShowText("Count", skin, minWidth);
        FieldInspectorHelper.ShowText("Cycle", skin, minWidth);
        FieldInspectorHelper.ShowText("Interval", skin, minWidth);
        FieldInspectorHelper.ShowAddSubButton(true, false, skin, () =>
        {
            brustList.Insert(0, new BrustData());
        }, null);
        FieldInspectorHelper.EndRow(0);
        for (int i = 0; i < brustList.Count; i++)
        {
            var brust = brustList[i];

            FieldInspectorHelper.StartRow(0);
            brust.time = FieldInspectorHelper.ShowFloatField(brust.time, skin, minWidth);
            brust.count = FieldInspectorHelper.ShowIntField(brust.count, skin, minWidth);
            brust.cycle = FieldInspectorHelper.ShowIntField(brust.cycle, skin, minWidth);
            brust.interval = FieldInspectorHelper.ShowFloatField(brust.interval, skin, minWidth);
            FieldInspectorHelper.ShowAddSubButton(true, true, skin, () =>
            {
                brustList.Insert(i+1, brust.Clone());
            }, () =>
            {
                brustList.RemoveAt(i);
            });
            FieldInspectorHelper.EndRow(0);
        }
        //BrustCount = FieldInspectorHelper.ShowIntField("Count", BrustCount, skin);

        FieldInspectorHelper.EndSection();
    }
#endif
}
[Serializable]
public class BrustData
{
    public float time;
    public int count = 0;
    public int cycle = 1;
    public float interval;
    int runTime;

    public BrustData Clone()
    {
        return this.DeepClone<BrustData>();
    }
    public void updateTime(float _time, Action<int> spawnAction)
    {
        if (_time == 0)
            runTime = 0;
        if (runTime >= cycle)
            return;
        if(_time > time + runTime * interval)
        {
            runTime++;
            spawnAction(count);
        }

    }
}
