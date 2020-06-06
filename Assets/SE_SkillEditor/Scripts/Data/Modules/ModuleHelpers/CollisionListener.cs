using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionListener : MonoBehaviour
{
    public SE_SkillObject skillObj;
    public OnCollisionDelegate OnCollisionDel;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter " + other.gameObject);
        if (OnCollisionDel != null)
            OnCollisionDel(skillObj, other);
    }
}