************************************************************
将build settings->player settings->configuration->Scriping RunTime Version改为.Net 4.x，否则会报错或部分功能能无法使用
************************************************************
YFramework V1.3
{
	//TODO
	（1）其他格式输出（图片/音频/视频等）
	（2）assetbundle工具类

	1.各种格式的文件硬盘写入（暂时只支持.wav格式）

	2.FileTool改进，包括路径合成，二进制写入等

	3.云主机键值对数据库

	4.一张格子测试图片
}

YFramework V1.2正式版
{
    1.改进CodeRunTimeTool工具，可以自定义名字并添加暂停功能(未测试)

    2.精简了一下，去掉了一些插件中的例子

    3.改进了SerializableDictionary,优化了字典字符串，普通字典和可序列化字典的相互转化

    4.添加ComputeShader例子

	5.添加了OtherExtension

	6.修复了Object.IsNull（）的bug

	7.修复了未添加批处理导致的无法打包的bug
}

YFramework V1.1正式版
{
    1.GizmosTool
    运行时的Gizmos工具，可以使用局部变量绘制gizmos而不必须在OnDrawGizmos中绘制

    2.dictionary的测试功能
    {
        Assets/Scripts/YFramework/Plugin/Map 高速Map
        Assets/Scripts/YFramework/Extension/DotNet/SerializableDictionary 可序列化的Dictionary
    }

    3.安全的delegate调用（空判断）

    4.hub字显示

    5.ShowIf的自定义类嵌套调用(方法暂不支持)

    6.新对象池(自己的代码用着安心)

    7.清晰化example
}

YFramework V1.0正式版
{
    *Example见YFrameworkExample，随便找个物体挂上去就行

    ==========================Editor==========================
    1.Asset Menu工具(Assets/YFramework/Editor/AssetMenuTool)
    {
        AssetMenuTool/Copy Relative Path:复制资源路径到剪贴板
        AssetMenuTool/Find References/Common Resources:资源在所有场景中的使用情况
        AssetMenuTool/Find References/Prefab:找出预制体在所有场景中的使用情况
        AssetMenuTool/Export Asset:导出预制体以及所有关联文件为unityPackage
    } 

    2.自动保存场景(Assets/YFramework/Editor/AutoSaveScene)
    {
        需要YFramework/AutoSaveScene窗口处于打开状态
    }

    3.所有资源的详细使用情况(Assets/YFramework/Editor/ResourceChecker)
    { 
        只能看到当前场景的资源使用情况
    }

    4.其他（Assets/YFramework/Editor/EditorSetting）
    {
        代码模板导入时的修改（添加作者、时间等信息）
        使用Object扩展方法debug时的代码行重定位
    }



    ==========================YInspector==========================
    //同一个字段使用不同Attribute时可能会有冲突，比如[DisableEdit]和[ShowIf]，以后面的为准
    1.显示方法为按钮
    [Button]

    2.代码描述（也可以描述Enum等）
    [Describe]

    3.使不可编辑
    [DisableEdit]

    4.字段中文化
    [Label]

    5.可排序的IList
    [Reorderable]

    6.在特定情况下显示字段
    [ShowIf]



    ==========================插件==========================
    //Assets/YFramework/Plugin/
    //有的不太会用

    1.音效管理
    AudioToolkit

    [Legcy]
    2.对象池
    PathologicalGames/PoolManager

    3.动画插件
    DOTween

    4.高速Dictionary
    Map

    5.Hierarchy显示工具
    QHierarchy

    6.单例
    Singleton

    [legcy]
    7.解压缩工具
    Zip

    8.真机打包Debug工具(还没整合)
    Yurowm_DebugEX


    ==========================功能扩展==========================
    //YFramework/Extension
    //扩展了一些常用类的常用方法，部分方法(方法名带_L后缀的)支持链式编程。详见每个类中的Example

    1.数组、列表、字典扩展(DotNet/IEnumerableExtension)
    {
        匿名方法的便捷foreach写法
        list的逆序foreach
        list的元素交换
        Dictionary的合并
    }

    2.字符串扩展(DotNet/StringExtension)
    {
        字符串与各类型间的相互转换
        实现IStringableObject接口可以自定义转换方法
        打印Dictionary内容 TODO:相互转化
        其他一些扩展方法
    }

    3.反射扩展(DotNet/Reflection)
    {
        ??
    }

    4.MonoBehaviour扩展(Unity/BehaviourExtension)
    {
        显示隐藏
        动态加载资源
    }

    5.GameObject扩展(Unity/GameObjectExtension)
    {
        显示隐藏
        设置Layer、Tag
    }

    6.Object扩展(Unity/ObjectExtension)
    {
        实例化
        设置名字
        销毁
        匿名方法
        Debug
    }

    7.Transform扩展(Unity/TransformExtension)
    {
        递归找各种子父物体上的东西
        生成路径以及根据路径查找
        便捷设置Vector的单一参数
    }

    8.协程内开启线程(Unity/WaitForThreadedTask)
    {
        用于处理一些协程不太好处理的耗时任务，不过需要自处理线程开关
    }



    ==========================工具==========================
    1.AutoLOD
        动态减面算法，还没完工

    2.CodeRunTimeTool
        一段代码的耗时

    3.ConverNumTool
        ？？

    4.CreateCodeTool
        用代码创建代码（ScriptTemplate）中用到

    5.CSVData
        CSV文件的读写（文件格式见Tools/CSVData）

    6.EventPoolTool
        事件池，永久存在，切换场景时清理下

    7.FbxAnimListPostprocessor
        导入FBX时自动切动画

    8.FileTool
        文件操作工具

    9.LoadSceneTool
        场景异步加载

    10.Math3dTool
        3D数学工具类

    11.RandomTool
        随机函数封装

    12.TimeTool
        获取服务器时间，网络不可用时获取本地时间
}

YFramework 预计添加功能：
1.YInspector功能添加(自定义类嵌套使用)
{
    第一批功能(EditorGUILayout中的基础功能)：IntPopup、passwordField等各种Field
    第二批功能(将field分组排列的)：FadeGroup等各种Group
}
2.Debug和DebugEX整合，添加debug优先级
3.YFrameworkManager功能完善
4.完善CSV工具的写入功能（自动路径）。
5.争取解决Inspector上的冲突问题
6.合并高速map和可序列化dictionary