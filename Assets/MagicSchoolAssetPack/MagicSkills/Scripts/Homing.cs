using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{

    internal Transform target;
    public float rocketTurnSpeed = 50.0f;
    public float rocketSpeed = 45f;
    public float randomOffset = 0.0f;

    private float timerSinceLaunch_Contor;
    public float objectLifeTimerValue = 10;

    // Use this for initialization
    void Start()
    {
        timerSinceLaunch_Contor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timerSinceLaunch_Contor += Time.deltaTime;

        if (target != null)
        {
            if (timerSinceLaunch_Contor > 1)
            {
                if ((target.position - transform.position).magnitude > 50)
                {
                    randomOffset = 100.0f;
                    rocketTurnSpeed = 90.0f;
                }
                else
                {
                    randomOffset = 5f;
                    //if close to target
                    if ((target.position - transform.position).magnitude < 10)
                    {
                        rocketTurnSpeed = 180.0f;
                    }
                }
            }

            Vector3 direction = target.position - transform.position + Random.insideUnitSphere * randomOffset;
            direction.Normalize();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), rocketTurnSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * rocketSpeed * Time.deltaTime);
        }

        if (timerSinceLaunch_Contor > objectLifeTimerValue)
        {
            Destroy(transform.gameObject, 1);
        }
    }
}