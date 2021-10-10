// ========================================================
// Des：Gameobject的扩展
// Author：yeyichen
// CreateTime：2018/05/29 23:11:48 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 2017 liangxie
 * Copyright (c) 2018 liangxie
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

    /// <summary>
    /// GameObject's Util/Static This Extension
    /// </summary>
    public static class GameObjectExtension
    {
        public static void Example()
        {
            var gameObject = new GameObject();
            var transform = gameObject.transform;
            var selfScript = gameObject.AddComponent<MonoBehaviour>();
            var boxCollider = gameObject.AddComponent<BoxCollider>();

            gameObject.Show_L(); // gameObject.SetActive(true)
            selfScript.Show_L(); // this.gameObject.SetActive(true)
            boxCollider.Show_L(); // boxCollider.gameObject.SetActive(true)
            gameObject.transform.Show_L(); // transform.gameObject.SetActive(true)

            gameObject.Hide_L(); // gameObject.SetActive(false)
            selfScript.Hide_L(); // this.gameObject.SetActive(false)
            boxCollider.Hide_L(); // boxCollider.gameObject.SetActive(false)
            transform.Hide_L(); // transform.gameObject.SetActive(false)

        

            gameObject.SetLayer_L(0);

            gameObject.SetLayer_L("Default");
        }
        
        #region Show

        /// <summary>
        /// SetActive(true)
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="selfObj">Self object.</param>
        public static GameObject Show_L(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }

        /// <summary>
        /// .gameobject.SetActive(true)
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="selfComponent">Self component.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Show_L<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Show_L();
            return selfComponent;
        }

        #endregion

        #region Hide

        /// <summary>
        /// SetActive(false)
        /// </summary>
        /// <returns>The hide.</returns>
        /// <param name="selfObj">Self object.</param>
        public static GameObject Hide_L(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }

        /// <summary>
        /// .gameobject.SetActive(false)
        /// </summary>
        /// <returns>The hide.</returns>
        /// <param name="selfComponent">Self component.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Hide_L<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.Hide_L();
            return selfComponent;
        }

        #endregion
     
        #region Layer

        public static GameObject SetLayer_L(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }

        public static GameObject SetLayer_L(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }

      
        #endregion

        #region Tag

        public static GameObject SetTag_L(this GameObject selfObj, string tagName)
        {
            selfObj.tag = tagName;
            return selfObj;
        }

        #endregion
    }
}    