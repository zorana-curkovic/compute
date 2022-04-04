using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatchOneDim : MonoBehaviour
{
    public ComputeShader compute;
   
    void Start()
    {
        int kernel = compute.FindKernel("CSMain1");

        ComputeBuffer computeBuffer = new ComputeBuffer(2 * 4, sizeof(int));
        compute.SetBuffer(kernel, "buffer", computeBuffer);

        // Dispatch 2 groups of 4*1*1 threads = 8 threads
        compute.Dispatch(kernel, 2, 1, 1);

        int[] data = new int[8];
        computeBuffer.GetData(data);
        int id = 0;

        System.Text.StringBuilder sb = new System.Text.StringBuilder("", 2*9 + 1);

        for (int g=0; g<2; g++)
        {
            for (int t = 0; t < 4; t++)
            {
                sb.Append($"{data[id]} ");
                id++;
            }
            sb.Append("\n");
        }

        Debug.Log($"<color=yellow>local thread id [x]\n{sb.ToString()}</color>");

        computeBuffer.Release();
        computeBuffer = null;
    }


}
