// ========================================================
// Des：Gameobject的扩展
// Author：yeyichen
// CreateTime：2018/06/06 08:13:28 
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

    public static class ObjectExtension
    {
        public static void Example()
        {
            GameObject go=new GameObject("OjbectExample")
                .ApplySelfTo_L(item => Debug.Log(item.name))
                .SetName_L("OjbectExample2")
                .LogI_L()
                .Hide_L()
                .NotNull(item => Debug.Log(item.name));
            
            go = null;
            go.NotNull(item => Debug.Log(item.name));
            
        }
        
        
        #region CEUO001 Instantiate
        /// <summary>
        /// 实例化一个Object
        /// </summary>
        /// <returns>The instantiate.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Instantiate_L<T>(this T selfObj) where T : Object
        {
            return Object.Instantiate(selfObj);
        }

        #endregion

        #region CEUO002 Instantiate
        /// <summary>
        /// 设置object.Name
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="name">Name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T SetName_L<T>(this T selfObj, string name) where T : Object
        {
            selfObj.name = name;
            return selfObj;
        }

        #endregion

        #region CEUO003 Destroy Self
        /// <summary>
        /// 销毁自身
        /// </summary>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void DestroySelf<T>(this T selfObj, bool destroyGameobject = true) where T : Component 
        {
            if(destroyGameobject)
            {
                Object.Destroy(selfObj.gameObject);
            }
            else
            {
                Object.Destroy(selfObj);
            }
        }

        /// <summary>
        /// 销毁自身
        /// </summary>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void DestroySelf(this GameObject selfObj, bool destroyGameobject = true)
        {
            Object.Destroy(selfObj);
            return;
        }

        /// <summary>
        /// 一段时间后销毁
        /// </summary>
        /// <returns>The self.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="afterDelay">After delay.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T DestroySelf_L<T>(this T selfObj, float afterDelay,bool destroyGameobject=true) where T : Component
        {
            if(destroyGameobject)
            {
                Object.Destroy(selfObj.gameObject, afterDelay);
            }
            else
            {
                Object.Destroy(selfObj, afterDelay);
            }
            return selfObj;
        }

        /// <summary>
        /// 一段时间后销毁
        /// </summary>
        /// <returns>The self.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="afterDelay">After delay.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static GameObject DestroySelf_L(this GameObject selfObj, float afterDelay)
        {
            Object.Destroy(selfObj, afterDelay);
            return selfObj;
        }

        #endregion

        #region CEUO005 Apply Self To 
        /// <summary>
        /// 执行参数为自己的方法
        /// </summary>
        /// <returns>The self to.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="toFunction">To function.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ApplySelfTo_L<T>(this T selfObj, System.Action<T> toFunction) where T : Object
        {
            toFunction.Invoke(selfObj);
            return selfObj;
        }

        #endregion

        #region CEUO006 DontDestroyOnLoad

        public static T DontDestroyOnLoad_L<T>(this T selfObj) where T : Object
        {
            Object.DontDestroyOnLoad(selfObj);
            return selfObj;
        }

        #endregion

        public static T As_L<T>(this object selfObj) where T : Object
        {
            return selfObj as T;
        }

        /// <summary>
        /// debug.log自己
        /// </summary>
        /// <returns>The i l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogI_L<T>(this T selfObj)
        {
            Debug.Log(selfObj);
            return selfObj;
        }

        /// <summary>
        /// debug.logError自己
        /// </summary>
        /// <returns>The e l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogE_L<T>(this T selfObj)
        {
            Debug.LogError(selfObj);
            return selfObj;
        }

        /// <summary>
        /// debug.logWarning自己
        /// </summary>
        /// <returns>The w l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogW_L<T>(this T selfObj)
        {
            Debug.LogWarning(selfObj);
            return selfObj;
        }

        /// <summary>
        /// debug.log Target
        /// </summary>
        /// <returns>The i l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="target">要被Debug的对象</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogI_L<T>(this T selfObj,object target)
        {
            Debug.Log(target);
            return selfObj;
        }

        /// <summary>
        /// debug.logError Target
        /// </summary>
        /// <returns>The e l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="target">要被Debug的对象</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogE_L<T>(this T selfObj,object target)
        {
            Debug.LogError(target);
            return selfObj;
        }

        /// <summary>
        /// debug.logWarning Target
        /// </summary>
        /// <returns>The w l.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="target">要被Debug的对象</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T LogW_L<T>(this T selfObj,object target)
        {
            Debug.LogWarning(target);
            return selfObj;
        }

        /// <summary>
        /// 当selfObj不为空时调用
        /// </summary>
        /// <returns>The null.</returns>
        /// <param name="selfObj">Self object.</param>
        /// <param name="action">Action.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T NotNull<T>(this T selfObj,System.Action<T> action)
        {
            if(!Object.Equals(selfObj,null))
            {
                action(selfObj);
            }
            return selfObj;
        }
    }
}