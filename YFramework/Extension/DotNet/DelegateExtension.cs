// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/09/2018 23:20:44
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YFramework.Extension;

namespace YFramework
{
    public static class DelegateExtension {
        private delegate void TestDelegate();

        public static void Example()
        {
            // action
            System.Action action = () => Debug.Log("action called");
            action.InvokeGracefully(); // if (action != null) action();

            // func
            Func<int> func = null;
            func.InvokeGracefully();

            // delegate
            TestDelegate testDelegate = () => { };
            testDelegate.InvokeGracefully();
        }


        //#region Func Extension

        //public static T InvokeGracefully<T>(this Func<T> selfFunc)
        //{
        //    return null != selfFunc ? selfFunc() : default(T);
        //}

        //#endregion

        //public static bool InvokeGracefully(this System.Action selfAction)
        //{
        //    if (null != selfAction)
        //    {
        //        selfAction();
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool InvokeGracefully<T>(this Action<T> selfAction, T t)
        //{
        //    if (null != selfAction)
        //    {
        //        selfAction(t);
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool InvokeGracefully<T, K>(this Action<T, K> selfAction, T t, K k)
        //{
        //    if (null != selfAction)
        //    {
        //        selfAction(t, k);
        //        return true;
        //    }
        //    return false;
        //}

        public static bool InvokeGracefully(this Delegate selfAction, params object[] args)
        {
            if (null != selfAction)
            {
                selfAction.DynamicInvoke(args);
                return true;
            }
            return false;
        }

    }
}
