// ========================================================
// Des：
// Author：yeyichen
// CreateTime：08/09/2018 09:38:14
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFramework;
using YFramework.Extension;

public class ComputeShaderExample : MonoBehaviour {

    struct VecMatPair
    {
        public Vector3 point;
        public Matrix4x4 matrix;
    }

    public ComputeShader shader;

    [Button]
    public void RunShader()
    {
        int size = 5;
        int time = 2000000;

        //1.初始化输入数据
        VecMatPair[] data = new VecMatPair[size];
        for (int i = 0; i < size;i++)
        {
            data[i].point = new Vector3(i, i, i);
            data[i].matrix = new Matrix4x4(new Vector4(1, 0, 0, 0),
                                         new Vector4(0, 1, 0, 0),
                                         new Vector4(0, 0, 1, 0),
                                         new Vector4(1, 2, 3, 1));
        }

        CodeRunTimeTool GPUTool = new CodeRunTimeTool();
        GPUTool.Begin();

        //2.new缓冲区，设置初始化缓冲区数值
        ComputeBuffer buffer = new ComputeBuffer(data.Length, System.Runtime.InteropServices.Marshal.SizeOf(new VecMatPair()));
        buffer.SetData(data);

        //3.CPU缓冲区写入GPU缓冲区
        int kernel = shader.FindKernel("Multiply");
        shader.SetInt("time", time);
        shader.SetBuffer(kernel, "dataBuffer", buffer);

        //4.计算
        shader.Dispatch(kernel, data.Length, 1, 1);

        //5.接收结果
        VecMatPair[] output = new VecMatPair[size];
        buffer.GetData(output);

        //for (int i = 0; i < n;i++)
        //{
        //    Debug.Log(string.Format("第{0}次原始：{1}", i, data[i].point));

        //    Vector4 temp =  data[i].matrix * new Vector4(data[i].point.x, data[i].point.y, data[i].point.z, 1.0f);
        //    Debug.Log(string.Format("第{0}次应为：{1}", i, new Vector3(temp.x,temp.y,temp.z)));

        //    Debug.Log(string.Format("第{0}次计算后：{1}", i, output[i].point));
        //}
        GPUTool.Stop();




        CodeRunTimeTool CPUTool = new CodeRunTimeTool();
        CPUTool.Begin();
        for (int i = 0; i < size;i++)
        {
            Vector4 temp = new Vector4();
            for (int j = 0; j < time;j++)
            {
                temp=  data[i].matrix * new Vector4(data[i].point.x, data[i].point.y, data[i].point.z, 1.0f);
            }
        }
        CPUTool.Stop();
    }
}
