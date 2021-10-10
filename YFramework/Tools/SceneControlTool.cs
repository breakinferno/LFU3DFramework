// ========================================================
// Des：
// Author：MIKASA
// CreateTime：2019/10/3 20:42:38
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFramework;
using YFramework.Extension;
using UnityEngine.SceneManagement;

public class SceneControlTool : QMonoSingleton<SceneControlTool> {

    //初始的场景
    public string initScene;
    Scene currentScene;

    public string target;

    private void Start()
    {
        SceneManager.LoadScene(initScene, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentScene = default;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        if (currentScene != default)
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
        currentScene = SceneManager.GetSceneByName(initScene);
        currentScene = scene;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    //[Button]
    //public void Load()
    //{
    //    LoadScene(target);
    //}
}
