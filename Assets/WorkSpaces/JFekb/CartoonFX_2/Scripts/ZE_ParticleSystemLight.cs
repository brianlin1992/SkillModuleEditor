using UnityEngine;
using System.Collections;

public class ZE_ParticleSystemLight : MonoBehaviour
{
    public AnimationCurve LightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float LightIntencity = 1;
    public float Radius = 15;
    //public bool UseDurationFromParticleSystem = true;
    //public float Duration = -1;

    private float timeDuration = 1;
    private bool isLoop;
    private bool canUpdate;
    private float startTime;
    private Light lightSource;
    private Material mat;

    private void Awake()
    {
        lightSource = gameObject.AddComponent<Light>();
        lightSource.range = Radius;
        lightSource.intensity = LightCurve.Evaluate(0);
        var ps = gameObject.GetComponent<ParticleSystem>();
        mat = ps.GetComponent<Renderer>().sharedMaterial;
        var color = mat.GetColor("_TintColor");
        var max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
        color.r = Mathf.Clamp01(color.r / max);
        color.g = Mathf.Clamp01(color.g / max);
        color.b = Mathf.Clamp01(color.b / max);
        color.a = 1;
        lightSource.color = color;
#if UNITY_5_3
        //timeDuration = UseDurationFromParticleSystem? ps.startLifetime : Duration;
        var usedRate = ps.emission.rate.constantMax > 0 ? true : false;
        timeDuration = !usedRate ? ps.startLifetime : ps.startLifetime + ps.duration;
        isLoop = ps.loop;
#else
        //timeDuration = UseDurationFromParticleSystem ? ps.main.startLifetime.constantMax : Duration;
        var usedRate = ps.emission.rateOverTime.constantMax > 0 ? true : false;
         timeDuration = !usedRate ? ps.main.startLifetime.constantMax : ps.main.startLifetime.constantMax + ps.main.duration;
         isLoop = ps.main.loop;
#endif
        if (isLoop) LightCurve = AnimationCurve.EaseInOut(0, 1, 1, 1);
    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = LightCurve.Evaluate(time / timeDuration) * LightIntencity;
            lightSource.intensity = eval;
            //var color = mat.GetColor("_TintColor");
            //var max = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            //color.r = Mathf.Clamp01(color.r / max);
            //color.g = Mathf.Clamp01(color.g / max);
            //color.b = Mathf.Clamp01(color.b / max);
            //color.a = 1;
            //lightSource.color = color;
        }
        if (time >= timeDuration) {
            if (isLoop) startTime = Time.time;
            else canUpdate = false;
        }
    }
}