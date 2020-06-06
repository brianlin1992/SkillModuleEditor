using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleWizardTestSkill : MonoBehaviour
{

    public enum SpawnAnchor
    {
        Hand,
        TemplatePosition
    }
    [System.Serializable]
    public class SkillInfo
    {
        public KeyCode key;
        public GameObject skillObj;
        public GameObject initFX;
        public string animTrigger;
        public float maxOffset;
        public SpawnAnchor spawnAnchor;
    }
    public SkillInfo[] skillList;
    public Animator animator;
    public Transform skillSpawnAnchor;
    public Transform enemyAnchor;
    SkillInfo skillToPerform;
    GameObject skillInstance;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (SkillInfo skill in skillList)
        {
            if (Input.GetKeyDown(skill.key))
            {
                skillToPerform = skill;
                animator.SetTrigger(skill.animTrigger);
            }
        }
    }
    public void SpawnMagicInit()
    {
        //Debug.Log("SpawnMagicInit");
    }
    public void SpawnMagicSkill()
    {
        var template = skillToPerform.skillObj;
        skillInstance = Instantiate<GameObject>(template);
        skillInstance.SetActive(true);
        switch (skillToPerform.spawnAnchor)
        {
            case SpawnAnchor.Hand:
                skillInstance.transform.position = skillSpawnAnchor.position + transform.forward * skillToPerform.maxOffset;
                break;
            case SpawnAnchor.TemplatePosition:
                skillInstance.transform.position = template.transform.position;
                break;
            default:
                break;
        }
        skillInstance.transform.forward = transform.forward;


        //}
        if (skillToPerform.initFX != null)
        {
            template = skillToPerform.initFX;
            skillInstance = Instantiate<GameObject>(template);
            skillInstance.transform.position = transform.position;
        }

    }
    public static Vector3 GetRandomLocWithinRadius(Vector3 Origin, float DistanceMax, float DistanceMin)
    {
        Vector3 V = Vector3.zero;
        Vector3 Vmin = Vector3.zero;
        float Angle = 0;
        float newDistanceMin = 0;

        newDistanceMin += (DistanceMax - DistanceMin) / 2;
        Angle = Random.value * Mathf.PI * 2.0f;

        V.x = Mathf.Sin(Angle) * (DistanceMax - newDistanceMin) * Random.value;
        V.z = Mathf.Cos(Angle) * (DistanceMax - newDistanceMin) * Random.value;
        Vmin.x = Mathf.Sin(Angle) * (newDistanceMin);
        Vmin.z = Mathf.Cos(Angle) * (newDistanceMin);
        return Origin + Vmin + V;
    }
    public void StopSkill()
    {
        if (skillInstance != null)
        {
            skillInstance.GetComponent<SE_SkillControl>().StopAllEmitters();
           
        }
    }
}
