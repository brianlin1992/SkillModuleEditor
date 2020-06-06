using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetSource
{
    SkillControl,
    MouseToWorld
}
public enum CameraType
{
    Main,
    ByTag,
    ByName
}
public class EM_SelectTarget : EM_ModuleBase
{
    public TargetSource targetSource;
    public Transform dummyTarget;
    public Camera ProjectCam;
    public CameraType cameraType;
    public string cameraTag;
    public string cameraName;
    public LayerMask mouseLayerMask;

    Camera customMouseCam;
    void Awake()
    {
        moduleType = ModuleType.SelectTarget;
    }
    void OnDestroy()
    {
        if (dummyTarget != null)
        {
            if (Application.isPlaying)
                Destroy(dummyTarget.gameObject);
            else
                DestroyImmediate(dummyTarget.gameObject);
        }
    }
    void Update()
    {
        switch (targetSource)
        {
            case TargetSource.SkillControl:
                break;
            case TargetSource.MouseToWorld:
                {
                    Ray ray = new Ray();
                    RaycastHit hit;
                    switch (cameraType)
                    {
                        case CameraType.Main:
                            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            break;
                        case CameraType.ByTag:
                        case CameraType.ByName:
                            {
                                if (customMouseCam == null)
                                    refreshCustomMouseCamera();
                                if(customMouseCam != null)
                                    ray = customMouseCam.ScreenPointToRay(Input.mousePosition);
                            }
                            break;
                    }
                    if (Physics.Raycast(ray, out hit, 200f, mouseLayerMask))
                    {
                        repositionDummyTarget(hit.point);
                    }
                }
                break;
            default:
                break;
        }
    }
    void refreshCustomMouseCamera()
    {
        customMouseCam = null;
        GameObject camHolder = null;
        switch (cameraType)
        {
            case CameraType.ByTag:
                if(!string.IsNullOrEmpty(cameraTag))
                    try
                    {
                        camHolder = GameObject.FindGameObjectWithTag(cameraTag);
                    }
                    catch (System.Exception)
                    {
                        //Fail to find camera by Tag   
                    }
                break;
            case CameraType.ByName:
                if (!string.IsNullOrEmpty(cameraName))
                    camHolder = GameObject.Find(cameraName);
                break;
        }
        if (camHolder != null)
            customMouseCam = camHolder.GetComponent<Camera>();
        if (customMouseCam == null)
            Debug.LogError(this.name + " cannot find camera.");
    }
    void repositionDummyTarget(Vector3 toPos)
    {
        if (dummyTarget == null)
            dummyTarget = new GameObject("dummyTarget").transform;
        dummyTarget.parent = transform;
        dummyTarget.position = toPos;
    }
    public override string GetDisplayName()
    {
        return "Select Target";
    }
    public Transform GetSkillTarget()
    {
        switch (targetSource)
        {
            case TargetSource.SkillControl:
                var targetList = emitter.skillControl.targets;
                if (targetList.Count > 0)
                {
                    return targetList[Random.Range(0, targetList.Count)];
                }
                break;
            case TargetSource.MouseToWorld:
                if(dummyTarget != null)
                    return dummyTarget;
                break;
            default:
                break;
        }
        
        return null;
    }
#if UNITY_EDITOR
    public override void DrawnNodeProperties(GUISkin skin)
    {
        base.DrawnNodeProperties(skin);
        FieldInspectorHelper.StartSection();

        targetSource = (TargetSource)FieldInspectorHelper.ShowEnumField("Target Source", targetSource, skin);
        

        FieldInspectorHelper.EndSection();

        switch (targetSource)
        {
            case TargetSource.SkillControl:
                break;
            case TargetSource.MouseToWorld:
                {
                    FieldInspectorHelper.ShowTitle("Mouse To World", skin, false);
                    FieldInspectorHelper.StartSection();
                    mouseLayerMask = FieldInspectorHelper.ShowMaskField("Layer Mask", mouseLayerMask, skin);
                    CameraType oldCamType = cameraType;
                    cameraType = (CameraType)FieldInspectorHelper.ShowEnumField("Camera Type", cameraType, skin);
                    switch (cameraType)
                    {
                        case CameraType.ByTag:
                            cameraTag = FieldInspectorHelper.ShowTextField("Camera Tag", cameraTag, skin);
                            customMouseCam = null;
                            break;
                        case CameraType.ByName:
                            cameraName = FieldInspectorHelper.ShowTextField("Camera Name", cameraName, skin);
                            customMouseCam = null;
                            break;
                        default:
                            break;
                    }
                    FieldInspectorHelper.EndSection();
                }
                break;
            default:
                break;
        }
        switch (targetSource)
        {
            //case SpawnLocationMode.Random:
            //    break;
            //case SpawnLocationMode.LoopByTimeRatio:
            //    break;
            //case SpawnLocationMode.LoopByCount:
            //    {
            //        FieldInspectorHelper.ShowTitle("Loop By Count", skin, false);
            //        FieldInspectorHelper.StartSection();
            //        bUseSpawnTotalCount = FieldInspectorHelper.ShowBoolField("Spawn Count", bUseSpawnTotalCount, skin);
            //        if (!bUseSpawnTotalCount)
            //            unitPerLoop = FieldInspectorHelper.ShowIntField("Unit Per Loop", unitPerLoop, skin);
            //        FieldInspectorHelper.EndSection();
            //    }
            //    break;
            //case SpawnLocationMode.LoopByBrust:
            //    {

            //    }
            //    break;
            //default:
            //    break;
        }
    }

#endif
}