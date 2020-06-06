using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
public class EM_VelocityNoise : EM_VelocityModifier
{
    public bool bSeperateXYZ = false;
    public FlexibleFloat fl_noiseSpeed = new FlexibleFloat(1);
    public FlexibleFloat fl_noiseStrength = new FlexibleFloat(1);

    public FlexibleVector3 fl_noiseSpeedXYZ = new FlexibleVector3(Vector3.one);
    public FlexibleVector3 fl_noiseStrengthXYZ = new FlexibleVector3(Vector3.one);

    public Vector3 worldPositionInfluence = Vector3.zero;

    void Awake()
    {
        moduleType = ModuleType.VelocityNoise;
        Priority = 2;
    }
    void Start()
    {

    }
    public override string GetDisplayName()
    {
        return "Velocity Noise";
    }
    public override Vector3 UpdateVelocity(SE_SkillObject skillObj, float deltaTime)
    {
        if (!enabled) return skillObj.velocity;
        Vector3 faceToTargetVelocity, noiseSpeed, noiseStrength, noiseVal, lerpVelocity = Vector3.zero;

        if (!bSeperateXYZ)
        {
            noiseSpeed = Vector3.one * fl_noiseSpeed.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
            noiseStrength = Vector3.one * fl_noiseStrength.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
            
        }
        else
        {
            noiseSpeed = fl_noiseSpeedXYZ.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
            noiseStrength = fl_noiseStrengthXYZ.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
        }
        noiseVal.x = Mathf.PerlinNoise(Time.time * noiseSpeed.x + (skillObj.transform.position.x * worldPositionInfluence.x), 0.0f);
        noiseVal.y = Mathf.PerlinNoise(Time.time * noiseSpeed.y + (skillObj.transform.position.y * worldPositionInfluence.y), 0.3f);
        noiseVal.z = Mathf.PerlinNoise(Time.time * noiseSpeed.z + (skillObj.transform.position.z * worldPositionInfluence.z), 0.6f);
        faceToTargetVelocity = new Vector3(noiseVal.x - 0.5f, noiseVal.y - 0.5f, noiseVal.z - 0.5f).normalized;
        
        lerpVelocity.x = Mathf.Lerp(skillObj.velocity.normalized.x, faceToTargetVelocity.x, Mathf.Clamp01(deltaTime * noiseStrength.x));
        lerpVelocity.y = Mathf.Lerp(skillObj.velocity.normalized.y, faceToTargetVelocity.y, Mathf.Clamp01(deltaTime * noiseStrength.y));
        lerpVelocity.z = Mathf.Lerp(skillObj.velocity.normalized.z, faceToTargetVelocity.z, Mathf.Clamp01(deltaTime * noiseStrength.z));

        float velocityMagnitude = skillObj.velocity.magnitude;
        lerpVelocity = lerpVelocity.normalized * velocityMagnitude;
        Transform objSimulationSpace = skillObj.transform.parent;
        //if (objSimulationSpace == null)//World Space
            return lerpVelocity;
        //else
        //    return Vector3.Lerp(skillObj.velocity, objSimulationSpace.TransformDirection(faceToTargetVelocity), deltaTime * noiseSpeed).normalized * velocityMagnitude;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        bSeperateXYZ = FieldInspectorHelper.ShowBoolField("Seperate XYZ", bSeperateXYZ, skin);
        if (!bSeperateXYZ)
        {
            FieldInspectorHelper.ShowFlexibleFloatField("Noise Speed", fl_noiseSpeed, skin);
            FieldInspectorHelper.ShowFlexibleFloatField("Noise Strength", fl_noiseStrength, skin);
        }
        else
        {
            FieldInspectorHelper.ShowFlexibleVector3Field("Speed", fl_noiseSpeedXYZ, skin);
            FieldInspectorHelper.ShowFlexibleVector3Field("Strength", fl_noiseStrengthXYZ, skin);
        }
        FieldInspectorHelper.EndSection();

        FieldInspectorHelper.ShowTitle("World Position Influence", skin, true);
        FieldInspectorHelper.StartSection();
        worldPositionInfluence = FieldInspectorHelper.ShowVector3Field("Strength", worldPositionInfluence, skin);
        FieldInspectorHelper.EndSection();
    }
#endif
}