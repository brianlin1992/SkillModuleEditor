using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;


public abstract class EM_RotationModifier : EM_ModuleBase
{
    public int Priority = 0; //smallest run first
    public abstract void UpdateRotation(SE_SkillObject skillObj, float deltaTime);
}
