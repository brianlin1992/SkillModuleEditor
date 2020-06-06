using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class EM_VelocityScaleOverLife : EM_VelocityModifier
{
    public ModifierType modifierType;
    public FlexibleFloat fl_velocityScale = new FlexibleFloat(1);

    void Awake()
    {
        moduleType = ModuleType.VelocityScaleOverLife;
        Priority = 10;
    }
    void Start()
    {
        
    }
    public override string GetDisplayName()
    {
        return "Velocity Scale Over Life";
    }
    public override Vector3 UpdateVelocity(SE_SkillObject skillObj, float deltaTime)
    {
        if (!enabled) return skillObj.velocity;

        float scale = fl_velocityScale.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
        float initVelocityMagnitude = skillObj.initVelocity.magnitude;

        Vector3 velocityDir = skillObj.velocity.normalized;

        switch (modifierType)
        {
            case ModifierType.Override:
                return scale * velocityDir;
            case ModifierType.Multiplier:
                return (initVelocityMagnitude * scale) * velocityDir;
            case ModifierType.Add:
                return (initVelocityMagnitude + scale) * velocityDir;
            case ModifierType.AddMultiplier:
                return (initVelocityMagnitude + initVelocityMagnitude * scale) * velocityDir;
            default:
                break;
        }
        return skillObj.velocity;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        modifierType = (ModifierType)FieldInspectorHelper.ShowEnumField("Modifier Type", modifierType, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Velocity Scale", fl_velocityScale, skin);
        FieldInspectorHelper.EndSection();
    }
#endif

}