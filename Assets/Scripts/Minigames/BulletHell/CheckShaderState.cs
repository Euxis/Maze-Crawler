using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckShaderState : MonoBehaviour
{
    [SerializeField] private UniversalAdditionalCameraData cameraData;
    private void OnEnable()
    {
        if (!MediatorScript.instance) return;
        if (!MediatorScript.instance.GetShader())
        {
            cameraData.SetRenderer(0);
        }
        else
        {
            cameraData.SetRenderer(1);
        }
    }

    public void ManualUpdate(bool b)
    {
        if (b)
        {
            cameraData.SetRenderer(1);
        }
        else
        {
            cameraData.SetRenderer(0);
        }
    }
}
