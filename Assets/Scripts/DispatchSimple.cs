using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchSimple : MonoBehaviour
{
    public ComputeShader compute;
   
    void Start()
    {
        int kernel = compute.FindKernel("CSMain");

        ComputeBuffer computeBuffer = new ComputeBuffer(2 * 4, sizeof(int));
        compute.SetBuffer(kernel, "buffer", computeBuffer);

        // Dispatch 2 groups of 4*1*1 threads = 8 threads
        compute.Dispatch(kernel, 2, 1, 1);

        int[] data = new int[8];
        computeBuffer.GetData(data);

        Debug.Log("[Dispatch Simple With Structured:");
        for (int i=0; i<8; ++i)
        {
            Debug.Log(data[i]);
        }

        computeBuffer.Release();
    }


}
