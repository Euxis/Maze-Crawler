using System;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ShaderSetting : MonoBehaviour
{
    [SerializeField] private UniversalAdditionalCameraData cameraData;
    
    private bool isEnabled = true;

    /// <summary>
    /// Toggles shaders and updates the other minigames on setting.
    /// </summary>
    /// <param name="context"></param>
    public void ToggleShaders(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isEnabled)
            {
                isEnabled = false;
                cameraData.SetRenderer(1);
                MediatorScript.instance.SetShader(false);
            }
            else
            {
                isEnabled = true;
                cameraData.SetRenderer(0);
                MediatorScript.instance.SetShader(true);
            }
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
