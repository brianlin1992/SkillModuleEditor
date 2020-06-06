using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_OnCollision : EM_ModuleBase {
    
    public LayerMask collisionMask;
    public bool killOnCollision;
    public float collisionGap
    {
        get { return fl_collisionGap.GetValueByTimeRatio(emitter.timeRatio); }
        set { fl_collisionGap.SetValue(value); }
    }
    public FlexibleFloat fl_collisionGap = new FlexibleFloat(0.5f);
    public List<EmitterTrigger> triggerEmitterList = new List<EmitterTrigger>();
    public List<SpawnGameObjectTrigger> spawnGameObjectList = new List<SpawnGameObjectTrigger>();


    void Awake()
    {
        moduleType = ModuleType.OnCollision;
    }
    public override string GetDisplayName()
    {
        return "On Collision";
    }
    public void OnCollision(SE_SkillObject skillObj, Collider other)
    {
        if (collisionMask != (collisionMask | (1 << other.gameObject.layer)))
        {
            return;
        }
        Debug.Log(gameObject.name + " hit " + other.gameObject);

        if (skillObj.collisionCooldown > 0)
            return;
        skillObj.collisionCooldown = collisionGap;

        if (killOnCollision)
        {
            skillObj.Kill();
        }

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
        collisionMask = FieldInspectorHelper.ShowMaskField("Collide Layer", collisionMask, skin);
        FieldInspectorHelper.ShowFlexibleFloatField("Col. Time Gap", fl_collisionGap, skin);
        killOnCollision = FieldInspectorHelper.ShowBoolField("Destroy On Hit", killOnCollision, skin);
        FieldInspectorHelper.EndSection();

        FieldInspectorHelper.ShowTitle("Spawn GameObject On Collision", skin, true);
        FieldInspectorHelper.StartSection();
        int spawnGameObjectListCount = FieldInspectorHelper.ShowIntField("Spawn Count", spawnGameObjectList.Count, skin);
        if (spawnGameObjectList.Count != spawnGameObjectListCount)
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
