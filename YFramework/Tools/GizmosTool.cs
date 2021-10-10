// ========================================================
// Des：运行时Gizmos工具
// Author：yeyichen
// CreateTime：06/09/2018 22:30:15
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
using System;
using YFramework.Extension;

namespace YFramework
{
    public class GizmosTool {

        Action gizmos;
        Action gizmosSelected;

        public GizmosTool()
        {
            if (YFrameworkManager.Instance == null)
                Resources.Load("YFrameworkManager").Instantiate_L().As_L<GameObject>();
        }

        public void AddGizmos(Action action,bool isSelected=false)
        {
            //Debug.Log(action.Method.Name);
            if(!isSelected)
            {
                if (gizmos == null)
                {
                    gizmos += action;
                }
                else
                {
                    Delegate[] delegates = gizmos.GetInvocationList();
                    for (int i = 0; i < delegates.Length;i++)
                    {
                        if(delegates[i].Method.Name==action.Method.Name)
                        {
                            gizmos=(Action)Delegate.Remove(gizmos, delegates[i]);
                        }
                    }
                    gizmos +=action;
                }
            }
            else
            {
                if (gizmosSelected == null)
                {
                    gizmosSelected += action;
                }
                else
                {
                    Delegate[] delegates = gizmosSelected.GetInvocationList();
                    for (int i = 0; i < delegates.Length; i++)
                    {
                        if (delegates[i].Method.Name == action.Method.Name)
                        {
                            gizmosSelected = (Action)Delegate.Remove(gizmosSelected, delegates[i]);
                        }
                    }
                    gizmosSelected += action;
                }
            }
        }

        public void RemoveGizmos(Action action, bool isSelected = false)
        {
            if (!isSelected)
            {
                if(gizmos.GetInvocationList().Contains(action))
                {
                    gizmos=(Action)Delegate.Remove(gizmos, action);
                }
            }
            else
            {
                if (gizmosSelected.GetInvocationList().Contains(action))
                {
                    gizmosSelected=(Action)Delegate.Remove(gizmosSelected, action);
                }
            }
        }

        public void ClearGizmos(bool isSelected=false)
        {
            if(!isSelected)
            {
                gizmos = null;
            }
            else
            {
                gizmosSelected = null;
            }
        }

        public void OnDrawGizmos()
        {
            if(gizmos!=null)
            {
                //Debug.Log(gizmos.GetInvocationList().Length);
                gizmos.Invoke();
            }
        }

        public void OnDrawGizmosSelected()
        {
            if(gizmosSelected!=null)
            {
                gizmosSelected.Invoke();
            }
        }
    }
}
