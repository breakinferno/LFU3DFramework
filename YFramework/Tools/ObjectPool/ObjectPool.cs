// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/17/2018 20:42:29
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
using YFramework;
using YFramework.Extension;
using System.Linq;

namespace YFramework
{
    [System.Serializable]
    public class PoolObjectInfo
    {
        [Tooltip("每个池里唯一的名字")]
        public string objName;

        [Tooltip("可以入池的物体(预制体或者gameobject)")]
        public GameObject obj;

        [Tooltip("预加载的数量")]
        public int preloadAmount;


        [Space,Tooltip("是否限制实例的最大数量")]
        public bool ifLimitInstanceNum;

        [ShowIf("ifLimitInstanceNum"),Tooltip("限制的最大数量")]
        public int limitNum;


        [Space,Tooltip("是否剔除长时间用不到的实例")]
        public bool ifCullDespawned;

        [ShowIf("ifCullDespawned"),Tooltip("要保留的总的实例数量")]
        public int cullAbove;

        [ShowIf("ifCullDespawned"),Tooltip("每隔多少时间检测一次要不要剔除")]
        public float cullInterval;

        [ShowIf("ifCullDespawned"), Tooltip("每次剔除最多销毁多少个对象，过多可能会导致卡顿")]
        public int cullNumMax;


        public int instanceNum
        {
            get
            {
                return spawnedList.Count + despawnedList.Count;
            }
        }
        //[Space]
        //public bool logMessage;

        [HideInInspector]
        public List<GameObject> spawnedList = new List<GameObject>();

        [HideInInspector]
        public List<GameObject> despawnedList = new List<GameObject>();

        public GameObject Spawn(Transform parent = null)
        {
            if (despawnedList.Count > 0)
            {
                GameObject go = despawnedList[0];
                parent.NotNull(item => go.transform.SetParent(parent));
                go.transform.SetToILocalPosition_L();
                despawnedList.RemoveAt(0);
                spawnedList.Add(go);
                return OnSpawn(go);
            }
            else
            {
                GameObject go = null;
                if (ifLimitInstanceNum && instanceNum >= limitNum)
                {
                    Despawn(spawnedList[0]);
                    go = Spawn();
                }
                else
                {
                    go = obj.Instantiate_L()
                        .SetName_L(objName + "(Pooled)")
                        .transform
                        .SetParent_L(parent)
                        .gameObject;
                    spawnedList.Add(go);
                    OnDespawn(go);
                }
                return OnSpawn(go);
            }
        }

        public GameObject Despawn(GameObject target)
        {
            OnDespawn(target);
            if (spawnedList.Contains(target))
            {
                spawnedList.Remove(target);
            }
            despawnedList.Add(target);
            return target;
        }

        GameObject OnSpawn(GameObject go)
        {
            go.SetActive(true);
            go.GetComponentInChildren<IPoolObject>(true).NotNull((item) => item.OnSpawn());
            return go;
        }

        GameObject OnDespawn(GameObject go)
        {
            go.GetComponent<IPoolObject>().NotNull((item) => item.OnDespawn());
            go.SetActive(false);
            return go;
        }
    }
}

public class ObjectPool : MonoBehaviour {

    public string poolName;

    [SerializeField]
    private List<PoolObjectInfo> objectInfos;

	private PoolObjectInfo GetInfoByName(string infoName)
    {
        return objectInfos.Where(item => item.objName == infoName).First();
    }

	private void OnEnable()
	{
        ObjectPoolManager.Instance.AddPool(this);

        objectInfos.ForEach((item) =>
        {
            if (item.ifCullDespawned && item.cullInterval > 0)
            {
                StartCoroutine(CheckCull(item));
            }
        });
	}

	private void OnDisable()
	{
        StopAllCoroutines();
        ObjectPoolManager.Instance.DeletePool(poolName);
	}

	public void Start()
	{
        objectInfos.ForEach(item =>
        {
            int numToSpawn = 0;
            if (item.ifLimitInstanceNum)
            {
                numToSpawn = item.preloadAmount > item.limitNum ? item.limitNum : item.preloadAmount;
            }
            else
            {
                numToSpawn = item.preloadAmount;
            }

            numToSpawn.ForEach(() =>
            {
                GameObject go = item.obj
                                    .Instantiate_L()
                                    .SetName_L(item.objName + "(Pooled)")
                                    .Hide_L()
                                    .transform
                                    .SetParent_L(this.transform)
                                    .gameObject;
                item.Despawn(go);
            });
        });
	}

    /// <summary>
    /// 代替实例化
    /// </summary>
    /// <returns>The spawn.</returns>
    /// <param name="name">Name.</param>
    public GameObject Spawn(string name,Transform parent=null)
    {
        return GetInfoByName(name).Spawn(parent);
    }

    /// <summary>
    /// 代替销毁
    /// </summary>
    /// <returns>The despawn.</returns>
    /// <param name="target">Target.</param>
    public void Despawn(GameObject target)
    {
        objectInfos.ForEach(item =>
        {
            if (item.spawnedList.Contains(target))
            {
                item.Despawn(target);
            }
        });
    }

    public void RegestObj(string poolName,string objName,GameObject go)
    {
        GetInfoByName(objName).spawnedList.Add(go);
    }

    public GameObject GetOriginal(string name)
    {
        return GetInfoByName(name).obj;
    }

    IEnumerator CheckCull(PoolObjectInfo info)
    {
        while(true)
        {
            yield return new WaitForSeconds(info.cullInterval);
            if(info.instanceNum>info.cullAbove)
            {
                int numToDestroy = info.instanceNum - info.cullAbove;

                //最多只会清除没被激活的而不会影响已经激活的
                numToDestroy = Mathf.Min(numToDestroy,info.despawnedList.Count,info.cullNumMax);

                numToDestroy.ForEach(item => {
                    info.despawnedList[0].DestroySelf();
                    info.despawnedList.RemoveAt(0);
                });
            }
        }
    }
}
