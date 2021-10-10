// ========================================================
// Des：editor上的一些设置，包括代码模板、debug的代码跟踪
// Author：yeyichen
// CreateTime：06/05/2018 22:53:05
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 2016 Health
 * Copyright (c) 2018 yeyichen
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

namespace YFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    public class DebugRepath
    {

        #region 自包装debug的代码行定位
        [UnityEditor.Callbacks.OnOpenAsset(0)]
        static bool OnOpenAsset(int instanceID, int line)
        {
            string stackTrace = GetStackTrace();
            if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("UnityEngine.Debug:Log"))
            {
                Match matches = Regex.Match(stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                string pathLine = "";
                while (matches.Success)
                {
                    pathLine = matches.Groups[1].Value;

                    if (!pathLine.Contains("ObjectExtension.cs"))
                    {
                        int splitIndex = pathLine.LastIndexOf(":");
                        string path = pathLine.Substring(0, splitIndex);
                        line = Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullPath = fullPath + path;
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath, line);
                        break;
                    }
                    matches = matches.NextMatch();
                }
                return true;
            }
            return false;
        }

        static string GetStackTrace()
        {
            Type consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            var consoleWindowInstance = fieldInfo.GetValue(null);
            if (consoleWindowInstance != null)
            {
                object focusedWindow = (object)EditorWindow.focusedWindow;
                if (consoleWindowInstance == focusedWindow)
                {
                    var listViewStateType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ListViewState");
                    fieldInfo = consoleWindowType.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
                    var listView = fieldInfo.GetValue(consoleWindowInstance);
                    fieldInfo = listViewStateType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
                    int row = (int)fieldInfo.GetValue(listView);
                    fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
                    return activeText;
                }
            }
            return null;
        }
        #endregion

     

    }

    public class ChangeScriptTemplate:UnityEditor.AssetModificationProcessor
    {
        #region 代码模板

        private static readonly Dictionary<string, string> deviceNameToPersonName = new Dictionary<string, string>()
        {
            {"叶宜宸的MacBook Air","yeyichen"}
        };

        //创建资源调用
        public static void OnWillCreateAsset(string path)
        {
            // 只修改C#脚本
            path = path.Replace(".meta", "");
            if (path.ToLower().EndsWith(".cs"))
            {
                string content = File.ReadAllText(path);
                if (content.Contains("#NewScriptTemplate#"))
                {
                    content = content.Replace("#NewScriptTemplate#", "");
                    content = content.Replace("#Author#", deviceNameToPersonName.ContainsKey(SystemInfo.deviceName) ? deviceNameToPersonName[SystemInfo.deviceName] : SystemInfo.deviceName);
                    content = content.Replace("#Time#", System.DateTime.Now.ToString());

                    File.WriteAllText(path, content);
                }
            }
        }
        #endregion
    }
                                                 
}
