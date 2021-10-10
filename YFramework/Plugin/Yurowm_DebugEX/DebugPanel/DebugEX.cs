using UnityEngine;
using System.Collections;

//用于外部调用
public class DebugEX : MonoBehaviour {

	static int index=0;

	//打印事件
	public static void LogEvent(string eventText)
	{
		System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
		string[] nameArray=st.GetFrame(0).GetFileName().Split('/');
		string name=nameArray[nameArray.Length-1];
		string line=st.GetFrame(0).GetFileLineNumber().ToString();
		System.DateTime now=System.DateTime.Now;
		DebugPanel.Log("["+index.ToString()+"]"+now.ToString() , "Event" , eventText+"\t"+name+"  "+line);
		index++;
	}

	//打印变量
	public static void LogPara(string name,object o)
	{
		DebugPanel.Log(name,o);
	}
	public static void LogPara(string name,string category,object o)
	{
		DebugPanel.Log(name,category,o);
	}
}
