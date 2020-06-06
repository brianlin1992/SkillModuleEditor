using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

public class EM_AnchorOffsetOverLife : EM_AnchorOffsetModifier
{
    public ModifierType modifierType;

    Vector3 anchorOffsetOverLife;
    public FlexibleVector3 fl_anchorOffsetOverLife = new FlexibleVector3(Vector3.zero);

    Vector3 calAnchorOffset = Vector3.zero;

    void Awake()
    {
        moduleType = ModuleType.AnchorOffsetOverLife;
    }
    public override string GetDisplayName()
    {
        return "Anchor Offset Over Life";
    }
    public override Vector3 UpdateAnchorOffset(SE_SkillObject skillObj, float deltaTime)
    {
        anchorOffsetOverLife = fl_anchorOffsetOverLife.GetValueByTimeRatio(1 - skillObj.lifetimeRemainRatio);
        switch (modifierType)
        {
            case ModifierType.Override:
                return anchorOffsetOverLife;
            case ModifierType.Multiplier:
                {
                    calAnchorOffset.x = skillObj.initAnchorOffset.x * anchorOffsetOverLife.x;
                    calAnchorOffset.y = skillObj.initAnchorOffset.y * anchorOffsetOverLife.y;
                    calAnchorOffset.z = skillObj.initAnchorOffset.z * anchorOffsetOverLife.z;
                    return calAnchorOffset;
                }
            case ModifierType.Add:
                return skillObj.initAnchorOffset + anchorOffsetOverLife;
            case ModifierType.AddMultiplier:
                {
                    calAnchorOffset.x = skillObj.initAnchorOffset.x * anchorOffsetOverLife.x;
                    calAnchorOffset.y = skillObj.initAnchorOffset.y * anchorOffsetOverLife.y;
                    calAnchorOffset.z = skillObj.initAnchorOffset.z * anchorOffsetOverLife.z;
                    return skillObj.initAnchorOffset + calAnchorOffset;
                }
            default:
                break;
        }
        return anchorOffsetOverLife; 
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();
        modifierType = (ModifierType) FieldInspectorHelper.ShowEnumField("Modifier Type", modifierType, skin);
        FieldInspectorHelper.ShowFlexibleVector3Field("Offset", fl_anchorOffsetOverLife, skin);
        FieldInspectorHelper.EndSection();
    }
#endif
}
