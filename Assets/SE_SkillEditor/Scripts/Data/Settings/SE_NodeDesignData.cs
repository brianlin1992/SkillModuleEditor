using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Skill Node Design Data", menuName = "Skill Node Design Data")]
public class SE_NodeDesignData : ScriptableObject
{
    public Vector2 size;
    public Vector2 gap;
    public GUIStyle nodeAvatar;
    public GUIStyle propertyTextStyle;
}
