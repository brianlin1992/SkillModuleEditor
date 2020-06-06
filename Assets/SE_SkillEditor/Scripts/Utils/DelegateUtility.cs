using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate float GetFloatDelegate();
public delegate Vector3 GetVector3Delegate();
public delegate Vector3 GetVector3ByVector3Delegate(Vector3 value);
public delegate Transform GetTransformDelegate();
public delegate EM_SpawnLocationData GetSpawnLocationDataDelegate(SE_SkillObject skillObj);

public delegate Vector3 UpdateVector3BySkillObjectDelegate(SE_SkillObject skillObj, float deltaTime);
public delegate void UpdateRotationBySkillObjectDelegate(SE_SkillObject skillObj, float deltaTime);
public delegate void OnCollisionDelegate(SE_SkillObject skillObj, Collider other);