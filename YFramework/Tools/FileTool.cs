// ========================================================
// Des：文件操作工具
// Author：yeyichen
// CreateTime：2018/05/19 10:53:07 
// Version：v 1.2
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
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
    using System;

    public enum FilePathType
    {
        /// <summary>
        /// 永久储存路径，每个操作系统都不一样
        /// OSPersistentPath/[CompanyName]/[ProductName]/
        /// </summary>
        persist,
        /// <summary>
        /// 于Assets文件夹同级
        /// [ProductName]/
        /// </summary>
        outsideData,
        /// <summary>
        /// Assets文件夹中
        /// [ProductName]/Assets
        /// </summary>
        data,
        /// <summary>
        /// 默认路径，Editor状态下在Assets中，其他都在永久储存路径下
        /// </summary>
        defaultPath,
        /// <summary>
        /// 直接绝对路径，不对路径做任何改变
        /// </summary>
        absolutePath
    }

    /// 文件操作类
    public class FileTool
    {
        static string CheckPath(string path,FilePathType pathType)
        {
            if (!string.IsNullOrEmpty(path))
            {
                //反斜杠换为斜杠
                path.Replace('\\','/');
                
                //保证路径后有/
                if( !path.EndsWith("/"))
                {
                    path += '/';
                }

                //如果不是绝对路径，去掉前面的/
                if(path.StartsWith("/") && pathType!=FilePathType.absolutePath)
                {
                    path=path.Remove(0, 1);
                }
            }

            //没有路径就创建一个
            string filePath = GetRootPath(pathType) + path;
            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return path;
        }

        #region 路径
        
        static readonly string persistentDataPath = Application.persistentDataPath + "/";

        //asset外
        public static readonly string outsideDataPath = Application.dataPath.Replace("Assets", "");

        public static readonly string dataPath = Application.dataPath + "/";

        //Editor环境下保存在Asset文件夹外，否则在永久地址中
        public static string defaultRootPath
        {
            get
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                {
                    return dataPath;
                }
                else
                {
                    return persistentDataPath;
                }
            }
        }

        static string GetRootPath(FilePathType type)
        {
            switch (type)
            {
                case FilePathType.persist:
                    return persistentDataPath;

                case FilePathType.outsideData:
                    return outsideDataPath;

                case FilePathType.data:
                    return dataPath;

                case FilePathType.defaultPath:
                    return defaultRootPath;

                case FilePathType.absolutePath:
                    return "";
            }
            return "";
        }

        public static string AbsolutePath2AssetPath(string path)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return path.Substring(path.IndexOf("Assets\\"));
            else
                return path.Substring(path.IndexOf("Assets/"));
        }

        public static string AssetPath2AbsolutePath(string path)
        {
            return Application.dataPath + path.Substring(path.IndexOf('/'));
        }
        #endregion

        #region 字符类型的文件
        /// <summary>
        /// 写文件操作,指定路径文件不存在会被创建
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="info">Info.</param>
        /// <param name="path">Path.</param>
        /// <param name="pathType">Path type.</param>
        public static void WriteOrCreateFile(string fileName, string info, string path = "", FilePathType pathType = FilePathType.defaultPath,bool ifDebug=true)
        {
            if(string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("要写入的文件名不能为空");
                return;
            }

            path=CheckPath(path,pathType);

            string filePath = GetRootPath(pathType) + path + fileName;

            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            fs.SetLength(0);    ///*清空文件*/
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
            if (ifDebug)
            {
               Debug.Log("写入" + filePath + "成功!");
            }
        }

        /// <summary>
        /// 读取文件内容,list中一个元素一行
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        public static List<string> ReadStringFile(string fileName, string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("要读取的文件名不能为空");
                return null;
            }

            path = CheckPath(path, pathType);

            string lineContent;
            string filePath = GetRootPath(pathType) + path + fileName;

            StreamReader sr = null;
            try
            {
                sr = File.OpenText(filePath);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }

            List<String> result = new List<string>();
            while ((lineContent = sr.ReadLine()) != null)
            {
                result.Add(lineContent);
            }
            sr.Close();
            sr.Dispose();
            return result;
        }

        /// <summary>
        /// 读取所有文件内容
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        public static string ReadAllString(string fileName, string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("要读取的文件名不能为空");
                return null;
            }

            path = CheckPath(path, pathType);

            string filePath = GetRootPath(pathType) + path + fileName;

            StreamReader sr = null;
            try
            {
                sr = File.OpenText(filePath);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }

            string result = sr.ReadToEnd();
            
            sr.Close();
            sr.Dispose();
            return result;
        }
        #endregion

        #region 二进制文件
        /// <summary>
        /// 写文件操作,指定路径文件不存在会被创建
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="info">Info.</param>
        /// <param name="path">Path.</param>
        /// <param name="pathType">Path type.</param>
        public static void WriteOrCreateFile(string fileName, byte[] info, string path = "", FilePathType pathType = FilePathType.defaultPath,bool ifDebug=true)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("要写入的文件名不能为空");
                return;
            }

            path = CheckPath(path, pathType);

            string filePath = GetRootPath(pathType) + path + fileName;

            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            fs.SetLength(0);    ///*清空文件*/
            fs.Write(info, 0, info.Length);
            fs.Close();
            fs.Dispose();
            if (ifDebug)
            {
               Debug.Log("写入" + filePath + "成功!");
            }
        }

        /// <summary>
        /// 读取所有文件内容
        /// </summary>
        /// <returnsf>The file.</returns>
        /// <param name="fileName">File name.</param>
        public static byte[] ReadAllByte(string fileName, string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("要读取的文件名不能为空");
                return null;
            }

            path = CheckPath(path, pathType);

            string filePath = GetRootPath(pathType) + path + fileName;

            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }

            byte[] result = new byte[fs.Length];
            fs.Read(result, 0, (int)fs.Length);

            fs.Close();
            fs.Dispose();
            return result;
        }
        #endregion

        public static bool IsFileExists(string fileName, string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            path = CheckPath(path, pathType);

            string filePath = GetRootPath(pathType) + path + fileName;
            return File.Exists(filePath);
        }

        public static void DeleteFile(string fileName, string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            path = CheckPath(path, pathType);

            string filePath = GetRootPath(pathType) + path + fileName;
            File.Delete(filePath);
        }

        public static void RenameFile(string sourceName,string targetName,string path="", FilePathType pathType = FilePathType.defaultPath)
        {
            path = CheckPath(path, pathType);

            string sourcePath = GetRootPath(pathType) + path + sourceName;
            string targetPath = GetRootPath(pathType) + path + targetName;

            FileInfo fi = new FileInfo(sourcePath);
            fi.MoveTo(targetPath);
        }

        /// <summary>
        /// 复制文件夹，需要传入文件夹的绝对路径
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public static void CopyFolder(string from, string to)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            ///* 子文件夹*/
            foreach (string sub in Directory.GetDirectories(from))
                CopyFolder(sub, to + Path.GetFileName(sub) + "/");

            ///* 文件*/
            foreach (string file in Directory.GetFiles(from))
                File.Copy(file, to + Path.GetFileName(file), true);
        }

        public static string RemoveSuffix(string fileName,string split=".")
        {
            return fileName.Substring(0,fileName.LastIndexOf(split));
        }

        /// <summary>
        /// 如果文件夹存在那么不动，否则创建
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathType"></param>
        public static void CreateFolderIfNotExist(string path = "", FilePathType pathType = FilePathType.defaultPath)
        {
            CheckPath(path, pathType);
        }
        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <returns>The list stitch.</returns>
        /// <param name="list">List.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string StringListStitch<T>(IList<T> list)
        {
            string result = "";
            foreach(T item in list)
            {
                result += item.ToString();
            }
            return result;
        }
    }

}
