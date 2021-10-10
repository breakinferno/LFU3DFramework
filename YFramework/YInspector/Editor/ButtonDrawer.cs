// ========================================================
// Des：
// Author：yeyichen
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
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;
    using System.Collections.Generic;
    using System;
    using Object = UnityEngine.Object;

    public class ButtonDrawer
    {
        MonoBehaviour target;
        List<MethodInfo> buttonMethods;
        List<ButtonAttribute> buttonAttributes;

        public ButtonDrawer(MonoBehaviour target)
        {
            if (!target)
            {
                return;
            }

            this.target = target;
            MethodInfo[] methodInfos = target.GetType().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (methodInfos.Length <= 0)
            {
                return;
            }

            buttonMethods = new List<MethodInfo>();
            buttonAttributes = new List<ButtonAttribute>();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                MethodInfo buttonMethod = methodInfos[i];
                if (Attribute.IsDefined(buttonMethod, typeof(ButtonAttribute), true))
                {
                    buttonMethods.Add(buttonMethod);
                    ButtonAttribute[] exAttributes = buttonMethod.GetCustomAttributes(typeof(ButtonAttribute), true) as ButtonAttribute[];
                    buttonAttributes.Add(exAttributes[0]);
                }
            }
        }

        public void OnInspectorGUI()
        {
            if (buttonMethods.Count == 0)
            {
                return;
            }

            for (int i = 0; i < buttonMethods.Count; i++)
            {
                MethodInfo methodInfo = buttonMethods[i];
                ButtonAttribute buttonExAttribute = buttonAttributes[i];

                string name = buttonExAttribute.txtButtonName ?? methodInfo.Name;
                if (GUILayout.Button(name))
                {
                    methodInfo.Invoke(target, null);
                }

            }
        }
    }

}
