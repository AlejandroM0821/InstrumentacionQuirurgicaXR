using Qualcomm.Snapdragon.Spaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.OpenXR;

public class PassThrough : MonoBehaviour
{
    private BaseRuntimeFeature runtimeFeature;
    private bool IsPassthroughOn;

    public virtual void OnEnable()
    {
        runtimeFeature = OpenXRSettings.Instance.GetFeature<BaseRuntimeFeature>();
    }

    public void TogglePassthrough()
    {
        IsPassthroughOn = !IsPassthroughOn;
        runtimeFeature.SetPassthroughEnabled(IsPassthroughOn);
    }

    
}
