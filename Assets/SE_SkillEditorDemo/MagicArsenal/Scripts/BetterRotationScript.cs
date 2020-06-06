using UnityEngine;
using System.Collections;

public class BetterRotationScript : MonoBehaviour {

    public float degreesPerSecond = 120;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime);
	}
}
