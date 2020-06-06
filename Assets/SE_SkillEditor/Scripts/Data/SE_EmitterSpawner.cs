using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public partial class SE_Emitter {

    #region Public variables
    [Header("EmitterParams")]
    public GameObject template;
    public float duration = 3;
    public bool looping = true;
    public float startDelay = 0;
    public SpaceType simulationSpace = SpaceType.World;
    #endregion

    #region Protected Variables
    public List<SE_SkillObject> emittedObjList = new List<SE_SkillObject>();
    public float time;
    public float emitTimer;
    public float timeRatio
    {
        get { return time / duration; }
    }
    public int spawnCount;
    #endregion

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (skillControl.previewSkill == false && !Application.isPlaying)
            return;
#endif
        if (template == null) return;

        if(time > duration)
        {
            if (looping)
            {
                spawnCount = 0;
                time = 0;
                emitTimer = 0;
            }
        }
        EmitBrust();

        if (time <= duration)
        {
            time += Time.deltaTime;
            emitTimer += Time.deltaTime;
        }
        //Update Skill Object Time List
        for (int i = 0; i < emittedObjList.Count; i++)
        {
            var obj = emittedObjList[i];
            obj.UpdateTime(Time.deltaTime);
        }
        //Spawn emit object
        Emit();
    }


    #region Main Methods
    public void Emit()
    {
        var m_spawn = GetComponent<EM_Spawn>();

        if (m_spawn == null)
            return;
        if (m_spawn.rateOverTime <= 0)
            return;

        float rate = 1 / m_spawn.rateOverTime;
        while (emitTimer >= rate)
        {
            emitTimer -= rate;
            GameObject emittedInstance = Instantiate<GameObject>(template);
            SetupSkillObj(emittedInstance);
        }
    }
    public void EmitBrust()
    {
        var m_spawn = GetComponent<EM_Spawn>();

        if (m_spawn == null)
            return;
        if (m_spawn.brustList.Count == 0)
            return;
        for (int i = 0; i < m_spawn.brustList.Count; i++)
        {
            BrustData brust = m_spawn.brustList[i];
            brust.updateTime(time, (spawnCount) =>
            {
                for (int j = 0; j < spawnCount; j++)
                {
                    GameObject emittedInstance = Instantiate<GameObject>(template);
                    SetupSkillObj(emittedInstance, spawnCount, j);
                }
            });
        }
    }
    public void StopEmitter()
    {
        looping = false;
        time = duration;

    }
    protected void SetupSkillObj(GameObject emittedInstance,
        int? brustCount = null,
        int? brustIndex = null)
    {
        GameObject skillObjHolder = new GameObject();
        SE_SkillObject skillObj = skillObjHolder.AddComponent<SE_SkillObject>();
        emittedObjList.Add(skillObj);

        emittedInstance.transform.parent = skillObjHolder.transform;
        skillObj.skillInstance = emittedInstance.transform;
        skillObj.emitter = this;
        skillObj.brustCount = brustCount == null ? 0 : brustCount.Value;
        skillObj.brustIndex = brustIndex == null ? 0 : brustIndex.Value;

        switch (simulationSpace)
        {
            case SpaceType.Local:
                skillObjHolder.transform.parent = this.transform;
                break;
            case SpaceType.World:
                break;
        }

        skillObj.OnInitDone = OnSkillObjectInitDone;
        skillObj.OnLifetimeEnd = OnSkillObjectLifetimeEnd;

        //select target module delegate
        var m_selectTarget = GetComponent<EM_SelectTarget>();
        if (m_selectTarget != null) skillObj.GetSkillTarget = m_selectTarget.GetSkillTarget;

        //skill object transform
        var m_spawnLocation = GetComponent<EM_SpawnLocation>();
        if (m_spawnLocation == null)
        {
            emittedInstance.transform.position = transform.position;
            emittedInstance.transform.rotation = transform.rotation;
        }
        else
        {
            skillObj.GetSpawnLocationData = m_spawnLocation.GetSpawnLocation;
        }
        //Lifetime module delegate
        var m_lifetime = GetComponent<EM_Lifetime>();
        if (m_lifetime != null) skillObj.GetInitLifetime = m_lifetime.GetInitLifetime;

        //Anchor Offset module delegate
        var m_initAnchorOffset = GetComponent<EM_InitialAnchorOffset>();
        if (m_initAnchorOffset != null) skillObj.GetInitAnchorOffset = m_initAnchorOffset.GetInitAnchorOffset;

        //Initial Size module delegate
        var m_initSize = GetComponent<EM_InitialSize>();
        if (m_initSize != null) skillObj.GetInitSize = m_initSize.GetInitSize;

        //Initial Rotation module delegate
        var m_initRotation = GetComponent<EM_InitialRotation>();
        if (m_initRotation != null) skillObj.GetInitRotation = m_initRotation.GetInitRotation;

        //Initial Rotation Rate module delegate
        var m_initRotRate = GetComponent<EM_InitialRotRate>();
        if (m_initRotRate != null) skillObj.GetInitRotationRate = m_initRotRate.GetInitRotationRate;
        var m_initAnchorRotRate = GetComponent<EM_InitialAnchorRotRate>();
        if (m_initAnchorRotRate != null) skillObj.GetInitAnchorRotationRate = m_initAnchorRotRate.GetInitAnchorRotationRate;

        //Initial Velocity module delegate
        var m_initVelocity = GetComponent<EM_InitialVelocity>();
        if (m_initVelocity != null) skillObj.GetInitVelocity = m_initVelocity.GetInitVelocity;

        //On Collision module delegate
        var m_onCollision = GetComponent<EM_OnCollision>();
        if (m_onCollision != null) skillObj.OnCollisionReceiver = m_onCollision.OnCollision;

        //On Collision module delegate
        var m_onDestroy = GetComponent<EM_OnDestroy>();
        if (m_onDestroy != null) skillObj.OnObjectDestroy = m_onDestroy.OnObjectDestroy;

        /**
         * Update Module
         */
        //Anchor Offset Update Delegate
        var m_AnchorOffsetList = new List<EM_AnchorOffsetModifier>(GetComponents<EM_AnchorOffsetModifier>());
        m_AnchorOffsetList.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        for (int i = 0; i < m_AnchorOffsetList.Count; i++)
        {
            skillObj.UpdateAnchorOffsetList.Add(m_AnchorOffsetList[i].UpdateAnchorOffset);
        }
        //Velocity Update Delegate
        var m_VelocityList = new List<EM_VelocityModifier>(GetComponents<EM_VelocityModifier>());
        m_VelocityList.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        for (int i = 0; i < m_VelocityList.Count; i++)
        {
            skillObj.UpdateVelocityList.Add(m_VelocityList[i].UpdateVelocity);
        }
        
        //Rotation Update Delegate
        var m_RotationList = new List<EM_RotationModifier>(GetComponents<EM_RotationModifier>());
        m_RotationList.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        for (int i = 0; i < m_RotationList.Count; i++)
        {
            skillObj.UpdateRotationList.Add(m_RotationList[i].UpdateRotation);
        }
    }
    public void ResetEmitter()
    {
        time = 0;
        emitTimer = 0;
        for (int i = 0; i < emittedObjList.Count; i++)
        {
            if(emittedObjList[i] != null)
                DestroyImmediate(emittedObjList[i].gameObject);
        }
        emittedObjList.Clear();
    }
    #endregion

    #region Utility Methods
    protected void OnSkillObjectInitDone(SE_SkillObject obj)
    {
        spawnCount++;
    }
    protected void OnSkillObjectLifetimeEnd(SE_SkillObject obj)
    {
        emittedObjList.Remove(obj);
        obj.DestroyTrigger();
        if (Application.isPlaying)
            Destroy(obj.gameObject);
        else
            DestroyImmediate(obj.gameObject);
    }
    #endregion

}
