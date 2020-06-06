using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_AlignToVelocity : EM_RotationModifier {

    private Quaternion targetRotation;

    void Awake()
    {
        moduleType = ModuleType.AlignToVelocity;
    }
    void Start()
    {

    }
    public override string GetDisplayName()
    {
        return "Align To Velocity";
    }
    public override void UpdateRotation(SE_SkillObject skillObj, float deltaTime)
    {
        Transform objSimulationSpace = skillObj.transform.parent;
        Vector3 targetDir;
        if (objSimulationSpace == null)//World Space
            targetDir = skillObj.transform.position + skillObj.velocity;
        else
            targetDir = skillObj.transform.position + objSimulationSpace.TransformDirection(skillObj.velocity);
        skillObj.transform.LookAt(targetDir);
    }
}
