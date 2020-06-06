using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SE_SkillObject : MonoBehaviour {

    public SE_Emitter emitter;
    public Transform skillInstance;
    public Transform target;
    public int brustCount = 0;
    public int brustIndex = 0;

    public GetSpawnLocationDataDelegate GetSpawnLocationData;
    public GetFloatDelegate GetInitLifetime;
    public GetVector3Delegate GetInitSize;
    public GetVector3Delegate GetInitAnchorOffset;
    public GetVector3Delegate GetInitRotation;
    public GetVector3Delegate GetInitRotationRate;
    public GetVector3Delegate GetInitAnchorRotationRate;
    public GetVector3Delegate GetInitVelocity;
    public GetTransformDelegate GetSkillTarget;
    public OnCollisionDelegate OnCollisionReceiver;

    public List<UpdateVector3BySkillObjectDelegate> UpdateAnchorOffsetList = new List<UpdateVector3BySkillObjectDelegate>();
    public List<UpdateVector3BySkillObjectDelegate> UpdateVelocityList = new List<UpdateVector3BySkillObjectDelegate>();
    public List<UpdateRotationBySkillObjectDelegate> UpdateRotationList = new List<UpdateRotationBySkillObjectDelegate>();

    public EM_SpawnLocationData spawnLocationData;
    public float initLifetime;
    public float lifetimeRemain;
    public float lifetimeRemainRatio {
        get { return lifetimeRemain / initLifetime; }
    }
    public Vector3 initSize;
    public Vector3 initRotation;
    public Vector3 initRotRate;
    public Vector3 initAnchorOffset;
    public Vector3 initAnchorRotRate;
    public Vector3 initVelocity;

    public Vector3 velocity;
    public Vector3 anchorOffset;
    Quaternion velocityDir;

    public System.Action<SE_SkillObject> OnInitDone;
    public System.Action<SE_SkillObject> OnLifetimeEnd;
    public System.Action<SE_SkillObject> OnObjectDestroy;

    public float collisionCooldown;

    void Awake()
    {

    }

    // Use this for initialization
    void Start () {

        initAnchorOffset = GetInitAnchorOffset != null ? GetInitAnchorOffset() : Vector3.zero;
        skillInstance.localPosition = initAnchorOffset;
        skillInstance.localRotation = new Quaternion();

        target = GetSkillTarget != null ? GetSkillTarget() : null;

        spawnLocationData = GetSpawnLocationData != null ? GetSpawnLocationData(this) : null;
        if(spawnLocationData != null)
        {
            transform.position = spawnLocationData.Location;
            transform.forward = spawnLocationData.Foward;
            velocityDir = transform.localRotation;
        }
        else
        {
            transform.position = emitter.transform.position;
            transform.forward = emitter.transform.forward;
            velocityDir = emitter.transform.rotation;
        }
        initLifetime = GetInitLifetime != null ? GetInitLifetime() : 1;
        lifetimeRemain = initLifetime;
        initSize = GetInitSize != null ? GetInitSize() : Vector3.one;
        transform.localScale = initSize;
        initRotation = GetInitRotation != null ? GetInitRotation() : Vector3.zero;
        initRotRate = GetInitRotationRate != null ? GetInitRotationRate() : Vector3.zero;
        initAnchorRotRate = GetInitAnchorRotationRate != null ? GetInitAnchorRotationRate() : Vector3.zero;
        skillInstance.localRotation = Quaternion.Euler(initRotation);
        initVelocity = GetInitVelocity != null ? GetInitVelocity() : Vector3.zero;
        velocity = velocityDir * (spawnLocationData != null ? initVelocity : initVelocity);

        if(OnCollisionReceiver != null)
        {
            var collisionListener = skillInstance.gameObject.AddComponent<CollisionListener>();
            collisionListener.skillObj = this;
            collisionListener.OnCollisionDel = OnCollisionReceiver;
        }
        if (OnInitDone != null)
            OnInitDone(this);
        UpdateModifierOverTime(0);
    }

    public void UpdateTime(float deltaTime)
    {
        if (initLifetime <= 0)
            return;

        if (lifetimeRemain > 0)
            lifetimeRemain -= deltaTime;
        else
        {
            Kill();
            return;
        }

        if (collisionCooldown > 0)
            collisionCooldown -= deltaTime;

        UpdateModifierOverTime(deltaTime);


    }
    void UpdateModifierOverTime(float deltaTime)
    {
        for (int i = 0; i < UpdateAnchorOffsetList.Count; i++)
        {
            var updateAnchorOffsetDel = UpdateAnchorOffsetList[i];
            anchorOffset = updateAnchorOffsetDel(this, deltaTime);
        }
        skillInstance.localPosition = anchorOffset;

        for (int i = 0; i < UpdateVelocityList.Count; i++)
        {
            var updateVelDel = UpdateVelocityList[i];
            velocity = updateVelDel(this, deltaTime);
        }
        this.transform.localPosition += velocity * deltaTime;

        for (int i = 0; i < UpdateRotationList.Count; i++)
        {
            var updateRotationDel = UpdateRotationList[i];
            updateRotationDel(this, deltaTime);
        }

        transform.Rotate(initAnchorRotRate, Space.Self);
        skillInstance.Rotate(initRotRate, Space.Self);
    }
    public void Kill()
    {
        lifetimeRemain = 0;
        if (OnLifetimeEnd != null)
            OnLifetimeEnd(this);
    }
    public void DestroyTrigger()
    {
        if (OnObjectDestroy != null)
            OnObjectDestroy(this);
    }
}
