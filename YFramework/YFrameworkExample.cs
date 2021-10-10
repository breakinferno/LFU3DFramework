// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/06/2018 14:50:53
// Version：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFramework;
using YFramework.Extension;

public class YFrameworkExample : MonoBehaviour
{
    #region Attribute

    public string labelText = "我叫a";

    [Label("$labelText")]
    public int a;

    public enum Colors
    {
        [Describe("红")]
        red,
        [Describe("黄")]
        yellow,
        [Describe("绿")]
        green
    }

    [Describe("显示隐藏")]
    public bool ifShow;

    public bool notShow;
   
    [ShowIf("ifShow & !notShow")]
    public int int1;

    [ShowIf("$IfShow"), DisableEdit]
    public int int2;

    [Reorderable]
    public List<int> intList;

    public bool IfShow()
    {
        return ifShow;
    }

    [Button]
    public void DescribeExample()
    {
        this.GetType().GetDescribe("ifShow").LogI_L();
        typeof(Colors).GetDescribe(Colors.green.ToString()).LogI_L();
    }
    #endregion





    #region 可序列化字典

    [System.Serializable]
    public class SerializableDictionaryExample : SerializableDictionary<int, float> { }

    public SerializableDictionaryExample dic;

    [Button]
    public void SerializableDictionaryExampleFunc()
    {
        dic.Add(dic.Count, Random.Range(0.0f, 100f));
    }

    #endregion





    #region IEnumerable扩展

    [Button]
    public void IEnumerableExtensionExample()
    {
        IEnumerableExtension.Example();
    }
    #endregion





    #region String扩展

    [Button]
    public void StringExtensionExample()
    {
        StringExtension.Example();
    }

    #endregion





    #region Object扩展

    [Button]
    public void ObjectExtensionExample()
    {
        ObjectExtension.Example();
    }

    #endregion





    #region 协程内开启线程

    [Button]
    public void ThreadExample()
    {
        StartCoroutine(ThreadTestIEnumerator());
        Debug.Log("thread complete");
    }

    IEnumerator ThreadTestIEnumerator()
    {
        Debug.Log("thread begin");
        //开启一个线程，用途为打印从0到9999
        yield return new WaitForThreadedTask(
            () => 10000.ForEach(
                (i) => Debug.Log(i)
            )
        );
    }
    #endregion





    #region 代码段运行时间

    [Button]
    public void CodeRunTimeExample()
    {
        CodeRunTimeTool tool = new CodeRunTimeTool();
        tool.Begin();
        string str = "";
        1000000.ForEach(
            () => str += "j"
        );
        tool.Stop();
    }

    #endregion





    #region 随机工具

    [Button]
    public void RandomExample()
    {
        RandomTool.Example();
    }
    #endregion





    #region 事件池
#if UNITY_EDITOR
    [Button]
    public void EventPoolExample()
    {
        EventPoolTool.Example();
    }
#endif
    #endregion



    #region
    [Button]
    public void SaveToolExample()
    {
        SaveTool.Instance.Get(SaveKey.a).LogI_L();
        SaveTool.Instance.Set(SaveKey.a, "123");
        SaveTool.Instance.Get(SaveKey.a).LogI_L();
        SaveTool.Instance.Save();
    }
    #endregion





    #region 运行时Gizmos

    GizmosTool gt;

    [Button]
    public void GizmosToolExample()
    {
        //0.2的概率清除所有
        if(RandomTool.IfHit(0.2f))
        {
            gt.ClearGizmos();
            gt.ClearGizmos(true);
            return;
        }

        gt.AddGizmos(() =>
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
        });

        float r = Random.Range(0.0f, 10f);
        gt.AddGizmos(() =>
        {
            Gizmos.DrawSphere(Vector3.one*10, r);
        },true);
    }

    private void OnDrawGizmosSelected()
    {
        if (gt != null)
            gt.OnDrawGizmosSelected();
    }

    private void OnDrawGizmos()
    {
        if(gt!=null)
            gt.OnDrawGizmos();
    }

	private void Start()
	{
        gt = new GizmosTool();
	}
	#endregion
}
