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
    using UnityEditorInternal;
    using System.Collections.Generic;
    using System.Reflection;

    public class ReorderableDrawer
    {
        List<ReorderableList> listList;
        SerializedObject serializedObject;

        public ReorderableDrawer(SerializedObject serializedObject)
        {
            this.serializedObject = serializedObject;
            listList = new List<ReorderableList>();
            FieldInfo[] infos = serializedObject.targetObject.GetType().GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo item in infos)
            {
                var objs = item.GetCustomAttributes(typeof(ReorderableAttribute), true);
                if (objs.Length > 0)
                {
                    ReorderableList newList = new ReorderableList(serializedObject, serializedObject.FindProperty(item.Name), true, true, true, true);

                    newList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                    {
                        //绘制编辑区域
                        SerializedProperty itemData = newList.serializedProperty.GetArrayElementAtIndex(index);

                        rect.y += 2;
                        rect.height = EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, itemData, GUIContent.none);

                    };

                    newList.drawHeaderCallback = (Rect rect) =>
                    {
                        //标题
                        GUI.Label(rect, item.Name);
                    };

                    newList.onRemoveCallback = (ReorderableList list) =>
                    {
                        //删除元素时的回调                   
                        ReorderableList.defaultBehaviours.DoRemoveButton(list);
                    };

                    newList.onAddCallback = (ReorderableList list) =>
                    {
                        //添加元素时的回调
                        ReorderableList.defaultBehaviours.DoAddButton(list);
                    };

                    listList.Add(newList);
                }
            }
        }

        public void OnInpectorGUI()
        {
            serializedObject.Update();
            foreach (ReorderableList item in listList)
            {
                item.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}
