using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New UI Editor Setting", menuName = "Skill UI Setting")]
public class SE_SkillUIEditorSetting : ScriptableObject {
    
    [Serializable]
    public class ModuleDesignSetting
    {
        public ModuleType moduleType;
        public SE_NodeDesignData designData;
    }
    #region Public variables
    public GUISkin ViewGUISkin;
    public GUISkin InspectorGUISkin;
    public SE_NodeDesignData emitterDesignSetting;
    public List<ModuleDesignSetting> modulesDesignSetting;
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
