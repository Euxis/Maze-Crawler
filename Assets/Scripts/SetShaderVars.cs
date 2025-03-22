using System;
using UnityEngine;

public class SetShaderVars : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature feature;

    private const float DEFAULT_CHROMATIC_VAL = 0.002f;
    // Sets the variables for the shaders
    
    public void SetChromatic(float val)
    {
        feature.passMaterial.SetFloat("_Blur_Offset", val);
    }

    public void ResetChromatic()
    {
        feature.passMaterial.SetFloat("_Blur_Offset", DEFAULT_CHROMATIC_VAL);
    }
}
