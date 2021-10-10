// ========================================================
// Des：随机数包装
// Author：yeyichen
// CreateTime：2018/05/31 12:30:13 
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

namespace YFramework
{
    using System.Collections;
    using UnityEngine;
    using System.Collections.Generic;
    using YFramework.Extension;
    using System.Linq;

    public class RandomTool
    {
        public static void Example()
        {
            //产生100个随机种子并打印出来
            int seed = 0;
            100.ForEach(() => seed = GenerateSeed().LogI_L());

            List<int> list = new List<int>();
            100.ForEach((i) => list.Add(i));
            3.ForEach(() =>
                      Range(seed, list, 10, true)
                      .ToSplitString()
                      .LogI_L()
                     );
            Debug.Log("测试是否影响原列表："+(list.Count==100));

            3.ForEach(() =>
                      Range(list, 10, false)
                      .ToSplitString()
                      .LogI_L()
                     );

            int hitTime = 0;
            100000.ForEach(() =>{
                if(IfHit(0.7f))
                {
                    hitTime++;
                }
                Debug.Log("0.7的成功率在10W次中hit" + hitTime + "次");
            });
        }

        /// <summary>
        /// 返回随机正数
        /// </summary>
        /// <returns>The seed.</returns>
        public static int GenerateSeed()
        {
            return Random.Range(0, 1 << 31-1);
        }

        /// <summary>
        /// 在列表中返回指定个数的重复/不重复的T；传入通样的seed返回相同结果
        /// </summary>
        /// <returns>The range.</returns>
        /// <param name="seed">同一个seed返回相同结果</param>
        /// <param name="list">总的列表</param>
        /// <param name="num">选取个数</param>
        /// <param name="noRepeat">If set to <c>true</c> 不重复.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> Range<T>(int seed, IList<T> list, int num, bool noRepeat = true)
        {
            System.Random random = new System.Random(seed);

            List<T> result = new List<T>();
            List<T> newList = new List<T>(list);

            if (noRepeat)
            {
                if (num > list.Count)
                {
                    Debug.LogError("list中的值不足以返回不重复的结果");
                    return result;
                }
                for (int i = 0; i < num; i++)
                {
                    T newR = newList[random.Next(0, newList.Count)];
                    result.Add(newR);
                    newList.Remove(newR);
                }
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    result.Add(list[random.Next(0, list.Count)]);
                }
            }
            return result;
        }

        /// <summary>
        /// 在连续的int中返回指定个数的重复/不重复的数，from包含，to不包含；传入通样的seed返回相同结果
        /// </summary>
        /// <returns>The range.</returns>
        /// <param name="seed">同一个seed返回相同结果</param>
        /// <param name="from">起始数值</param>
        /// <param name="to">末数值</param>
        /// <param name="num">返回的数总数</param>
        /// <param name="noRepeat">If set to <c>true</c> 不重复 </param>
        public static List<int> Range(int seed, int from, int to, int num, bool noRepeat = true)
        {
            List<int> intList = new List<int>();
            for (int i = from; i < to; i++)
            {
                intList.Add(i);
            }
            return Range<int>(seed,intList, num, noRepeat);
        }

        /// <summary>
        /// 在连续的int中返回指定个数的重复/不重复的数，from包含，to不包含；传入通样的seed返回相同结果
        /// </summary>
        /// <returns>The range.</returns>
        /// <param name="seed">同一个seed返回相同结果</param>
        /// <param name="from">起始数值</param>
        /// <param name="to">末数值</param>
        /// <param name="num">返回的数总数</param>
        public static List<float> Range(int seed, int num, float from, float to)
        {
            System.Random random = new System.Random(seed);

            List<float> floatList = new List<float>();
            for (int i = 0; i < num; i++)
            {
                floatList.Add((float)random.NextDouble());
            }
            return floatList;
        }

        /// <summary>
        /// 在列表中返回指定个数的重复/不重复的T；每次结果不同
        /// </summary>
        public static List<T> Range<T>(List<T> list,int num,bool noRepeat=true)
        {
            List<T> result = new List<T>();
            List<T> newList = new List<T>(list);

            if(noRepeat)
            {
                if(num>list.Count)
                {
                    Debug.LogError("list中的值不足以返回不重复的结果");
                    return result;
                }
                for (int i = 0; i < num;i++)
                {
                    T newR = newList[Random.Range(0, newList.Count)];
                    result.Add(newR);
                    newList.Remove(newR);
                }
            }
            else
            {
                for (int i = 0; i < num;i++)
                {
                    result.Add(list[Random.Range(0, list.Count)]);
                }
            }
            return result;
        }

        /// <summary>
        /// 在连续的int中返回指定个数的重复/不重复的数，from包含，to不包含；每次结果不同
        /// </summary>
        public static List<int> Range(int from,int to,int num,bool noRepeat=true)
        {
            List<int> intList = new List<int>();
            for (int i = from; i < to;i++)
            {
                intList.Add(i);
            }
            return Range<int>(intList, num, noRepeat);
        }

        /// <summary>
        /// 返回指定个数的重复/不重复的float，from包含，to不包含；每次结果不同
        /// </summary>
        public static List<float> Range(int num, float from=0, float to=1)
        {
            List<float> floatList = new List<float>();
            for (int i = 0; i < num; i++)
            {
                floatList.Add(Random.Range(from,to));
            }
            return floatList;
        }

        /// <summary>
        /// 比如概率为70%，则传0.7
        /// </summary>
        /// <returns><c>true</c>, if hit was ifed, <c>false</c> otherwise.</returns>
        public static bool IfHit(float chance)
        {
            if (Random.value < chance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据权值来获取索引
        /// </summary>
        /// <param name="powers">权重列表</param>
        /// <returns>获取到的值对应的list索引</returns>
        public static int GetRandomWithPower(List<float> powers)
        {
            float sum = 0;
            foreach (float power in powers)
            {
                sum += power;
            }

            float randomNum = UnityEngine.Random.Range(0.0f, sum);
            float currentSum = 0;
            for (int i = 0; i < powers.Count; i++)
            {
                float nextSum = currentSum + powers[i];
                if ( (randomNum > currentSum) && (randomNum < nextSum) )
                {
                    return i;
                }

                currentSum = nextSum;
            }

            Debug.LogError("权值范围计算错误！");
            return -1;
        }

        /// <summary>
        /// 根据权值获取值，Key为值，Value为权值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="powersDict"></param>
        /// <returns></returns>
        public static T GetRandomWithPower<T>(Dictionary<T, float> powersDict)
        {
            var keys = new List<T>();
            var values = new List<float>();

            foreach (var key in powersDict.Keys)
            {
                keys.Add(key);
                values.Add(powersDict[key]);
            }

            int finalKeyIndex = GetRandomWithPower(values);
            return keys[finalKeyIndex];
        }

        /// <summary>
        /// 获得随机列表中元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(List<T> selfList)
        {
            return selfList[UnityEngine.Random.Range(0, selfList.Count)];
        }

        /// <summary>
        /// 获得随机列表中元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(List<T> selfList,int seed)
        {
            return Range<T>(seed,selfList, 1)[0];
        }

        /// <summary>
        /// 获得随机字典中的元素
        /// </summary>
        /// <returns>The random item.</returns>
        /// <param name="selfDic">Self dic.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        /// <typeparam name="V">The 2nd type parameter.</typeparam>
        public static KeyValuePair<K,V> GetRandomItem<K,V>(IDictionary<K,V> selfDic)
        {
            int index = Random.Range(0, selfDic.Count);
            return new KeyValuePair<K, V>(selfDic.Keys.ElementAt(index), selfDic.Values.ElementAt(index));
        }

    }
}