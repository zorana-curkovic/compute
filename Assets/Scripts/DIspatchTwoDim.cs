using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIspatchTwoDim : MonoBehaviour
{
    public ComputeShader computeShader;
    ComputeBuffer buffer;
    void Start()
    {
        buffer = new ComputeBuffer(2 * 2 * 4 * 4, sizeof(int));
        int kernel1 = computeShader.FindKernel("CSMain1");
        int kernel2 = computeShader.FindKernel("CSMain2");

        computeShader.SetBuffer(kernel2, "buffer2", buffer);
        computeShader.Dispatch(kernel2, 2, 2, 1);

        int[] data = new int[2 * 2 * 4 * 4];
        buffer.GetData(data);

        for (int i=0; i<8; ++i)
        {
            string line="";
            for (int j=0; j<8; ++j)
            {
                line += " " + data[j + i*8];
            }
            Debug.Log(line);

        }

        buffer.Release();
    }
}
