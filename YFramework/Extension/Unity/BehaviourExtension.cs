// ========================================================
// Des：
// Author：yeyichen
// CreateTime：2018/05/27 15:24:38 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 2017 liangxie
 * Copyright (c) 2018 liangxie
 * Copyright (c) 2018 yeyichen
 * 
 * http://qframework.io
 * https://github.com/liangxiegame/QFramework
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
    using UnityEngine;

    public static class BehaviourExtension
    {
        public static void Example()
        {
            var gameObject = new GameObject();
            var component = gameObject.GetComponent<MonoBehaviour>();

            component.Enable_L(); 
            component.Disable_L();
        }

        /// <summary>
        /// enabled=true
        /// </summary>
        /// <returns>The enable.</returns>
        /// <param name="selfBehaviour">Self behaviour.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Enable_L<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = true;
            return selfBehaviour;
        }

        /// <summary>
        /// enabled=false
        /// </summary>
        /// <returns>The disable.</returns>
        /// <param name="selfBehaviour">Self behaviour.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Disable_L<T>(this T selfBehaviour) where T : Behaviour
        {
            selfBehaviour.enabled = false;
            return selfBehaviour;
        }

        /// <summary>
        /// Load Asset to RA
        /// </summary>
        public static T LoadAssetByPath<T>(this MonoBehaviour mono, string path) where T : UnityEngine.Object
        {
            if (path.IsNullOrEmpty())
            {
                Debug.LogError("Load Path is Null!");
            }
            T obj = Resources.Load<T>(path);
            if (obj == null)
            {
                Debug.LogError("Load " + typeof(T).ToString() + " Fail,chech path:" + path);
            }
            return obj;
        }
    }
}