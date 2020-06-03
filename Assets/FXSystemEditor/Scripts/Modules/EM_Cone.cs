using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EM_Cone : Module
{
    public float Angle = 25;
    public float Radius = 1;
    public float Length = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override string GetDisplayName()
    {
        return "Cone";
    }
}
