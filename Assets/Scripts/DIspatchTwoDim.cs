using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  In this example we will observe 2x2 number of groups with 4x4 threads
 *  Application could be a 8x8 pixels image which we want to process in 4x4 blocks
 */
public class DispatchTwoDim : MonoBehaviour
{
    public ComputeShader computeShader;
    private ComputeBuffer dispX;
    private ComputeBuffer dispY;
    private ComputeBuffer groupX;
    private ComputeBuffer groupY;
    private ComputeBuffer flattenedID;
    private ComputeBuffer localXID;
    private ComputeBuffer localYID;

    void Start()
    {
        int dataCount = 2 * 2 * 4 * 4;
        
        dispX = new ComputeBuffer(dataCount, sizeof(int));
        dispY = new ComputeBuffer(dataCount, sizeof(int));
        groupX = new ComputeBuffer(dataCount, sizeof(int));
        groupY = new ComputeBuffer(dataCount, sizeof(int));
        flattenedID = new ComputeBuffer(dataCount, sizeof(int));
        localXID = new ComputeBuffer(dataCount, sizeof(int));
        localYID = new ComputeBuffer(dataCount, sizeof(int));

        int kernel2 = computeShader.FindKernel("CSMain2");

        computeShader.SetBuffer(kernel2, "dispatchXID", dispX);
        computeShader.SetBuffer(kernel2, "dispatchYID", dispY);
        computeShader.SetBuffer(kernel2, "groupXID", groupX);
        computeShader.SetBuffer(kernel2, "groupYID", groupY);
        computeShader.SetBuffer(kernel2, "flattenedID", flattenedID); 
        computeShader.SetBuffer(kernel2, "threadInGroupXID", localXID); 
        computeShader.SetBuffer(kernel2, "threadInGroupYID", localYID); 

        computeShader.Dispatch(kernel2, 2, 2, 1);

        int[] xData = new int[2 * 2 * 4 * 4];
        int[] yData = new int[2 * 2 * 4 * 4];
        int[] groupXData = new int[2 * 2 * 4 * 4];
        int[] groupYData = new int[2 * 2 * 4 * 4];
        int[] flatIDData = new int[2 * 2 * 4 * 4];
        int[] localXData = new int[2 * 2 * 4 * 4];
        int[] localYData = new int[2 * 2 * 4 * 4];

        dispX.GetData(xData);
        dispY.GetData(yData);
        groupX.GetData(groupXData);
        groupY.GetData(groupYData);
        flattenedID.GetData(flatIDData);
        localXID.GetData(localXData);
        localYID.GetData(localYData);

        #region dispatch
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder("", 8*40 + 3);

        int id = 0;
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    sb.Append($"[{xData[id]}, {yData[id]}]");
                    id++;
                }

                sb.Append("    ");
            }

            if (i == 3) sb.Append("\n\n\n");
            else sb.Append("\n");

        }
        Debug.Log($"<color=yellow>Dispatch IDs [x,y] \n{sb.ToString()}</color>");
        
        #endregion

        #region group
        
        sb = new System.Text.StringBuilder("", 8*40 + 3);
        id = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    sb.Append($"[{groupXData[id]}, {groupYData[id]}]");
                    id++;
                }

                sb.Append("    ");
            }
            
            if (i == 3) sb.Append("\n\n\n");
            else sb.Append("\n");

        }
        Debug.Log($"<color=yellow>Group IDs  [x,y]\n{sb.ToString()}</color>");
        
        #endregion
        
        #region flat

        sb = new System.Text.StringBuilder("", 8*40 + 3);
        id = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    sb.Append($"[{flatIDData[id]}]");
                    id++;
                }

                sb.Append("    ");
            }
            
            if (i == 3) sb.Append("\n\n\n");
            else sb.Append("\n");

        }
        Debug.Log($"<color=yellow>Flat IDs  [x]\n{sb.ToString()}</color>");
        
        #endregion
        
        #region local
        
        sb = new System.Text.StringBuilder("", 8 * 40 + 3);
        id = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    sb.Append($"[{localXData[id]}, {localYData[id]}]");
                    id++;
                }

                sb.Append("    ");
            }

            if (i == 3) sb.Append("\n\n\n");
            else sb.Append("\n");

        }

        Debug.Log($"<color=yellow>Local Thread IDs  [x,y]\n{sb.ToString()}</color>");
        
        #endregion
        
        dispX.Release();
        dispY.Release();
        groupX.Release();
        groupY.Release();
        flattenedID.Release();
        
        dispX = null;
        dispY = null;
        groupX = null;
        groupY = null;
        flattenedID = null;
    }
}
