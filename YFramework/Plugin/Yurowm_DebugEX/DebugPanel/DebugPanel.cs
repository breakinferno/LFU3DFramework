using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YFramework;

//[ExecuteInEditMode]
public class DebugPanel : MonoBehaviour {

	static DebugPanel mMain;
	public static DebugPanel main {
		get {
			if (!mMain)
				TryToFindPanel();
			return mMain;
		}
	}

	readonly public Dictionary<string, Field> parameters = new Dictionary<string, Field>();
	readonly public Dictionary<string, bool> categories = new Dictionary<string, bool> ();

	List<string> keys = new List<string> ();
	
//	[HideInInspector]
	public bool hideInEditMode = false;
//	[HideInInspector]
	public bool visible = true;
//	[HideInInspector]
	public bool hideOnAwake = true;
//	[HideInInspector]
	public bool showFPS = true;
//	[HideInInspector]
	public float lockAlphaMultiplier = 0.5f;
//	[HideInInspector]
	public bool ignoreDefLog = false;
//	[HideInInspector]
	public bool ignoreDefLogByDefault = false;

	GUIContent content = new GUIContent();

	bool isShow = false;

	// FPS
	float fpsUpdateDelay = 0.2f;
	float fpsTime = 0f;
	int fps = 0;

	// Colors
	[HideInInspector]
	public Color bgColor = new Color(0, 0.5f, 1, 0.5f);
	Color defaultColor;

	void Awake () {
		Application.RegisterLogCallback(HandleLog);
		transform.SetSiblingIndex (0);
		IngorDefLog(ignoreDefLogByDefault);
		if (visible && !(hideInEditMode && Application.isEditor)) gameObject.AddComponent<DebugPanelViewport> ();
		//DontDestroyOnLoad(this.gameObject);
	}

	public static void Log (string name, object o) {
		Log (name, "", o);
		}
	
	public static void Log (string name, string category, object o) {
		#if UNITY_EDITOR
		Log (name, category, UnityEngine.StackTraceUtility.ExtractStackTrace(), o);
		#else
		Log (name, category, "", o);
		#endif
		}

	public static void Log (string name, string category, string trace, object o) 
	{
		if (!main.parameters.ContainsKey(name))
		{
			main.parameters.Add(name, new Field());
		}

		main.parameters [name].name = name;
		main.parameters [name].value = o.ToString ();
		main.parameters [name].category = category;
		main.parameters [name].trace = trace;

		if (!main.categories.ContainsKey(category))
		{
			main.categories.Add (category, true);
		}
	}

	public static void Break (string name) {
		if (main.parameters.ContainsKey(name))
			main.parameters.Remove(name);
	}

	public static void Clear () {
		main.parameters.Clear ();
		main.categories.Clear ();
	}

	public static void IngorDefLog (bool i)
	{
		main.ignoreDefLog = i;
		if (main.ignoreDefLog) {
			if (main.categories.ContainsKey("Error")) main.categories.Remove("Error");
			if (main.categories.ContainsKey("Warning")) main.categories.Remove("Warning");
			if (main.categories.ContainsKey("Default Log")) main.categories.Remove("Default Log");
		}

		List<string> keys = new List<string> (main.parameters.Keys);
		foreach (string key in keys) {
			if (IsDeflog(main.parameters[key].category))
				Break(key);
		}
	}

	public static void TurnAll (bool vis) {
		main.keys = new List<string> (main.categories.Keys);
		foreach (string key in main.keys)
			main.categories[key] = vis;
	}

	static void TryToFindPanel() {
		DebugPanel panel;
		panel = FindObjectOfType<DebugPanel> ();
		if (!panel) {
			GameObject go = new GameObject();
			go.name = "DebugPanel";
			panel = go.AddComponent<DebugPanel>();
		}
		mMain = panel;
	}

	void OnEnable() {
		Application.RegisterLogCallback(HandleLog);
	}
	void OnDisable() {
		Application.RegisterLogCallback(null);
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		if (ignoreDefLog) return;
		switch (type) {
		case LogType.Exception:
		case LogType.Error: 
			HandleError(logString,stackTrace);
			break;

		case LogType.Warning: 
			break;

		}
	}

	void HandleError(string logString, string stackTrace)
	{
		System.DateTime now=System.DateTime.Now;
		Log(now.ToString(), "Error", stackTrace,logString+" "+stackTrace);
	}

	void Update () {
		if (showFPS) {
			if (fpsTime + fpsUpdateDelay < Time.unscaledTime) {
				fpsTime = Time.unscaledTime;
				fps = Mathf.RoundToInt(1f / Time.unscaledDeltaTime);
				Log("FPS", "System", fps);
				#if UNITY_EDITOR
				Log("DrawCalls", "System", UnityEditor.UnityStats.drawCalls);
				Log("Triangles", "System", UnityEditor.UnityStats.triangles);
				#endif
			}
		}
	}

	public static bool IsDeflog(string c) {
		return c == "Error" || c == "Warning";
	}

	[System.Serializable]
	public class Field {
		public string name;
		public string value;
		public string category;
		public string trace;
	}

	//将所有归类为Event的信息打印到本地文件
	public static void WriteEventToLogFile()
	{
		string logContent="";
		foreach(Field item in main.parameters.Values)
		{
			if(item.category=="Event")
			{
				logContent+=item.name;
				logContent+=":";
				logContent+=item.value;
				logContent+="\n";
			}
		}
		for(int i=0;i<3;i++)
		{
			//空三行
			logContent+="\n";
		}
		FileTool.WriteOrCreateFile("LogFile"+System.DateTime.Now+".txt",logContent,"LogFile/",FilePathType.outsideData);

	}

	//将所有归类为Error的信息打印到本地文件
	public static void WriteEventToLogErrorFile()
	{
		string logContent="";
		foreach(Field item in main.parameters.Values)
		{
			if(item.category=="Error")
			{
				logContent+=item.name;
				logContent+=":";
				logContent+=item.value;
				logContent+="\n";
			}
		}
		for(int i=0;i<3;i++)
		{
			//空三行
			logContent+="\n";
		}
		FileTool.WriteOrCreateFile("LogErrorFile"+System.DateTime.Now+".txt",logContent,"LogErrorFile/",FilePathType.outsideData);
	}
}
