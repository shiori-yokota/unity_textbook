using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class RobotController3 : MonoBehaviour {
	GameObject robot;

	// python
	ScriptEngine scriptEngine;  // スクリプト実行用のScriptEngine
	ScriptScope scriptScope;    // スクリプトに値を渡すためのScriptScope
	ScriptSource scriptSource;  // スクリプトのソースを指定するためのScriptSource

	string script;
	string filename;
    
	List<string> stateList = new List<string> { };
	List<int> actionList = new List<int> { };
	Dictionary<List<string>, List<int>> StateAction = new Dictionary<List<string>, List<int>>();

	Vector3 startPosition = new Vector3();
	Vector3 endPosition = new Vector3();
	bool walk = false;
	float distance;

	// Use this for initialization
	void Start () {
		robot = GameObject.Find("RobotPy");
		filename = Application.dataPath + "/../Python/Chapter3/Astar.py";
		setStateAction();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S))
        {
            startPosition = robot.transform.position;
            UnityEngine.Debug.Log("START A* ALGORITHM");
            Astar();
        }

        if (walk)
        {
            distance += Time.deltaTime * 3.0f;
            robot.transform.position = Vector3.MoveTowards(startPosition, endPosition, distance);
            if (Vector3.Distance(robot.transform.position, endPosition) < 0.1)
            { // 移動完了
                // UnityEngine.Debug.Log("move finish");
                walk = false;
                distance = 0.0f;
                startPosition = endPosition;
                Walking();
            }
        }
	}

	void Astar()
	{
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
		{
			script = sr.ReadToEnd();
		}
		scriptEngine = Python.CreateEngine();       // Pythonスクリプト実行エンジン						
		scriptScope = scriptEngine.CreateScope();       // 実行エンジンに渡す値を設定する					
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);       // pythonのソースを指定										

		// IronPythonで実装されているリスト
		var openlist = new IronPython.Runtime.List { "S" };
		var closedlist = new IronPython.Runtime.List { };

		// 初期状態をOpenListに入れ、Closedリストを空に初期化する
		scriptScope.SetVariable("OPENLIST", openlist);
		scriptScope.SetVariable("GOAL", "G");
		scriptScope.SetVariable("CLOSEDLIST", closedlist);

		scriptSource.Execute(scriptScope);      // ソースを実行する

		var Result = scriptScope.GetVariable<IronPython.Runtime.List>("CLOSEDLIST");
		// ロボットが移動します
		stateList = Result.Cast<string>().ToList();
		moveTheRobot();
	}

	void moveTheRobot()
	{
		if (stateList.Count > 1)    // 最後のstateはゴール
		{
			// state[i] はロボットの状態 state[i + 1]はロボットの次の状態
            List<string> NowNext = new List<string> { stateList[0], stateList[1] };
            actionList = getActionNum(NowNext);
			Walking();
			stateList.RemoveAt(0);  // 移動したのでロボットの状態を更新
		} else UnityEngine.Debug.Log("Finish");
	}

	List<int> getActionNum(List<string> name)
	{
		List<int> act = new List<int>();
        int count = 0;
		foreach (List<string> key in StateAction.Keys)
		{
            // for (int i = 0; i < key.Count; i++) UnityEngine.Debug.Log("key:" + key[i]);
            // UnityEngine.Debug.Log(" :" + key.SequenceEqual(name));
            // 完全一致
            if (key.SequenceEqual(name))
            {
                act = StateAction[key];
                break;
            } else count++;
		}
        if (count >= StateAction.Count) act.Add(-1);
		return act;
	}

	void Walking()
	{
		if (actionList.Count > 0)
		{
            int action = actionList[0];
            // UnityEngine.Debug.Log(action);
            if (action == 0) {
				endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + 2.0f);
				walk = true;
			} else if (action == 1) {
				endPosition = new Vector3(startPosition.x + 2.0f, startPosition.y, startPosition.z);
				walk = true;
			} else if (action == 2) {
				endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - 2.0f);
				walk = true;
			} else if (action == 3) {
				endPosition = new Vector3(startPosition.x - 2.0f, startPosition.y, startPosition.z);
				walk = true;
			} else {
				UnityEngine.Debug.Log("error : not difine next position");
				walk = false;
			}
			actionList.RemoveAt(0);
		}
		else
		{
			// UnityEngine.Debug.Log("arrived next state");
			moveTheRobot();
		}
	}

    List<int> Return(List<int> action)
    {
        List<int> act = new List<int>();
        for (int i = 0; i < action.Count; i++)
        {
            if (action[i] == 0) act.Add(2);
            else if (action[i] == 1) act.Add(3);
            else if (action[i] == 2) act.Add(0);
            else if (action[i] == 3) act.Add(1);
            else act.Add(-1);
        }
        return act;
    }

	void Teleportation()
	{
		robot.transform.position = new Vector3(endPosition.x + 0.5f, robot.transform.position.y, endPosition.z - 0.5f);
		moveTheRobot();
	}

	void setStateAction()
	{
		StateAction.Add(new List<string> { "S", "S3" }, new List<int> { 1, 2 });
		StateAction.Add(new List<string> { "S3", "S4" }, new List<int> { 1, 1, 1 });
		StateAction.Add(new List<string> { "S4", "S1" }, new List<int> { 0, 3, 3 });
		StateAction.Add(new List<string> { "S4", "S6" }, new List<int> { 2, 1 });
		StateAction.Add(new List<string> { "S6", "S2" }, new List<int> { 0, 0 });
		StateAction.Add(new List<string> { "S6", "G" }, new List<int> { 2, 3, 2, 1, 1 });
		StateAction.Add(new List<string> { "S3", "S7" }, new List<int> { 2, 2, 1 });
		StateAction.Add(new List<string> { "S7", "S8" }, new List<int> { 1 });
		StateAction.Add(new List<string> { "S7", "S9" }, new List<int> { 2, 3 });
		StateAction.Add(new List<string> { "S8", "S5" }, new List<int> { 0, 3 });
		StateAction.Add(new List<string> { "S8", "S10" }, new List<int> { 2 });

        StateAction.Add(new List<string> { "S3", "S" }, new List<int> { 0, 3 });
        StateAction.Add(new List<string> { "S4", "S3" }, new List<int> { 3, 3, 3 });
        StateAction.Add(new List<string> { "S1", "S4" }, new List<int> { 1, 1, 2 });
        StateAction.Add(new List<string> { "S6", "S4" }, new List<int> { 3, 0 });
        StateAction.Add(new List<string> { "S2", "S6" }, new List<int> { 2, 2 });
        StateAction.Add(new List<string> { "G", "S6" }, new List<int> { 3, 3, 0, 1, 0 });
        StateAction.Add(new List<string> { "S7", "S3" }, new List<int> { 3, 0, 0 });
        StateAction.Add(new List<string> { "S8", "S7" }, new List<int> { 3 });
        StateAction.Add(new List<string> { "S9", "S7" }, new List<int> { 1, 0 });
        StateAction.Add(new List<string> { "S5", "S8" }, new List<int> { 1, 2 });
        StateAction.Add(new List<string> { "S10", "S8" }, new List<int> { 0 });
    }
}
