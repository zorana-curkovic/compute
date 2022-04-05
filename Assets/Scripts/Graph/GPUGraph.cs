using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GPUGraph : MonoBehaviour {

    //[SerializeField]
    //Transform pointPrefab;

    [SerializeField, Range(10, 200)] private int resolution = 10;

    [SerializeField] private FunctionLibrary.FunctionName function;

    [SerializeField] private ComputeShader computeShader;
    
    [SerializeField] Material material;

    [SerializeField] Mesh mesh;
    
    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode = TransitionMode.Cycle;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    //Transform[] points;

    float duration;

    bool transitioning;

    FunctionLibrary.FunctionName transitionFunction;

    private ComputeBuffer positionsBuffer;

    private static int positionsId = Shader.PropertyToID("_Positions");
    private static int stepId = Shader.PropertyToID("_Step");
    private static int resolutionId = Shader.PropertyToID("_Resolution");
    private static int timeId = Shader.PropertyToID("_Time");

    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetBuffer(0, positionsId, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);
        
        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);
        
        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f/ resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }
    
    void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void Update()
    {
        // duration += Time.deltaTime;
        // if (duration >= functionDuration) {
        //     duration -= functionDuration;
        //     PickNextFunction();
        // }

        UpdateFunctionOnGPU();
     
    }

    void PickNextFunction () 
    {  
        function = transitionMode == TransitionMode.Cycle ?
        FunctionLibrary.GetNextFunctionName(function) :
        FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }

    //void UpdateFunction () { }

    //void UpdateFunctionTransition () {  }
}