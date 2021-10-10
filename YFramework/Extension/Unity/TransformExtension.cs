// ========================================================
// Des：Transform操作工具
// Author：yeyichen
// CreateTime：2018/05/19 10:53:07 
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

namespace YFramework.Extension
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TransformExtension
    {
        /// <summary>
        /// 得到所有子物体的Transform,深度遍历
        /// </summary>
        /// <returns>The all children.</returns>
        /// <param name="target">Target.</param>
        public static List<Transform> FindAllChildren(this Transform target)
        {
            List<Transform> list = new List<Transform>();
            list = FindAllChildren(target, list);
            return list;
        }

        //递归添加所有子物体
        static List<Transform> FindAllChildren(this Transform target, List<Transform> list)
        {
            foreach (Transform item in target)
            {
                list.Add(item);
                FindAllChildren(item, list);
            }
            return list;
        }

        /// <summary>
        /// 得到所有父物体的Transform
        /// </summary>
        /// <returns>The all parent.</returns>
        /// <param name="target">Target.</param>
        public static List<Transform> FindAllParent(this Transform target)
        {
            Transform parent = target.parent;
            List<Transform> parentList = new List<Transform>();

            while (parent != null)
            {
                parentList.Add(parent);
                parent = parent.parent;
            }
            return parentList;

        }

        /// <summary>
        /// 在子物体中得到第一个T类型的组件,深度遍历
        /// </summary>
        /// <returns>The first component in children.</returns>
        /// <param name="target">Target.</param>
        /// <param name="includeSelf">If set to <c>true</c> include self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T FindFirstComponentInChildren<T>(this Transform target, bool includeSelf=true) where T : Component
        {
            T t = null;

            if (includeSelf)
            {
                t = target.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
            }

            foreach (Transform item in target)
            {
                T component = item.GetComponent<T>();
                if (component != null)
                {
                    t = component;
                    break;
                }
                else
                {
                    t = FindFirstComponentInChildren<T>(item, true);

                    if (t != null)
                    {
                        break;
                    }
                }
            }
            return t;
        }

        /// <summary>
        /// 得到子物体中所有T类型的组件,深度遍历
        /// </summary>
        /// <returns>The all components in children.</returns>
        /// <param name="target">Target.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> FindAllComponentsInChildren<T>(this Transform target) where T : Component
        {
            List<Transform> allChildren = FindAllChildren(target);
            List<T> tList = new List<T>();
            foreach (Transform item in allChildren)
            {
                T t = item.GetComponent<T>();
                if (t != null)
                {
                    tList.Add(t);
                }
            }
            return tList;
        }

        /// <summary>
        /// 在父物体中得到第一个T类型
        /// </summary>
        /// <returns>The first component in parent.</returns>
        /// <param name="target">Target.</param>
        /// <param name="includeSelf">If set to <c>true</c> include self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T FindFirstComponentInParent<T>(this Transform target, bool includeSelf=true) where T : Component
        {
            T t = null;

            if (!includeSelf)
            {
                target = target.parent;
            }

            while (target != null)
            {
                t = target.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
                else
                {
                    target = target.parent;
                }
            }
            return t;
        }

        /// <summary>
        /// 在父物体中得到所有T类型的组件
        /// </summary>
        /// <returns>The all components in parents.</returns>
        /// <param name="target">Target.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> FindAllComponentsInParents<T>(this Transform target) where T : Component
        {
            List<Transform> allParent = FindAllParent(target);
            List<T> tList = new List<T>();
            foreach (Transform item in allParent)
            {
                T t = item.GetComponent<T>();
                if (t != null)
                {
                    tList.Add(t);
                }
            }
            return tList;
        }

        /// <summary>
        /// 找到所有特定tag的子物体
        /// </summary>
        /// <returns>The children with tag.</returns>
        /// <param name="target">Target.</param>
        /// <param name="tag">Tag.</param>
        /// <param name="includeSelf">If set to <c>true</c> include self.</param>
        public static List<Transform> FindChildrenWithTag(this Transform target, string tag, bool includeSelf = true)
        {
            List<Transform> childrenList = FindAllChildren(target);
            List<Transform> result = new List<Transform>();
            if (includeSelf && target.CompareTag(tag))
            {
                result.Add(target);
            }

            foreach (Transform item in childrenList)
            {
                if (item.CompareTag(tag))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 找到所有有特定组件的子物体
        /// </summary>
        /// <returns>The children with component.</returns>
        /// <param name="target">Target.</param>
        /// <param name="includeSelf">If set to <c>true</c> include self.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<Transform> FindChildrenWithComponent<T>(this Transform target, bool includeSelf = true) where T : Component
        {
            List<Transform> result = new List<Transform>();
            if (includeSelf && target.GetComponent<T>() != null)
            {
                result.Add(target);
            }

            List<T> list = FindAllComponentsInChildren<T>(target);
            foreach (T item in list)
            {
                result.Add(item.transform);
            }
            return result;
        }

        /// <summary>
        /// 找到所有特定名字的子物体
        /// </summary>
        /// <returns>The children with name.</returns>
        /// <param name="target">Target.</param>
        /// <param name="str">String.</param>
        public static List<Transform> FindAllChildrenWithName(this Transform target, string str, bool includeSelf=true)
        {
            List<Transform> result = new List<Transform>();
            if (includeSelf && target.name == str)
            {
                result.Add(target);
            }

            List<Transform> list = FindAllChildren(target);
            foreach (Transform item in list)
            {
                if (item.name == str)
                {
                    result.Add(item.transform);
                }
            }
            return result;
        }

        /// <summary>
        /// 找到所有包含特定名字的子物体
        /// </summary>
        /// <returns>The children contains name.</returns>
        /// <param name="target">Target.</param>
        /// <param name="str">String.</param>
        /// <param name="includeSelf">If set to <c>true</c> include self.</param>
        public static List<Transform> FindAllChildrenContainsName(this Transform target, string str, bool includeSelf=true)
        {
            List<Transform> result = new List<Transform>();
            if (includeSelf && target.name.Contains(str))
            {
                result.Add(target);
            }

            List<Transform> list = FindAllChildren(target);
            foreach (Transform item in list)
            {
                if (item.name.Contains(str))
                {
                    result.Add(item.transform);
                }
            }
            return result;
        }

        public static Transform FindFirstChildrenWithName(this Transform target, string str)
        {
            var childTrans = target.Find(str);

            if (null != childTrans)
                return childTrans;

            foreach (Transform trans in target)
            {
                childTrans = trans.FindFirstChildrenWithName(str);

                if (null != childTrans)
                    return childTrans;
            }

            return null;
        }

        #region Identity

        public static Transform SetToILocalPosition_L(this Transform target)
        {
            target.localPosition = Vector3.zero;
            return target;
        }

        public static Transform SetToILocalRotation_L(this Transform target)
        {
            target.localRotation = Quaternion.identity;
            return target;
        }

        public static Transform SetToILocalScale_L(this Transform target)
        {
            target.localScale = Vector3.one;
            return target;
        }

        public static Transform SetToILocal_L(this Transform target)
        {
            SetToILocalScale_L(target);
            SetToILocalPosition_L(target);
            SetToILocalRotation_L(target);
            return target;
        }


        public static Transform SetToIWorldPosition_L(this Transform target)
        {
            target.position = Vector3.zero;
            return target;
        }

        public static Transform SetToIWorldRotation_L(this Transform target)
        {
            target.rotation = Quaternion.identity;
            return target;
        }
        #endregion

        #region Position

        public static Transform SetLocalPos_L(this Transform target, Vector3 val)
        {
            target.transform.localPosition = val;
            return target;
        }

        public static Transform SetLocalPosX_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localPosition;
            temp.x = val;
            target.transform.localPosition = temp;
            return target;
        }

        public static Transform SetLocalPosY_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localPosition;
            temp.y = val;
            target.transform.localPosition = temp;
            return target;
        }

        public static Transform SetLocalPosZ_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localPosition;
            temp.z = val;
            target.transform.localPosition = temp;
            return target;
        }


        public static Transform SetWorldPos_L(this Transform target, Vector3 val)
        {
            target.transform.position = val;
            return target;
        }

        public static Transform SetWorldPosX_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.position;
            temp.x = val;
            target.transform.position = temp;
            return target;
        }

        public static Transform SetWorldPosY_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.position;
            temp.y = val;
            target.transform.position = temp;
            return target;
        }

        public static Transform SetWorldPosZ_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.position;
            temp.z = val;
            target.transform.position = temp;
            return target;
        }
        #endregion


        #region Scale

        public static Transform SetLocalScaleX_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localScale;
            temp.x = val;
            target.transform.localScale = temp;
            return target;
        }

        public static Transform SetLocalScaleY_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localScale;
            temp.y = val;
            target.transform.localScale = temp;
            return target;
        }

        public static Transform SetLocalScaleZ_L(this Transform target, float val)
        {
            Vector3 temp = target.transform.localScale;
            temp.z = val;
            target.transform.localScale = temp;
            return target;
        }
        #endregion

        /// <summary>
        /// 设置父物体
        /// </summary>
        /// <returns>The parent l.</returns>
        /// <param name="target">Transform.</param>
        /// <param name="parentTransform">Parent transform.</param>
        public static Transform SetParent_L(this Transform target, Transform parentTransform)
        {
            target.SetParent(parentTransform);
            return target;
        }

        /// <summary>
        /// 通过路径设置父物体
        /// </summary>
        /// <returns>The transform l.</returns>
        /// <param name="target">Transform.</param>
        /// <param name="path">Path.</param>
        public static Transform SetParent_L(this Transform target,string path)
        {
            target.SetParent(path.ToTransform());
            return target;
        }

        /// <summary>
        /// 销毁所有子物体
        /// </summary>
        /// <returns>The all child.</returns>
        /// <param name="target">Target.</param>
        public static Transform DestroyAllChild_L(this Transform target)
        {
            target.ForEachChildReverse(item => item.DestroySelf(true));
            return target;
        }

        /// <summary>
        /// 将fromTrans的数据拷贝到target上
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="fromTrans">From trans.</param>
        public static Transform CopyDataFromOther_L(this Transform target, Transform fromTrans)
        {
            Transform oriParent = target.parent;

            target.SetParent(fromTrans.parent);
            target.localPosition = fromTrans.localPosition;
            target.localRotation = fromTrans.localRotation;
            target.localScale = fromTrans.localScale;
            target.SetParent(oriParent);

            return target;
        }

        /// <summary>
        /// 在Hirechry中的位置,只能用于查找激活的物体
        /// </summary>
        /// <returns>The path.</returns>
        /// <param name="target">Transform.</param>
        public static string GetPath(this Transform target)
        {
            var sb = new System.Text.StringBuilder();
            var t = target;
            while (true)
            {
                sb.Insert(0, t.name);
                t = t.parent;
                if (t)
                {
                    sb.Insert(0, "/");
                }
                else
                {
                    return sb.ToString();
                }
            }
        }

        /// <summary>
        /// 遍历targetTransform的子物体，应用方法
        /// </summary>
        /// <returns>The each child.</returns>
        /// <param name="target">Target.</param>
        /// <param name="action">Action.</param>
        public static Transform ForEachChild(this Transform target, Action<Transform> action)
        {
            for (int i = 0;i<target.childCount ;i++)
            {
                action(target.GetChild(i));
            }
            return target;
        }

        /// <summary>
        /// 逆序遍历targetTransform的子物体，应用方法，
        /// 一般用于销毁所有子物体
        /// </summary>
        /// <returns>The each child reverse.</returns>
        /// <param name="target">Target.</param>
        /// <param name="action">Action.</param>
        public static Transform ForEachChildReverse(this Transform target,Action<Transform> action)
        {
            for (int i = target.childCount - 1; i >= 0;i--)
            {
                action(target.GetChild(i));
            }
            return target;
        }
    }
}
