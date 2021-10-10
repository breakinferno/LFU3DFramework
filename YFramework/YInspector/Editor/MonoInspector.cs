// ========================================================
// Des：
// Author：叶宜宸的MacBook Air 
// CreateTime：2018/05/19 11:51:16 
// Version：v 1.0
// ========================================================
/****************************************************************************
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
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    [CanEditMultipleObjects]
    internal class MonoInspector : Editor
    {
        bool ifShow;

        ButtonDrawer buttonAttributeDrawer;

        ReorderableDrawer reorderableDrawer;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            bool showSwitchButton=false;
            Type type = target.GetType();

            //字段上的Attribute
            FieldInfo[] infos = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo item in infos)
            {
                if (Attribute.IsDefined(item, typeof(ReorderableAttribute), true))
                {
                    showSwitchButton = true;
                }
            }

            //方法上的Attribute
            MethodInfo[] infos2=type.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach(MethodInfo item in infos2)
            {
                if(Attribute.IsDefined(item, typeof(ButtonAttribute), true))
                {
                    showSwitchButton = true;
                }
            }

            if(showSwitchButton)
            {
                if (GUILayout.Button("Editor工具开关", GUILayout.Height(40)))
                {
                    ifShow = !ifShow;
                }   

                if (ifShow)
                {
                    if (buttonAttributeDrawer == null)
                    {
                        buttonAttributeDrawer = new ButtonDrawer((MonoBehaviour)target);
                    }
                    buttonAttributeDrawer.OnInspectorGUI();

                    if (reorderableDrawer == null)
                    {
                        reorderableDrawer = new ReorderableDrawer(serializedObject);
                    }
                    reorderableDrawer.OnInpectorGUI();
                }
            }
        }
    }
}
