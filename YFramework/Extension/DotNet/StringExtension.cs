// ========================================================
// Des：字符串的扩展
// Author：yeyichen
// CreateTime：2018/06/2 11:47:16 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 20?? 浩深
 * Copyright (c) 2018 liangxie
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
 * THE SOFTWARE.using System.Collections.Generic;
 ****************************************************************************/

namespace YFramework.Extension
{
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Linq;

    public static class StringExtension
    {
        public class IStringableTest:IStringableObject<IStringableTest>
        {
            public int a;
            public int b;

            public IStringableTest ConvertFromString(string str)
            {
                String[] sA=str.Split(';');
                return new IStringableTest { a = sA[0].ToInt(), b = sA[1].ToInt() };
            }

            public string ConvertToString()
            {
                return a.ToString() + ";" + b.ToString();
            }
        }

        public static void Example()
        {
            string test1 = "2;3;4;5;6";
            Debug.Log(test1.ToAnyTypeList<int>()[2]);

            List<int> test2 = new List<int>() { 1, 2, 3, 4, 5 };
            Debug.Log(test2.ToSplitString());

            Vector4 vector4 = new Vector4(1.23f, 2.43f, 35.43f, 12.9f);
            Debug.Log(vector4.ToString());
            Debug.Log(vector4.ToString().ToVector4());

            Vector3 vector3 = new Vector3(2.22f, 4.22f, 6.22f);
            Debug.Log(vector3.ToString());
            Debug.Log(vector3.ToString().ToVector3());

            Vector2 vector2 = new Vector2(2.33f, 4.22f);
            Debug.Log(vector2.ToString());
            Debug.Log(vector2.ToString().ToVector2());

            Quaternion qua = new Quaternion(0.2f,0.12f,0.88f,1);
            Debug.Log(qua.ToString());
            Debug.Log(qua.ToString().ToQuaternion());

            int i = 129;
            Debug.Log(i.ToString());
            Debug.Log(i.ToString().ToInt());

            float j = 98.263782f;
            Debug.Log(j);
            Debug.Log(j.ToString().ToFloat());

            Debug.Log("几点开始jjj".HasChinese());
            Debug.Log("dfsfd".HasChinese());

            Debug.Log("jfl,sdkgmixc,vdm".FindBetween("j", "m"));
            Debug.Log("sdfsdglkjafs".FindAfter("s"));

            Dictionary<int, string> test3 = new Dictionary<int, string>();
            test3.Add(1, "this is 1");
            test3.Add(2, "this is 2");
            test3.Add(3, "this is 3");
            Debug.Log(test3.ToContentString());

            IStringableTest stringable = new IStringableTest { a = 12, b = 21 };
            string stringableString = stringable.ConvertToString();
            Debug.Log(stringableString);
            Debug.Log(stringableString.ToStringableObject<IStringableTest>().b);
        }

        public const string defaultSplit = ";";

        #region 字符串转数组和列表
        /// <summary>
        /// 有分隔符的字符串转换为数组
        /// </summary>
        /// <returns>指定类型数组.</returns>
        /// <param name="str">要转换的字符串.</param>
        /// <param name="split">分割字符.</param>
        /// <typeparam name="T">任意类型.</typeparam>
        public static T[] ToAnyTypeArray<T>(this string str, string split = defaultSplit)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string[] strArray = str.Split(new string[] { split }, StringSplitOptions.None);
            T[] convertArray = new T[strArray.Length];

           
            for (int i = 0; i < strArray.Length; i++)
            {
                convertArray[i] = (T)Convert.ChangeType(strArray[i], typeof(T));
            }

            return convertArray;
        }

        /// <summary>
        /// 有分隔符的字符串转换为列表
        /// </summary>
        /// <returns>The to any type list.</returns>
        /// <param name="str">String.</param>
        /// <param name="split">Split.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> ToAnyTypeList<T>(this string str, string split = defaultSplit)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            string[] strArray = str.Split(new string[] { split }, StringSplitOptions.None);
            List<T> convertList = new List<T>();

            for (int i = 0; i < strArray.Length; i++)
            {
                convertList.Add((T)Convert.ChangeType(strArray[i], typeof(T)));
            }

            return convertList;

        }
        #endregion




        #region 数组和列表转字符串

        public static string ToSplitString<T>(this IList<T> tArray, string split = defaultSplit)
        {
            if (tArray == null)
                return null;

            StringBuilder sbTemp = new StringBuilder();
            tArray.ForEach_L(item =>
            {
                sbTemp.Append(item.ToString());
                sbTemp.Append(split);
            });
            return sbTemp.ToString().Substring(0, sbTemp.Length - split.Length);
        }

        #endregion

        #region 字符串转换成对应Type
        /// <summary>
        /// 将字符串转成继承IStringableObject的任意对象
        /// </summary>
        /// <returns>The stringable object.</returns>
        /// <param name="str">String.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T ToStringableObject<T>(this string str) where T : IStringableObject<T>
        {
            return Activator.CreateInstance<T>().ConvertFromString(str);
        }

        public static Vector4 ToVector4(this string selfStr)
        {
            var str = selfStr.Trim();
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            Vector4 result = new Vector4();

            var strArray = str.Split(',');
            if (strArray.Length == 4)
            {
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
                result.w = float.Parse(strArray[3]);
            }
            else
            {
                throw new Exception("四维向量解析失败");
            }
            return result;
        }

        public static Quaternion ToQuaternion(this string selfStr)
        {
            Vector4 vec = selfStr.ToVector4();
            Quaternion result = new Quaternion(vec.x, vec.y, vec.z, vec.w);
            return result;
        }

        public static Vector3 ToVector3(this string selfStr)
        {
            var str = selfStr.Trim();
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            Vector3 result = new Vector3();
            var strArray = str.Split(',');
            if (strArray.Length == 3)
            {
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
                result.z = float.Parse(strArray[2]);
            }
            else
            {
                throw new Exception("三维向量解析失败");
            }
            return result;
        }

        public static Vector2 ToVector2(this string selfStr)
        {
            var str = selfStr.Trim();
            str = str.Replace("(", "");
            str = str.Replace(")", "");
            Vector2 result = new Vector2();
            var strArray = str.Split(',');
            if (strArray.Length == 2)
            {
                result.x = float.Parse(strArray[0]);
                result.y = float.Parse(strArray[1]);
            }
            else
            {
                throw new Exception("二维向量解析失败");
            }
            return result;
        }

        public static int ToInt(this string selfStr)
        {
            return int.Parse(selfStr);
        }

        public static float ToFloat(this string selfStr)
        {
            return float.Parse(selfStr);
        }

        /// <summary>
        /// 根据路径在场景中找到Transform
        /// </summary>
        /// <returns>The transform.</returns>
        /// <param name="selfStr">Self string.</param>
        public static Transform ToTransform(this string selfStr)
        {
            return GameObject.Find(selfStr).transform;
        }
        #endregion

        /// <summary>
        /// 是否存在中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <returns>The first.</returns>
        /// <param name="str">String.</param>
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <returns>The first.</returns>
        /// <param name="str">String.</param>
        public static string LowercaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string ToUnixLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        /// <summary>
        /// 扩展方法来判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 查找在两个字符串中间的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="front"></param>
        /// <param name="behined"></param>
        /// <returns></returns>
        public static string FindBetween(this string str, string front, string behined)
        {
            var startIndex = str.IndexOf(front) + front.Length;
            var endIndex = str.IndexOf(behined);
            if (startIndex < 0 || endIndex < 0)
                return str;
            return str.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 查找在字符后面的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="front"></param>
        /// <returns></returns>
        public static string FindAfter(this string str, string front)
        {
            var startIndex = str.IndexOf(front) + front.Length;
            return startIndex < 0 ? str : str.Substring(startIndex);
        }

        public static string TrimAll(this string str)
        {
            return str.Replace(" ", "");
        }

        /// <summary>
        /// 将Dictionary内容作为字符串输出
        /// </summary>
        public static string ToContentString<K, V>(this IDictionary<K, V> dict)
        {
            List<string> kvps = new List<string>();
            dict.ForEach_L(item =>
            {
                string perKVP = string.Format(" {0}->{1}", item.Key, item.Value);
                kvps.Add(perKVP);
            });
            return kvps.ToSplitString();
        }

        /// <summary>
        /// 将Dictionary字符串作为Dictionary输出
        /// </summary>
        /// <returns>The any type dic.</returns>
        /// <param name="str">String.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        /// <typeparam name="V">The 2nd type parameter.</typeparam>
        public static IDictionary<K,V> ToAnyTypeDic<K,V>(this string str)
        {
            Dictionary<K, V> result=new Dictionary<K, V>();
            List<string> kvps = str.ToAnyTypeList<string>();
            kvps.ForEach(item =>
            {
                List<string> kvp = item.Substring(1).ToAnyTypeList<string>("->");
                kvp[0] = kvp[0].TrimAll();
                result.Add((K)Convert.ChangeType(kvp[0], typeof(K)),(V)Convert.ChangeType(kvp[1],typeof(V)));
            });
            return result;
        }
    }

}
