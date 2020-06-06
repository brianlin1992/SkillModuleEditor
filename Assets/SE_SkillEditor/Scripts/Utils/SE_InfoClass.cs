using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EmitterTrigger
{
    public SE_Emitter emitter;
    public bool useLocation;
    public bool useRotation;
}
[System.Serializable]
public partial class SpawnGameObjectTrigger
{
    public GameObject template;
    public int count = 1;
    public bool useLocation;
    public bool useRotation;
    public ParentTo parentTo;
}
#if UNITY_EDITOR
public partial class SpawnGameObjectTrigger
{
    public bool bShow;
}
#endif