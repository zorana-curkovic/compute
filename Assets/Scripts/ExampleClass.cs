using UnityEngine;
using UnityEngine.Rendering;

public class ExampleClass : MonoBehaviour
{
    public Material material;
    public Mesh mesh;
    public ComputeShader argumentsShader;
    
    GraphicsBuffer _indirectDataBuffer;
    GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
    const int commandCount = 2;

    void Start()
    {
        _indirectDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandCount, GraphicsBuffer.IndirectDrawIndexedArgs.size);
        commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[commandCount];
    }

    void OnDestroy()
    {
        _indirectDataBuffer?.Release();
        _indirectDataBuffer = null;
    }

    void Update()
    {
        RenderParams rp = new RenderParams(material);
        rp.worldBounds = new Bounds(Vector3.zero, 10000 * Vector3.one); // use tighter bounds for better FOV culling
        rp.matProps = new MaterialPropertyBlock();
        rp.matProps.SetMatrix("_ObjectToWorld", Matrix4x4.Translate(new Vector3(-4.5f, 0, 0)));
        //commandData[0].indexCountPerInstance = mesh.GetIndexCount(0);
        //commandData[0].instanceCount = 10;
        //commandData[1].indexCountPerInstance = mesh.GetIndexCount(0);
        //commandData[1].instanceCount = 10;
        _indirectDataBuffer.SetData(commandData);

        
        int computeKernel = argumentsShader.FindKernel("Clear");

        CommandBuffer cmd = new CommandBuffer();
        cmd.name = "GenerateCubes";
        cmd.SetComputeBufferParam(argumentsShader, computeKernel, "indirect_data_buffer", _indirectDataBuffer);
        cmd.DispatchCompute(argumentsShader, computeKernel, 2, 1, 1);
        Graphics.ExecuteCommandBuffer(cmd);

        Graphics.RenderMeshIndirect(rp, mesh, _indirectDataBuffer, commandCount);
    }
}