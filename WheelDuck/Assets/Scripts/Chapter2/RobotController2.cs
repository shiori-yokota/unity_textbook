using UnityEngine;
using System.Collections;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class RobotController2 : MonoBehaviour {

	// python
	ScriptEngine scriptEngine;  // スクリプト実行用のScriptEngine
	ScriptScope scriptScope;    // スクリプトに値を渡すためのScriptScope
	ScriptSource scriptSource;  // スクリプトのソースを指定するためのScriptSource

	string script;
	string depthFile = Application.dataPath + "/../Python/Chapter2/DepthFirstController.py";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.D))
		{       //Depth-First
			startDepthFirstSearch();
		}
		else if (Input.GetKeyDown(KeyCode.B))
		{       //Breadth-First
			startBreadthFirstSearch();
		}
	}

	void startDepthFirstSearch()
	{
		using (StreamReader sr = new StreamReader(depthFile, System.Text.Encoding.UTF8))
		{
			script = sr.ReadToEnd();
		}
		scriptEngine = Python.CreateEngine();       // Pythonスクリプト実行エンジン						
		scriptScope = scriptEngine.CreateScope();       // 実行エンジンに渡す値を設定する					
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);       // pythonのソースを指定										
		scriptSource.Execute(scriptScope);      // Moderator7.pyのソースを実行する

	}

	void startBreadthFirstSearch()
	{

	}
}
