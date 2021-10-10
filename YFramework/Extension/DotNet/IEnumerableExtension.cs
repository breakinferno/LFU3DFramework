// ========================================================
// Des：数组、列表、字典等的扩展
// Author：yeyichen
// CreateTime：2018/06/1 11:53:45 
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
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using UnityEngine;

    public static class IEnumerableExtension
    {
        public static void Example()
        {
            // Array
            var testArray = new int[] {1, 2, 3};
            testArray.ForEach_L(number => Debug.Log(number));

            // IEnumerable<T>
            IEnumerable<int> testIenumerable = new List<int> {1, 2, 3};
            testIenumerable.ForEach_L(number => Debug.Log(number));
            var testDictionary = new Dictionary<string, string>()
                .ForEach_L(keyValue => Debug.Log(string.Format("key:{0},value:{1}", keyValue.Key, keyValue.Value)));
            
            // testList
            List<int> testList = new List<int> {1, 2, 3};
            testList.ForEach(number => Debug.Log(number));
            testList.ForEach_L((index, item) =>
            {
                Debug.Log(index + ":"+item.ToString());
            });
            testList.ForEachReverse_L(number => Debug.Log(number));
            Debug.Log(testList.ToSplitString());

            // merge
            var dictionary1 = new Dictionary<string, string> {{"1", "2"}};
            var dictionary2 = new Dictionary<string, string> {{"3", "4"}};
            var dictionary3 = new Dictionary<string, string> { { "3", "6" } };
            Debug.Log(dictionary3.ToContentString());
            dictionary3.Merge_L(true, dictionary1, dictionary2);
            Debug.Log(dictionary3.ToContentString());

            List<int> lis = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            lis.Exchange_L(0, 9)
               .ToSplitString()
               .LogI_L<string>(lis.ToSplitString().Replace(';','&'))
               .LogI_L<string>();

            //数值foreach
            5.ForEach((i) => i.LogI_L());

            new Vector2Int(3, 5).ForEach((i) =>
            {
                i=i + 1;
                Debug.Log(i);
            });
        }

        #region Array Extension

        public static T[] ForEach_L<T>(this T[] selfArray, Action<T> action)
        {
            Array.ForEach(selfArray, action);
            return selfArray;
        }

        public static IEnumerable<T> ForEach_L<T>(this IEnumerable<T> selfArray, Action<T> action)
        {
            if (action == null) throw new ArgumentException();
            foreach (var item in selfArray)
            {
                action(item);
            }
            return selfArray;
        }

        public static T[] Add<T>(this T[] selfArray,T target)
        {
            T[] temp = new T[selfArray.Length + 1];
            selfArray.CopyTo(temp,0);
            temp[temp.Length - 1] = target;
            selfArray = temp;
            return selfArray;
        }

        public static T[] Add<T>(this T[] selfArray, T[] target)
        {
            T[] temp = new T[selfArray.Length + target.Length];
            selfArray.CopyTo(temp, 0);
            target.CopyTo(temp, selfArray.Length);
            selfArray = temp;
            return selfArray;
        }

        /// <summary>
        /// 截取target中的一部分数据，如果不足填入default值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selfArray"></param>
        /// <param name="target"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static T[] Add<T>(this T[] selfArray, T[] target,int num)
        {
            T[] temp = new T[selfArray.Length];
            selfArray.CopyTo(temp, 0);
            T[] newArr = new T[num];
            if (num > target.Length)
            {
                //target加入
                for (int i = 0; i < target.Length; i++)
                {
                    newArr[i] = target[i];
                }


                //填空
                for (int i = target.Length; i < num; i++)
                {
                    newArr[i] = default(T);
                }
            }
            else
            {
                //截取一段填入
                for (int i = 0; i < num; i++)
                {
                    newArr[i] = target[i];
                }
            }
            return temp.Add(newArr);
        }

        public static bool Contains<T>(this T[] selfArray,T t)
        {
            return new List<T>(selfArray).Contains(t);
        }

        #endregion

        #region List Extension

        /// <summary>
        /// 倒着遍历
        /// </summary>
        /// <returns>The each reverse.</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="action">Action<T>,item为selfList的每个元素</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ForEachReverse_L<T>(this List<T> selfList, Action<T> action)
        {
            if (action == null) throw new ArgumentException();

            for (var i = selfList.Count - 1; i >= 0; --i)
                action(selfList[i]);

            return selfList;
        }

        /// <summary>
        /// 遍历列表
        /// </summary>
        /// <typeparam name="T">列表类型</typeparam>
        /// <param name="selfList">目标表</param>
        /// <param name="action">Action<int,T>，int为序号，T为selfList的每个元素<index,T></param>
        public static List<T> ForEach_L<T>(this List<T> selfList, Action<int, T> action)
        {
            for (var i = 0; i < selfList.Count; i++)
            {
                action(i, selfList[i]);
            }
            return selfList;
        }

        /// <summary>
        /// 交换两个列表中的两个特定位置的元素
        /// </summary>
        /// <returns>The l.</returns>
        /// <param name="selfList">Self list.</param>
        /// <param name="indexFront">Index front.</param>
        /// <param name="indexBack">Index back.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> Exchange_L<T>(this List<T> selfList,int indexFront,int indexBack)
        {
            T item1 = selfList[indexFront];
            T item2 = selfList[indexBack];
            selfList.RemoveAt(indexBack);
            selfList.RemoveAt(indexFront);
            selfList.Insert(indexFront, item2);
            selfList.Insert(indexBack, item1);
            return selfList;
        }

        #endregion

        #region Dictionary Extension

        /// <summary>
        /// 遍历字典
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="selfDic"></param>
        /// <param name="action"></param>
        public static Dictionary<K, V> ForEach_L<K, V>(this Dictionary<K, V> selfDic, Action<K, V> action)
        {
            var dictE = selfDic.GetEnumerator();

            while (dictE.MoveNext())
            {
                var current = dictE.Current;
                action(current.Key, current.Value);
            }

            dictE.Dispose();
            return selfDic;
        }

        /// <summary>
        /// 合并多个字典
        /// </summary>
        /// <returns>The merge.</returns>
        /// <param name="selfDic">Self dic.</param>
        /// <param name="isOverride">If set to <c>true</c> is override.</param>
        /// <param name="targetDic">Target dic.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        /// <typeparam name="V">The 2nd type parameter.</typeparam>
        public static Dictionary<K, V> Merge_L<K, V>(this Dictionary<K, V> selfDic, bool isOverride, params Dictionary<K, V>[] targetDic)
        {
            targetDic.ForEach_L(item =>
            {
                item.ForEach_L(subItem =>
                {
                    if(selfDic.ContainsKey(subItem.Key))
                    {
                        if(isOverride)
                            selfDic[subItem.Key] = subItem.Value;
                    }
                    else
                    {
                        selfDic.Add(subItem.Key,subItem.Value);
                    }
                });
            });
            return selfDic;
        }

        /// <summary>
        /// 将普通字典转化为可序列化字典
        /// </summary>
        /// <returns>The serializable dictionary.</returns>
        /// <param name="selfDic">Self dic.</param>
        /// <param name="targetDic">Target dic.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        /// <typeparam name="V">The 2nd type parameter.</typeparam>
        public static SerializableDictionary<K,V> ToSerializableDictionary<K,V>(this IDictionary<K,V> selfDic,SerializableDictionary<K,V> targetDic)
        {
            selfDic.ForEach_L(item =>
            {
                targetDic = new SerializableDictionary<K, V>();
                targetDic.Add(item.Key,item.Value);
            });
            return null;
        }
        #endregion

        /// <summary>
        /// 将函数执行times次
        /// </summary>
        /// <param name="times">Times.</param>
        /// <param name="action">Action.</param>
        public static void ForEach(this int times,Action action)
        {
            for (int i = 0; i < times;i++)
            {
                action();
            }
        }

        /// <summary>
        /// 将函数执行times次，传入为第N次执行
        /// </summary>
        /// <param name="times">Times.</param>
        /// <param name="action">Action.</param>
        public static void ForEach(this int times,Action<int> action)
        {
            for (int i = 0; i < times;i++)
            {
                action(i);
            }
        }

        /// <summary>
        /// 执行times.y-times.x次action方法
        /// </summary>
        /// <param name="times">Times.</param>
        /// <param name="action">Action.</param>
        public static void ForEach(this Vector2Int times,Action action)
        {
            for (int i = times.x; i < times.y;i++)
            {
                action();
            }
        }

        /// <summary>
        /// 执行times.y-times.x次action方法，传入int为（times.x+第N次执行）
        /// </summary>
        /// <param name="times">Times.</param>
        /// <param name="action">Action.</param>
        public static void ForEach(this Vector2Int times,Action<int> action)
        {
            for (int i = times.x; i < times.y;i++)
            {
                action(i);
            }
        }
    }
}


/*Linq
 * public class YFrameworkTest : MonoBehaviour
{
    public class TT
    {
        public int index;
        public string indexT;

        public override string ToString()
        {
            return index+" "+indexT;
        }
    }

    List<TT> tTs = new List<TT>()
    {
        new TT{index=-4,indexT="-40"},
        new TT{index=-3,indexT="-30"},
        new TT{index=-2,indexT="-20"},
        new TT{index=-1,indexT="-10"},
        new TT{index=0,indexT="00"},
        new TT{index=1,indexT="10"},
        new TT{index=2,indexT="20"},
        new TT{index=3,indexT="30"},
        new TT{index=4,indexT="40"},
    };


    [Button]
    public void Test()
    {
        tTs.Where(item => item.index > 0)
           //.All(item=>item.index>1)//0.25
           //.Any(item=>item.index>1)//false
           //.Aggregate((item1, item2) =>
           //{
           //    item2.indexT = item1.indexT + item2.indexT;
           //    return item2;
           //})//4 10203040
           //.Average(item=>item.index)//2.5

           .LogI_L()
           ;
    }
}

 */