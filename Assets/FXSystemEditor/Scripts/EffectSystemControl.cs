using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class EffectSystemControl : MonoBehaviour {

    public GUIStyleData emitterGUIStyle;
    public GUIStyleData moduleGUIStyle;
    public List<Emitter> emitters = new List<Emitter>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Emitter AddEmitter()
    {
        var go = new GameObject("Emitter " + emitters.Count);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.rotation = new Quaternion();
        var emitter = go.AddComponent<Emitter>();
        emitter.index = emitters.Count;
        emitters.Add(emitter);
        emitter.emitterControl = this;
        Debug.Log("AddEmitter: "+go.name);
        var spawnModule = emitter.AddModule<EM_Spawn>();
        spawnModule.index = 0;
        return emitter;
    }
    public void DeleteEmitter(Emitter emitter)
    {
        emitter.emitterControl = null;
        emitters.Remove(emitter);
        DestroyImmediate(emitter.gameObject);
        RefreshEmittersIndex();
    }
    public void RefreshEmittersIndex()
    {
        for (int i = 0; i < emitters.Count; i++)
        {
            emitters[i].index = i;
        }
    }
}
