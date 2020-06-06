using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Homing : EM_VelocityModifier
{
    public FlexibleFloat fl_homingSpeed = new FlexibleFloat(3);
    public FlexibleVector3 fl_homingStrengthXYZ = new FlexibleVector3(Vector3.one);

    protected Vector3 initialPosition;


    void Awake()
    {
        moduleType = ModuleType.Homing;
        Priority = 1;
    }
    void Start()
    {
        initialPosition = transform.position;
    }
    public override string GetDisplayName()
    {
        return "Homing Missile";
    }
    public override Vector3 UpdateVelocity(SE_SkillObject skillObj, float deltaTime)
    {
        float homingSpeed = fl_homingSpeed.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);

        if (!enabled) return skillObj.velocity;
        if (skillObj.target == null) return skillObj.velocity;

        float velocityMagnitude = skillObj.velocity.magnitude;
        Vector3 faceDir = (skillObj.target.position - skillObj.transform.position);
        Vector3 homingStrength = fl_homingStrengthXYZ.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
        faceDir.x *= Mathf.Clamp01(homingStrength.x);
        faceDir.y *= Mathf.Clamp01(homingStrength.y);
        faceDir.z *= Mathf.Clamp01(homingStrength.z);
        Vector3 faceToTargetVelocity = faceDir.normalized * velocityMagnitude;
        Vector3 lerpVelocity = Vector3.Lerp(skillObj.velocity, faceToTargetVelocity, deltaTime * homingSpeed).normalized * velocityMagnitude;

        Transform objSimulationSpace = skillObj.transform.parent;

        if (objSimulationSpace == null)//World Space
            return lerpVelocity;
        else
            return Vector3.Lerp(skillObj.velocity, objSimulationSpace.TransformDirection(faceToTargetVelocity), deltaTime * homingSpeed).normalized * velocityMagnitude;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        FieldInspectorHelper.ShowFlexibleFloatField("Speed", fl_homingSpeed, skin);
        FieldInspectorHelper.ShowFlexibleVector3Field("Strength", fl_homingStrengthXYZ, skin);
        FieldInspectorHelper.EndSection();
    }
#endif

}