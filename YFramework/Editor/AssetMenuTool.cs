// ========================================================
// Des：Asset Menu工具
// Author：yeyichen
// CreateTime：2018/05/23 10:24:37 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 20?? xuanyusong
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
    using System.Collections;
    using UnityEditor;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using UnityEditor.SceneManagement;
    using System.Collections.Generic;
    using System;
    using System.Reflection;
    using Object = UnityEngine.Object;

    public class FindReferences
    {
        #region 找出资源的使用情况
        [MenuItem("Assets/AssetMenuTool/Find References/Common Resources")]
        static void Find()
        {
            EditorSettings.serializationMode = SerializationMode.ForceText;
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!string.IsNullOrEmpty(path))
            {
                string guid = AssetDatabase.AssetPathToGUID(path);
                string withoutExtensions = "*.prefab*.unity*.mat*.asset";
                string[] files = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories)
                    .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();
                int startIndex = 0;

                EditorApplication.update = delegate ()
                {
                    string file = files[startIndex];

                    bool isCancel = EditorUtility.DisplayCancelableProgressBar("匹配资源中", file, (float)startIndex / (float)files.Length);

                    if (Regex.IsMatch(File.ReadAllText(file), guid))
                    {
                        Debug.Log(file, AssetDatabase.LoadAssetAtPath<Object>(GetRelativeAssetsPath(file)));
                    }

                    startIndex++;
                    if (isCancel || startIndex >= files.Length)
                    {
                        EditorUtility.ClearProgressBar();
                        EditorApplication.update = null;
                        startIndex = 0;
                        Debug.Log("匹配结束");
                    }

                };
            }
        }

        [MenuItem("Assets/AssetMenuTool/Find References/Common Resources", true)]
        static bool VFind()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return (!string.IsNullOrEmpty(path));
        }

        static string GetRelativeAssetsPath(string path)
        {
            return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
        }
        #endregion

        #region 复制资源路径到剪贴板  
        [MenuItem("Assets/AssetMenuTool/Copy Relative Path")]
        static void CopyRelativePath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeInstanceID);

            //去除 Asset/Resources/ 的前缀
            path = path.Replace("Assets/Resources/", "");

            //如果有后缀名，去除后缀名
            if (path.Contains("."))
                path = path.Substring(0, path.LastIndexOf('.'));

            TextEditor text2Editor = new TextEditor();
            text2Editor.text = path;
            text2Editor.OnFocus();
            text2Editor.Copy();
        }

        [MenuItem("Assets/AssetMenuTool/Copy Relative Path", true)]
        static bool VCopyRelativePath()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return (!string.IsNullOrEmpty(path));
        }
        #endregion

        #region 找出预制体在所有场景中的使用情况,会打开所有场景。
        [MenuItem("Assets/AssetMenuTool/Find References/Prefab")]
        static void SearchForPrefabReferences()
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            int num = 0;
            //遍历所有游戏场景
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    //打开场景
                    EditorSceneManager.OpenScene(scene.path);
                    //获取场景中的所有游戏对象
                    GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
                    foreach (GameObject go in gos)
                    {
                        //判断GameObject是否为一个Prefab的引用
                        if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                        {
                            UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource(go);
                            string path = AssetDatabase.GetAssetPath(parentObject);
                            //判断GameObject的Prefab是否和右键选择的Prefab是同一路径。
                            if (path == AssetDatabase.GetAssetPath(Selection.activeGameObject))
                            {
                                //输出场景名，以及Prefab引用的路径
                                Debug.Log(scene.path + "  " + GetGameObjectPath(go));
                                num++;
                            }
                        }
                    }
                }
            }
            Debug.Log(string.Format("共找到{0}处引用", num));
        }

        static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        [MenuItem("Assets/AssetMenuTool/Find References/Prefab", true)]
        static bool VSearchForPrefabReferences()
        {
            return Selection.gameObjects.Length == 1;
        }
        #endregion

        #region 导出预制体以及所有关联文件为unityPackage
        [MenuItem("Assets/AssetMenuTool/Export Asset")]
        static void Build()
        {
            if (Selection.objects == null)
                return;
            List<string> paths = new List<string>();
            foreach (Object o in Selection.objects)
            {
                paths.Add(AssetDatabase.GetAssetPath(o));
            }

            if (!Directory.Exists(Application.dataPath + "/UnityPackage"))
            {
                //不存在就创建目录
                Directory.CreateDirectory(Application.dataPath + "/UnityPackage");
            }

            AssetDatabase.ExportPackage(paths.ToArray(), "Assets/UnityPackage/" + Selection.objects[0].name + "Andxxx.unitypackage", ExportPackageOptions.IncludeDependencies);
            AssetDatabase.Refresh();
            Debug.Log("Build all Done!");
        }
        #endregion

        #region 打包AssetBundle
        [MenuItem("Assets/AssetMenuTool/Pack AssetBundle")]
        static void PackAssetBundle()
        {
            string targetPath= Application.dataPath + "/AssetBundle";
            if (!Directory.Exists(targetPath))
            {
                //不存在就创建目录
                Directory.CreateDirectory(targetPath);
            }

            BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            AssetDatabase.Refresh();
            Debug.Log("Pack all Done!");
        }
        #endregion
    }
}
