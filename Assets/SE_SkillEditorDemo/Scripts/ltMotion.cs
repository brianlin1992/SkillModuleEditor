using UnityEngine;
using System.Collections;
/**
 * Set gameobject to translate/rotate constantly over time
 */
[ExecuteInEditMode]
public class ltMotion : MonoBehaviour {

    public enum MotionType
    {
        Rotate,
        Translate
    }
    public enum MotionDirection
    {
        Up, Down, Left, Right, Forward, Back
    }
    public bool active = false;
    public bool bActiveOnStart = true;
    public MotionType motionType;
    public MotionDirection motionDir;
    public float speed = 1;
    
	// Use this for initialization
	void Start () {
        if (bActiveOnStart) active = true;
        lerpTime = 0;

    }
	
	// Update is called once per frame
	void Update () {
        if (!active)
            return;
		if (Application.isPlaying) {
			if (bSpeedLerp) {
				lerpTime += Time.deltaTime;
			}
			switch (motionType) {
			case MotionType.Rotate:
				gameObject.transform.Rotate (getDirectionVector (motionDir) * (bSpeedLerp ? lerpSpeed : speed) * Time.deltaTime);
				break;
			case MotionType.Translate:
				gameObject.transform.Translate (getDirectionVector (motionDir) * (bSpeedLerp ? lerpSpeed : speed) * Time.deltaTime);
				break;
			}
		} else
			return;
	}
    Vector3 getDirectionVector(MotionDirection dir)
    {
        switch (dir)
        {
            case MotionDirection.Up: return Vector3.up;
            case MotionDirection.Down: return Vector3.down;
            case MotionDirection.Left: return Vector3.left;
            case MotionDirection.Right: return Vector3.right;
            case MotionDirection.Back: return Vector3.back;
            case MotionDirection.Forward: return Vector3.forward;
        }
        return Vector3.zero;
    }
    [Header("Speed Lerp")]
    public bool bSpeedLerp;
    public float beginningSpeed;
    public float lerpDuration = 1;
    float lerpTime;
    float lerpSpeed
    {
        get {
            return Mathf.Lerp(beginningSpeed, speed, MyEase.easeTween(MyEaseType.easeOutQuart, Mathf.Clamp(lerpTime, 0, lerpDuration) / lerpDuration,0,1,1));
        }
    }
    public void ReActivateLerp()
    {
        lerpTime = 0;
    }

}
