using System;
using UnityEngine;

public class SetShaderVars : MonoBehaviour
{
    [SerializeField] FullScreenPassRendererFeature feature;

    // Sets the variables for the shaders

    // Get needed references
    public void SetChromatic(float val)
    {
        feature.passMaterial.SetFloat("_Blur_Offset", val);
    }

    public void ResetChromatic()
    {
        feature.passMaterial.SetFloat("_Blur_Offset", 0.002f);
    }
}
