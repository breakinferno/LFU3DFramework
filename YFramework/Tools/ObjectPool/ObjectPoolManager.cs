// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/17/2018 20:44:24
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

namespace YFramework
{
    /// <summary>
    /// 可以得到任意对象池
    /// </summary>
    public class ObjectPoolManager : QSingleton<ObjectPoolManager>{

        /// <summary>
        /// 对象池名字到对象池对象的映射
        /// </summary>
        private Dictionary<string, ObjectPool> nameToObjectPoolDic;

        private ObjectPoolManager()
        {
            
        }

		public override void OnSingletonInit()
		{
            base.OnSingletonInit();
            nameToObjectPoolDic = new Dictionary<string, ObjectPool>();
		}

        public void AddPool(ObjectPool newPool)
        {
            if(nameToObjectPoolDic.ContainsKey(newPool.poolName))
            {
                Debug.LogError("已存在名字为" + newPool.poolName + "的对象池，新的创建命令已忽略");
            }
            else
            {
                nameToObjectPoolDic.Add(newPool.poolName,newPool);
            }
        }

        public ObjectPool GetPool(string poolName)
        {
            if (nameToObjectPoolDic.ContainsKey(poolName)) 
            {
                return nameToObjectPoolDic[poolName];
            }
            else
            {
                Debug.LogError("不存在名字为" + poolName + "的对象池，返回null");
                return null;
            }
        }

        public bool DeletePool(string poolName)
        {
            if (nameToObjectPoolDic.ContainsKey(poolName))
            {
                return nameToObjectPoolDic.Remove(poolName);
            }
            else
            {
                Debug.LogError("不存在名字为" + poolName + "的对象池，删除命令已忽略");
                return false;
            }
        }
	}
}
