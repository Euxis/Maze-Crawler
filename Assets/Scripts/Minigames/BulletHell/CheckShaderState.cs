using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckShaderState : MonoBehaviour
{
    [SerializeField] private UniversalAdditionalCameraData cameraData;
    private void OnEnable()
    {
        if (!MediatorScript.instance) return;
        if (MediatorScript.instance.GetShader())
        {
            cameraData.SetRenderer(0);
        }
        else
        {
            cameraData.SetRenderer(1);
        }
    }

    // 1: disable post processing
    // 0: post processing
    public void ManualUpdate(bool b)
    {
        if (b)
        {
            cameraData.SetRenderer(0);
        }
        else
        {
            cameraData.SetRenderer(1);
        }
    }
}
