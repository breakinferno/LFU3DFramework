// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/18/2018 12:42:54
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
using YFramework;
using YFramework.Extension;

/// <summary>
/// 用于管理非对象池生成的物体
/// 注：
/// 使用过多可能会导致超出PoolObjectInfo中的设置
/// </summary>
public class PlacedPoolObject : MonoBehaviour {

    public string belongedPoolName;

    public string objName;

	// Use this for initialization
	void Start () {
        ObjectPoolManager.Instance.GetPool(belongedPoolName)
                         .NotNull((item) =>
                         {
                             item.RegestObj(belongedPoolName, objName, this.gameObject);
                             this.DestroySelf(false);
                         });
	}
	
}
