// ========================================================
// Des：
// Author：yeyichen
// CreateTime：2018/05/19 11:51:16 
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
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class DescribeAttributeTool
    {
        static Dictionary<Type, Dictionary<string, string>> cache = new Dictionary<Type, Dictionary<string, string>>();

        public static string GetDescribe(this Type type, string fieldName)
        {
            if (!cache.ContainsKey(type))
            {
                Cache(type);
            }
            var fieldNameToDesc = cache[type];
            return fieldNameToDesc.ContainsKey(fieldName) ? fieldNameToDesc[fieldName] : string.Format("Can not found such desc for field `{0}` in type `{1}`", fieldName, type.Name);
        }

        static void Cache(Type type)
        {
            Dictionary<String, string> dict = new Dictionary<string, string>();
            cache.Add(type, dict);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var objs = field.GetCustomAttributes(typeof(DescribeAttribute), true);
                if (objs.Length > 0)
                {
                    dict.Add(field.Name, ((DescribeAttribute)objs[0]).des);
                }
            }
        }
    }
}
