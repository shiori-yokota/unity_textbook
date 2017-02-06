using UnityEngine;
using System.Collections;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class QLearning : MonoBehaviour
{
	GameObject robot;

	int episode = 0;
	int action;
	double EPSILON;
	double GAMMA;
	double BETA;
	int new_row, new_col;
	int old_row, old_col;

	private bool walk;
	bool Colli;
	private Vector3 startPosition;
	private Vector3 endPosition;
	float distance;

	// Python
	ScriptEngine scriptEngine;
	ScriptScope scriptScope;
	ScriptSource scriptSource;

	void Start()
	{
		robot = GameObject.Find("RobotPy");
		walk = false;
		Colli = false;

		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/Moderator.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();

		// Pythonスクリプト実行エンジン
		scriptEngine = Python.CreateEngine();
		// 実行エンジンに渡す値を設定する
		scriptScope = scriptEngine.CreateScope();
		// pythonのソースを指定
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		// Moderator.pyのソースを実行する
		scriptSource.Execute(scriptScope);
		/* Moderator.pyを実行した結果を取得 */
		// epsilon-greedy法のパラメータを取得
		EPSILON = scriptScope.GetVariable<double>("EPSILON");
		// 割引率を取得
		GAMMA = scriptScope.GetVariable<double>("GAMMA");
		// 学習率の取得
		BETA = scriptScope.GetVariable<double>("BETA");

	}

	void Update()
	{
		if (walk)
		{
			distance += Time.deltaTime * 3;
			robot.transform.position = Vector3.MoveTowards(startPosition, endPosition, distance);
			if (Vector3.Distance(robot.transform.position, endPosition) < 0.1)
			{
				walk = false;
				old_col = new_col;
				old_row = new_row;
				startPosition = endPosition;
				int[] arr = position2rowcol(startPosition);
				Position(arr);
				UnityEngine.Debug.Log("行動し終わったので報酬を教えてもらう");
				GameObject moderator = GameObject.Find("GameObject");
				moderator.SendMessage("GetReward", Colli);
			}
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		walk = false;
		// 壁にぶつかったらもといた場所に戻る
		UnityEngine.Debug.Log("壁にぶつかった");
		// 今のpositionを取得
		Vector3 crr_pos = robot.transform.position;
		endPosition = startPosition;
		startPosition = crr_pos;
		walk = true;
		Colli = true;
	}

	void QLearning_start(bool Continue)
    {
		episode += 1;
		UnityEngine.Debug.Log("Start QLearning");
		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();
		

		scriptEngine = IronPython.Hosting.Python.CreateEngine();
		scriptScope = scriptEngine.CreateScope();
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("SIZE", 5);
		scriptScope.SetVariable("EPSILON", EPSILON);
		scriptScope.SetVariable("CONTINUE", Continue);
		scriptScope.SetVariable("EPISODE", episode);
		scriptScope.SetVariable("ROW", new_row);
		scriptScope.SetVariable("COL", new_col);
		scriptSource.Execute(scriptScope);

		// 選択した行動
		action = scriptScope.GetVariable<int>("ACT");
		UnityEngine.Debug.Log("action : " + action);
		// 行動が決まったので移動する
		Walk(action);
	}

	int[] position2rowcol(Vector3 pos)
	{
		int row = -(((int)pos.z + 1) / 2);
		int col = ((int)pos.x - 1) / 2;
		int[] arr = new int[] { row, col };
		UnityEngine.Debug.Log("new row col : (" + row + ", " + col + ")");
		return arr;
	}

	void Position(int[] arr)
	{
		new_row = arr[0];
		new_col = arr[1];
		robot.transform.position = new Vector3((new_col * 2) + 1, 1, -((new_row * 2) + 1));
		startPosition = robot.transform.position;

		UnityEngine.Debug.Log("startPosition : " + startPosition.x + ", " + startPosition.z);
	}

	/* 行動が決まったので移動する */
	void Walk(int act)
	{
		if (act == 0)
			endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + 2.0f);
		else if (act == 1)
			endPosition = new Vector3(startPosition.x + 2.0f, startPosition.y, startPosition.z);
		else if (act == 2)
			endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - 2.0f);
		else if (act == 3)
			endPosition = new Vector3(startPosition.x - 2.0f, startPosition.y, startPosition.z);
		else
			UnityEngine.Debug.Log("error : not difine endPosition");
		UnityEngine.Debug.Log("endPosition : " + endPosition.x + ", " + endPosition.z);
		walk = true;
	}

	void sendReward(double value)
	{
		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();


		scriptEngine = IronPython.Hosting.Python.CreateEngine();
		scriptScope = scriptEngine.CreateScope();
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("SIZE", 5);
		scriptScope.SetVariable("CONTINUE", true);
		scriptScope.SetVariable("EPSILON", EPSILON);
		scriptScope.SetVariable("GAMMA", GAMMA);
		scriptScope.SetVariable("BETA", BETA);
		scriptScope.SetVariable("REWARD", value);
		scriptScope.SetVariable("OLD_ROW", old_row);
		scriptScope.SetVariable("OLD_COL", old_col);
		scriptScope.SetVariable("NEW_ROW", new_row);
		scriptScope.SetVariable("NEW_COL", new_col);
		scriptScope.SetVariable("ACT", action);
		scriptSource.Execute(scriptScope);
	}
}