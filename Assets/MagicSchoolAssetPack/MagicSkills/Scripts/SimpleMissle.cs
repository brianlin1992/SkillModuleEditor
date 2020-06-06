using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMissle : MonoBehaviour {

    public float speed = 3;
    public float lifetime = 3;
	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        transform.transform.position += transform.forward * speed * Time.deltaTime;
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }

    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("hit "+ other.gameObject);
        Destroy(gameObject);
    }
}
