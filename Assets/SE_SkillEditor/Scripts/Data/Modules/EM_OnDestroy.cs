using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_OnDestroy : EM_ModuleBase
{
    public List<EmitterTrigger> triggerEmitterList = new List<EmitterTrigger>();
    public List<SpawnGameObjectTrigger> spawnGameObjectList = new List<SpawnGameObjectTrigger>();


    void Awake()
    {
        moduleType = ModuleType.OnDestroy;
    }
    public override string GetDisplayName()
    {
        return "On Destroy";
    }
    public void OnObjectDestroy(SE_SkillObject skillObj)
    {
        for (int i = 0; i < spawnGameObjectList.Count; i++)
        {
            var spawnInfo = spawnGameObjectList[i];
            if (spawnInfo.template != null)
            {
                for (int j = 0; j < spawnInfo.count; j++)
                {
                    var instance = GameObject.Instantiate<GameObject>(spawnInfo.template);
                    if (spawnInfo.useLocation) instance.transform.position = skillObj.skillInstance.transform.position;
                    if (spawnInfo.useRotation) instance.transform.rotation = skillObj.skillInstance.transform.rotation;
                    switch (spawnInfo.parentTo)
                    {
                        case ParentTo.None:
                            break;
                        case ParentTo.TriggerObject:
                            instance.transform.parent = skillObj.skillInstance.transform;
                            break;
                        case ParentTo.TriggerObjectParent:
                            instance.transform.parent = skillObj.transform.parent;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
#if UNITY_EDITOR

    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);

        FieldInspectorHelper.StartSection();
       
        int spawnGameObjectListCount = FieldInspectorHelper.ShowIntField("Spawn Count", spawnGameObjectList.Count, skin);
        if(spawnGameObjectList.Count != spawnGameObjectListCount)
            spawnGameObjectList.Resize<SpawnGameObjectTrigger>(spawnGameObjectListCount);

        for (int i = 0; i < spawnGameObjectList.Count; i++)
        {
            var spawnTrigger = spawnGameObjectList[i];
            if (spawnTrigger == null)
            {
                spawnTrigger = new SpawnGameObjectTrigger();
                spawnGameObjectList[i] = spawnTrigger;
            }

            spawnTrigger.bShow = FieldInspectorHelper.ShowExpandField(string.Format("Index {0}", i + 1), spawnTrigger.bShow, skin);
            if (spawnTrigger.bShow)
            {
                FieldInspectorHelper.StartSubSection();
                spawnTrigger.template = FieldInspectorHelper.ShowObjectField<GameObject>("Template", spawnTrigger.template, skin);
                spawnTrigger.count = FieldInspectorHelper.ShowIntField("Quanity", spawnTrigger.count, skin);
                spawnTrigger.useLocation = FieldInspectorHelper.ShowBoolField("Copy Location", spawnTrigger.useLocation, skin);
                spawnTrigger.useRotation = FieldInspectorHelper.ShowBoolField("Copy Rotation", spawnTrigger.useRotation, skin);
                spawnTrigger.parentTo = (ParentTo)FieldInspectorHelper.ShowEnumField("Parent To", spawnTrigger.parentTo, skin);
                FieldInspectorHelper.EndSubSection();
            }
        }
        FieldInspectorHelper.EndSection();
    }
#endif
}
