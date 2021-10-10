// ========================================================
// Des：
// Author：YYC-PC
// CreateTime：9/18/2018 9:06:46 AM
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 2018 YYC-PC
 * 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFramework.Extension;

namespace YFramework
{
    public class SaveWav {

        const int HEADER_SIZE = 44;

        public static void Save(string fileName, string path, AudioClip clip)
        {
            if (!fileName.ToLower().EndsWith(".wav") && !string.IsNullOrEmpty(fileName))
            {
                fileName = fileName + ".wav";
            }

            byte[] headerData = SaveHeader(clip);
            byte[] contentData = SaveContent(clip);
            byte[] result = headerData.Add(contentData);

            FileTool.WriteOrCreateFile(fileName, result);
        }

        //音频内容
        static byte[] SaveContent(AudioClip clip)
        {
            //audioClip中的原始数据
            float[] samples = new float[clip.samples];
            clip.GetData(samples, 0);

            //放大后的数据
            short[] intData = new short[clip.samples];
            //数据转化为byte数据
            byte[] data = new byte[clip.samples * 2];

            samples.Length.ForEach(index =>
            {
                intData[index] = (short)(samples[index] * 32767);
                BitConverter.GetBytes(intData[index]).CopyTo(data,index*2);
            });

            return data;
        }

        //头部
        static byte[] SaveHeader(AudioClip clip)
        {
            byte[] data = new byte[0];
            data = data
                .Add(System.Text.Encoding.UTF8.GetBytes("RIFF"), 4)
                .Add(BitConverter.GetBytes(HEADER_SIZE + clip.samples * 2 - 8), 4)
                .Add(System.Text.Encoding.UTF8.GetBytes("WAVE"), 4)
                .Add(System.Text.Encoding.UTF8.GetBytes("fmt "), 4)
                .Add(BitConverter.GetBytes(16), 4)
                .Add(BitConverter.GetBytes(1), 2)
                .Add(BitConverter.GetBytes(clip.channels), 2)
                .Add(BitConverter.GetBytes(clip.frequency), 4)
                .Add(BitConverter.GetBytes(clip.channels * clip.frequency * 2), 4)
                .Add(BitConverter.GetBytes((ushort)(clip.channels * 2)), 2)
                .Add(BitConverter.GetBytes((ushort)16), 2)
                .Add(System.Text.Encoding.UTF8.GetBytes("data"), 4)
                .Add(BitConverter.GetBytes(clip.samples * clip.channels * 2), 4);

            return data;
        }
    }
}
