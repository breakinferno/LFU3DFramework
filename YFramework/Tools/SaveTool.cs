// ========================================================
// Des：
// Author：YYC-PC
// CreateTime：2019/10/31 15:48:28
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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using YFramework.Extension;

namespace YFramework
{

    public enum SaveKey
    {
        a,
        b,
        c,
        d,
        e,
        f
    }

    public class SaveTool :QSingleton<SaveTool> {

        bool ifEncoded = false;
        string fileName = "PlayerData.txt";
        Dictionary<string, string> data = new Dictionary<string, string>();
        Dictionary<string, string> originalData =new Dictionary<string, string>
        {
            {SaveKey.a.ToString(),"b" },
            {SaveKey.b.ToString(),"c" },
            {SaveKey.c.ToString(),"d" },
            {SaveKey.d.ToString(),"e" },
            {SaveKey.e.ToString(),"f" },
            {SaveKey.f.ToString(),"g" },
        };

        private SaveTool()
        {
            if (FileTool.IsFileExists(fileName))
            {
                string content;
                //存在了，旧存档
                if (ifEncoded)
                {
                    byte[] bytes = FileTool.ReadAllByte(fileName);
                    content = Encoding.UTF8.GetString(bytes);

                }
                else
                {
                    content = FileTool.ReadAllString(fileName);
                }
                data = (Dictionary<string, string>)content.ToAnyTypeDic<string, string>();
            }
            else
            {
                //没有，新存档
                string content = originalData.ToContentString();
                data = (Dictionary<string, string>)content.ToAnyTypeDic<string, string>();
                SaveToFile(content);
            }
        }

        public void Set(SaveKey key,string value)
        {
            data[key.ToString()] = value;
        }

        public string Get(SaveKey key)
        {
            return data[key.ToString()];
        }

        public void Save()
        {
            SaveToFile(data.ToContentString());
        }

        void SaveToFile(string content)
        {
            if (ifEncoded)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                FileTool.WriteOrCreateFile(fileName, bytes);
            }
            else
            {
                FileTool.WriteOrCreateFile(fileName, content);
            }
        }
    }
}
