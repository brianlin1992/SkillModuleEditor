using UnityEngine;

[System.Serializable]
public class FlexibleVector3
{
    public FlexibleEditType type;
    public Vector3 uniformValue;
    [UnityEngine.SerializeField]
    public StatRangeFloat rangeX = new StatRangeFloat(0,0);
    [UnityEngine.SerializeField]
    public StatRangeFloat rangeY = new StatRangeFloat(0, 0);
    [UnityEngine.SerializeField]
    public StatRangeFloat rangeZ = new StatRangeFloat(0, 0);
    [UnityEngine.SerializeField]
    public UnityEngine.AnimationCurve curveX;
    [UnityEngine.SerializeField]
    public UnityEngine.AnimationCurve curveY;
    [UnityEngine.SerializeField]
    public UnityEngine.AnimationCurve curveZ;

    public Vector3 value
    {
        get {
            switch (type)
            {
                case FlexibleEditType.Uniform:
                    return uniformValue;
                case FlexibleEditType.RangeTween:
                    return new Vector3(rangeX.min, rangeY.min, rangeZ.min);
                case FlexibleEditType.RangeRandom:
                    return new Vector3(rangeX.random, rangeY.random, rangeZ.random);
                case FlexibleEditType.Curve:
                    return new Vector3(curveX.Evaluate(0), curveY.Evaluate(0), curveZ.Evaluate(0));
            }
            return uniformValue;
        }
    }
    public Vector3 GetValueByTimeRatio(float ratio)
    {
        switch (type)
        {
            case FlexibleEditType.Uniform:
                return uniformValue;
            case FlexibleEditType.RangeTween:
                return new Vector3(rangeX.Evaluate(ratio), rangeY.Evaluate(ratio), rangeZ.Evaluate(ratio));
            case FlexibleEditType.RangeRandom:
                return new Vector3(rangeX.random, rangeY.random, rangeZ.random);
            case FlexibleEditType.Curve:
                return new Vector3(curveX.Evaluate(ratio), curveY.Evaluate(ratio), curveZ.Evaluate(ratio));
        }
        return uniformValue;
    }
    public void SetDefaultValue(Vector3 value)
    {
        uniformValue = value;
        rangeX.min = value.x;
        rangeX.max = value.x;
        rangeY.min = value.y;
        rangeY.max = value.y;
        rangeZ.min = value.z;
        rangeZ.max = value.z;
        curveX = new UnityEngine.AnimationCurve();
        curveX.AddKey(new UnityEngine.Keyframe(0, value.x));
        curveY = new UnityEngine.AnimationCurve();
        curveY.AddKey(new UnityEngine.Keyframe(0, value.y));
        curveZ = new UnityEngine.AnimationCurve();
        curveZ.AddKey(new UnityEngine.Keyframe(0, value.z));
    }
    public void SetValue(Vector3 value)
    {
        uniformValue = value;
        rangeX.min = value.x;
        rangeX.max = value.x;
        rangeY.min = value.y;
        rangeY.max = value.y;
        rangeZ.min = value.z;
        rangeZ.max = value.z;
        rangeX.max = value.x;
        rangeY.max = value.y;
        rangeZ.max = value.z;
        if (curveX.length == 1)
            curveX.keys[0].value = value.x;
        if (curveY.length == 1)
            curveY.keys[0].value = value.y;
        if (curveZ.length == 1)
            curveZ.keys[0].value = value.z;
    }

    public FlexibleVector3()
    {
        SetDefaultValue(Vector3.zero);
    }

    public FlexibleVector3(Vector3 val)
    {
        SetDefaultValue(val);
    }

    public static FlexibleVector3 One
    {
        get { return new FlexibleVector3(Vector3.one); }
    }
    public static FlexibleVector3 Zero
    {
        get { return new FlexibleVector3(Vector3.zero); }
    }
}