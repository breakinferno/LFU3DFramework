// ========================================================
// Des：
// Author：yeyichen
// CreateTime：06/09/2018 22:35:52
// Version：v 1.0
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFramework;
using YFramework.Extension;
using System.Linq;
using UnityEditor;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;

public class YFrameworkTest : MonoBehaviour
{
    public VideoPlayer player;



    IEnumerator Get1()
    {
        string path = Application.dataPath+"/AssetBundle/test.ab";
        //第二种加载方式 LoadFromFile
        //异步加载
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
        yield return request;
        AssetBundle ab = request.assetBundle;
        //使用里面的资源
        VideoClip[] obj = ab.LoadAllAssets<VideoClip>();//加载出来放入数组中
        // 创建出来
        player.clip = obj[0];
        player.Play();
    }

    IEnumerator Get2()
    {
        string url = @"http://104.243.28.247/AssetBundle/test.ab";
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        uwr.SendWebRequest();//开始请求

        while (!uwr.isDone)
        {
            Debug.Log(uwr.downloadProgress);
            yield return null;
        }
        Debug.Log("Done");

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
            yield break;
        }
        Debug.Log(uwr.downloadedBytes);

        AssetBundle ab = ((DownloadHandlerAssetBundle)uwr.downloadHandler).assetBundle;
        //使用里面的资源
        VideoClip[] obj = ab.LoadAllAssets<VideoClip>();//加载出来放入数组中
        // 创建出来
        player.clip = obj[0];
        player.Play();
    }

    IEnumerator Get3()
    {
        string url = @"http://104.243.28.247/asdf.avi";
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        uwr.SendWebRequest();//开始请求

        while (!uwr.isDone)
        {
            Debug.Log(uwr.downloadProgress);
            yield return null;
        }
        Debug.Log("Done");

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
            yield break;
        }
        DownloadHandler dh = uwr.downloadHandler;
        FileTool.WriteOrCreateFile("asdf.avi", dh.data, "StreamingAssets",FilePathType.data);
        ////使用里面的资源
        //VideoClip[] obj = ab.LoadAllAssets<VideoClip>();//加载出来放入数组中
        //// 创建出来
        //player.clip = obj[0];
        //player.Play();
    }
}
