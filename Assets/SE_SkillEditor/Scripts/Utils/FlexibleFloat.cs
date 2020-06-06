public enum FlexibleEditType
{
    Uniform,
    RangeTween,
    RangeRandom,
    Curve
}

[System.Serializable]
public class FlexibleFloat
{
    public FlexibleEditType type;
    public float uniformValue;
    [UnityEngine.SerializeField]
    public StatRangeFloat rangeValue = new StatRangeFloat(0,0);
    [UnityEngine.SerializeField]
    public UnityEngine.AnimationCurve curveValue;

    public float value
    {
        get {
            switch (type)
            {
                case FlexibleEditType.Uniform:
                    return uniformValue;
                case FlexibleEditType.RangeTween:
                    return rangeValue.min;
                case FlexibleEditType.RangeRandom:
                    return rangeValue.random; 
                case FlexibleEditType.Curve:
                    return curveValue.Evaluate(0);
            }
            return uniformValue; 
        }
    }
    public float GetValueByTimeRatio(float ratio)
    {
        switch (type)
        {
            case FlexibleEditType.Uniform:
                return uniformValue;
            case FlexibleEditType.RangeTween:
                return rangeValue.Evaluate(ratio);
            case FlexibleEditType.RangeRandom:
                return rangeValue.random;
            case FlexibleEditType.Curve:
                return curveValue.Evaluate(ratio);
        }
        return uniformValue;
    }
    public void SetDefaultValue(float value)
    {
        uniformValue = value;
        rangeValue.min = value;
        rangeValue.max = value;
        curveValue = new UnityEngine.AnimationCurve();
        curveValue.AddKey(new UnityEngine.Keyframe(0, value));
    }
    public void SetValue(float value)
    {
        uniformValue = value;
        rangeValue.max = value;
        if (curveValue.length == 1)
            curveValue.keys[0].value = value;
    }

    public FlexibleFloat() { }
    public FlexibleFloat(float val)
    {
        SetDefaultValue(val);
    }

    public static FlexibleFloat One
    {
        get { return new FlexibleFloat(1); }
    }
}