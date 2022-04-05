using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchWithTexture : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture result;

    public Renderer renderer;

    void Start()
    {
        int kernel = computeShader.FindKernel("CSMain");
        result = new RenderTexture(512, 512, 24);
        result.enableRandomWrite=true;
        result.Create();
        
        computeShader.SetTexture(kernel, "Result", result);
        computeShader.Dispatch(kernel, 512/8, 512/8, 1);
        renderer.material.SetTexture("_BaseMap", result);
    }
}
