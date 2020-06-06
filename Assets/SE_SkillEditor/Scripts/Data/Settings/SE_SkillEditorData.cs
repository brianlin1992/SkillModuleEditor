using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Editor Data", menuName = "Skill Editor Data")]
public class SE_SkillEditorData : ScriptableObject
{

    [Serializable]
    public class ModuleMenuItem
    {
        public ModuleSubType subType;
        public ModuleType moduleType;
    }
    #region Public variables
    public List<ModuleMenuItem> moduleMenuList;
    #endregion

    #region Protected Variables
    #endregion

    #region Constructors
    #endregion

    #region Main Methods
    #endregion

    #region Utility Methods
    #endregion

}
