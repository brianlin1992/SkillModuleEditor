using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;

[Serializable]
public class EM_SpawnLocationData
{
    public Vector3 Location;
    public Vector3 Foward;

    public EM_SpawnLocationData()
    {

    }
    public EM_SpawnLocationData(Vector3 location, Vector3 forward)
    {
        Location = location;
        Foward = forward;
    }
}
public abstract class EM_SpawnLocation : EM_ModuleBase {


    public abstract EM_SpawnLocationData GetSpawnLocation(SE_SkillObject skillObj);
}
