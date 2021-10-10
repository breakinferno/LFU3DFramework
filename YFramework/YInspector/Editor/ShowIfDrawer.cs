// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/05/2018 12:07:39
// Version：v 1.1
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using YFramework.Extension;
using System.Reflection;

namespace YFramework
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute), false)]
    public class ShowIfDrawer : PropertyDrawer
    {
        float height = 0;
        bool ifShow = true;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
            ifShow = true;

            ShowIfAttribute attr = attribute as ShowIfAttribute;

            if(attr.name.StartsWith("$"))
            {
                string methodName = attr.name.Replace("$", "");
                ifShow=(bool)property.serializedObject.targetObject.GetType().GetMethod(methodName,BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Invoke(property.serializedObject.targetObject,null);
            }
            else
            {
                string[] conditions=attr.name.TrimAll().Split('&');
                conditions.ForEach_L(item =>
                {
                    bool currentShow = false;

                    bool reverse = item.StartsWith("!");
                    string attrName = item.Replace("!", "");
                    int pointIndex = property.propertyPath.LastIndexOf('.');
                    string path = property.propertyPath.Remove(pointIndex + 1) + attrName;
                    if(reverse)
                    {
                        currentShow = !property.serializedObject.FindProperty(path).boolValue;
                    }
                    else
                    {
                        currentShow = property.serializedObject.FindProperty(path).boolValue;
                    }
                    ifShow = ifShow && currentShow;
                });
            }

            return ifShow ? base.GetPropertyHeight(property, label) : 0;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(ifShow)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}
