// ========================================================
// Des：
// Author：叶宜宸的MacBook Air 
// CreateTime：2018/05/19 11:51:16 
// Verson：v 1.0
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

    [CustomPropertyDrawer(typeof(LabelAttribute), false)]
    public class LabelDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string labelStr = (attribute as LabelAttribute).label;
            if(labelStr.StartsWith("$"))
            {
                string fieldName=labelStr.Replace("$", "");
                int pointIndex = property.propertyPath.LastIndexOf('.');
                string path = property.propertyPath.Remove(pointIndex + 1) + fieldName;
                label.text = property.serializedObject.FindProperty(path).stringValue;
            }
            else
            {
                label.text = labelStr;
            }
            EditorGUI.PropertyField(position, property, label);
        }
    }

}
