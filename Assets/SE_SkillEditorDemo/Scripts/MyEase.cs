using UnityEngine;

public enum MyEaseType
{
    linearTween,
    easeInQuad,
    easeOutQuad,
    easeInOutQuad,
    easeInCubic,
    easeOutCubic,
    easeInOutCubic,
    easeInQuart,
    easeOutQuart,
    easeInOutQuart,
    easeInQuint,
    easeOutQuint,
    easeInOutQuint,
    easeInSine,
    easeOutSine,
    easeInOutSine,
    easeInExpo,
    easeOutExpo,
    easeInOutExpo,
    easeInCirc,
    easeOutCirc,
    easeInOutCirc,
    easeInBack,
    easeOutBack,
    easeInOutBack
}

class MyEase
{
    public static float easeTween(MyEaseType type, float time, float startVal, float changeInVal, float duration)
    {
        switch (type)
        {
            case MyEaseType.linearTween:
                return linearTween(time, startVal, changeInVal, duration);
            case MyEaseType.easeInQuad:
                return easeInQuad(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutQuad:
                return easeOutQuad(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutQuad:
                return easeInOutQuad(time, startVal, changeInVal, duration);
            case MyEaseType.easeInCubic:
                return easeInCubic(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutCubic:
                return easeOutCubic(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutCubic:
                return easeInOutCubic(time, startVal, changeInVal, duration);
            case MyEaseType.easeInQuart:
                return easeInQuart(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutQuart:
                return easeOutQuart(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutQuart:
                return easeInOutQuart(time, startVal, changeInVal, duration);
            case MyEaseType.easeInQuint:
                return easeInQuint(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutQuint:
                return easeOutQuint(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutQuint:
                return easeInOutQuint(time, startVal, changeInVal, duration);
            case MyEaseType.easeInSine:
                return easeInSine(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutSine:
                return easeOutSine(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutSine:
                return easeInOutSine(time, startVal, changeInVal, duration);
            case MyEaseType.easeInExpo:
                return easeInExpo(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutExpo:
                return easeOutExpo(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutExpo:
                return easeInOutExpo(time, startVal, changeInVal, duration);
            case MyEaseType.easeInCirc:
                return easeInCirc(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutCirc:
                return easeOutCirc(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutCirc:
                return easeInOutCirc(time, startVal, changeInVal, duration);
            case MyEaseType.easeInBack:
                return easeInBack(time, startVal, changeInVal, duration);
            case MyEaseType.easeOutBack:
                return easeOutBack(time, startVal, changeInVal, duration);
            case MyEaseType.easeInOutBack:
                return easeInOutBack(time, startVal, changeInVal, duration);
            default:
                return linearTween(time, startVal, changeInVal, duration);
        }
    }

    public static float linearTween(float t, float b, float c, float d)
    {
        return c * t / d + b;
    }
    public static float easeInQuad(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t + b;
    }
    public static float easeOutQuad(float t, float b, float c, float d)
    {
        t /= d;
        return -c * t * (t - 2) + b;
    }
    public static float easeInOutQuad(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t + b;
        t--;
        return -c / 2 * (t * (t - 2) - 1) + b;
    }
    public static float easeInCubic(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t + b;
    }
    public static float easeOutCubic(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * (t * t * t + 1) + b;
    }
    public static float easeInOutCubic(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t + 2) + b;
    }
    public static float easeInQuart(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t + b;
    }
    public static float easeOutQuart(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return -c * (t * t * t * t - 1) + b;
    }
    public static float easeInOutQuart(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t + b;
        t -= 2;
        return -c / 2 * (t * t * t * t - 2) + b;
    }
    public static float easeInQuint(float t, float b, float c, float d)
    {
        t /= d;
        return c * t * t * t * t * t + b;
    }
    public static float easeOutQuint(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * (t * t * t * t * t + 1) + b;
    }
    public static float easeInOutQuint(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * t * t * t * t * t + b;
        t -= 2;
        return c / 2 * (t * t * t * t * t + 2) + b;
    }
    public static float easeInSine(float t, float b, float c, float d)
    {
        return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
    }
    public static float easeOutSine(float t, float b, float c, float d)
    {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }
    public static float easeInOutSine(float t, float b, float c, float d)
    {
        return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
    }
    public static float easeInExpo(float t, float b, float c, float d)
    {
        return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
    }
    public static float easeOutExpo(float t, float b, float c, float d)
    {
        return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
    }
    public static float easeInOutExpo(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
        t--;
        return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
    }
    public static float easeInCirc(float t, float b, float c, float d)
    {
        t /= d;
        return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
    }
    public static float easeOutCirc(float t, float b, float c, float d)
    {
        t /= d;
        t--;
        return c * Mathf.Sqrt(1 - t * t) + b;
    }
    public static float easeInOutCirc(float t, float b, float c, float d)
    {
        t /= d / 2;
        if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
        t -= 2;
        return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
    }
    public static float easeOutBack(float t, float b, float c, float d)
    {
        return easeOutBack(t, b, c, d, 1.70158f);
    }
    public static float easeOutBack(float t, float b, float c, float d, float s)
    {
        return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
    }
    public static float easeInBack(float t, float b, float c, float d)
    {
        return easeInBack(t, b, c, d, 1.70158f);
    }
    public static float easeInBack(float t, float b, float c, float d, float s)
    {
        return c * (t /= d) * t * ((s + 1) * t - s) + b;
    }
    public static float easeInOutBack(float t, float b, float c, float d)
    {
        return easeInBack(t, b, c, d, 1.70158f);
    }
    public static float easeInOutBack(float t, float b, float c, float d, float s)
    {
        if (t < d / 2) return easeOutBack(t * 2, b, c / 2, d, s);
        return easeInBack((t * 2) - d, b + c / 2, c / 2, d, s);
    }
}

