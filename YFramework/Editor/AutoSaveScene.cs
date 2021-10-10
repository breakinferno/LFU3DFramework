// ========================================================
// Des：自动保存场景,需保证该EditorWindow处于激活状态
// Author：yeyichen
// CreateTime：2018/05/19 10:53:07 
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
    using UnityEditor;
    using System;
    using UnityEditor.SceneManagement;

    public class AutoSaveScene : EditorWindow
    {

        private bool autoSaveScene = true;
        private bool showMessage = false;
        private bool isStarted = false;
        [Range(1, 3600)]
        private float interval = 300;
        private DateTime lastSaveTimeScene;
        private string scenePath;

        [MenuItem("YFramework/AutoSaveScene")]
        static void Init()
        {
            AutoSaveScene saveWindow = (AutoSaveScene)EditorWindow.GetWindow(typeof(AutoSaveScene));
            saveWindow.Show();
            saveWindow.autoRepaintOnSceneChange = true;
        }

        void OnEnable()
        {
            lastSaveTimeScene = DateTime.Now;
        }

        void OnGUI()
        {
            GUILayout.Label("Info:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Saving to:", "" + Application.dataPath);
            EditorGUILayout.LabelField("Saving scene:", "" + scenePath);
            GUILayout.Label("Options:", EditorStyles.boldLabel);
            autoSaveScene = EditorGUILayout.Toggle("Auto save", autoSaveScene);
            showMessage = EditorGUILayout.Toggle("Show Message", showMessage);
            interval = EditorGUILayout.FloatField("Interval (seconds)", interval);
            if (isStarted)
            {
                EditorGUILayout.LabelField("Last save:", "" + lastSaveTimeScene);
            }
        }

        void Update()
        {
            scenePath = EditorSceneManager.GetActiveScene().path;

            if (autoSaveScene)
            {
                if ((DateTime.Now - lastSaveTimeScene).TotalSeconds >= interval && scenePath != "")
                {
                    saveScene();
                }
            }
            else
            {
                isStarted = false;
            }
        }

        void saveScene()
        {
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            lastSaveTimeScene = DateTime.Now;
            isStarted = true;
            if (showMessage)
            {
                Debug.Log("AutoSave saved: " + scenePath + " on " + lastSaveTimeScene);
            }
        }

    }

}
