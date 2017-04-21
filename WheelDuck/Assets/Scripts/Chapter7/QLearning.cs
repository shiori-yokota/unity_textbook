using UnityEngine;
using System.Collections;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class QLearning : MonoBehaviour
{
	GameObject robot;
	GameObject moderator;

	int episode = 0;
	int step = 0;
	int action;
	double EPSILON;
	double GAMMA;
	double BETA;
	double GOAL;
	int MazeSize;
	int new_row, new_col;
	int old_row, old_col;

	private bool walk;
	bool Colli;
    bool stop = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	float distance;

	string Mode;

	// Python
	ScriptEngine scriptEngine;
	ScriptScope scriptScope;
	ScriptSource scriptSource;

	void Start()
	{
		robot = GameObject.Find("RobotPy");
		moderator = GameObject.Find("GameObject");
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
		// Moderator7.pyのソースを実行する
		scriptSource.Execute(scriptScope);
		/* Moderator7.pyを実行した結果を取得 */
		// epsilon-greedy法のパラメータを取得
		EPSILON = scriptScope.GetVariable<double>("EPSILON");
		// 割引率を取得
		GAMMA = scriptScope.GetVariable<double>("GAMMA");
		// 学習率の取得
		BETA = scriptScope.GetVariable<double>("BETA");
		// 迷路のサイズ
		MazeSize = scriptScope.GetVariable<int>("SIZE");
		GOAL = scriptScope.GetVariable<double>("GOAL_REWARD");
	}

	void Update()
	{
        if (walk)
        {
            distance += Time.deltaTime * 3;
            robot.transform.position = Vector3.MoveTowards(startPosition, endPosition, distance);
            if (Vector3.Distance(robot.transform.position, endPosition) < 0.1)
            { // 移動完了
                moderator.SendMessage("RobotCollision", Colli);
                distance = 0.0f;
                walk = false;
                if (stop) STOP();
                else walkFinish();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("************************************");
            stop = true;
        }
        
	}

	void OnCollisionEnter(Collision collision)
	{
		walk = false;
		Colli = true;
		moderator.SendMessage("RobotCollision", Colli);
		distance = 0.0f;
		// 壁にぶつかったらもといた場所に戻る
		endPosition = startPosition;
		returnPosition(endPosition);
	}

	void GameMode(string mode)
	{
		Mode = mode;
	}

	void returnPosition(Vector3 pos)
	{
		UnityEngine.Debug.Log("壁にぶつかった:もといた座標に戻る");
		robot.transform.position = pos;
		walkFinish();
	}

	void walkFinish()
	{
		int[] olds = position2rowcol(startPosition);
		old_row = olds[0];
		old_col = olds[1];
		int[] news = position2rowcol(endPosition);
		new_row = news[0];
		new_col = news[1];
		if (Mode == "learn")
		{
			UnityEngine.Debug.Log("行動し終わったので報酬を教えてもらう");
			moderator.SendMessage("GetReward", news);
		} else if (Mode == "move")
		{
			UnityEngine.Debug.Log("行動し終わったのでゴールかどうかを教えてもらう");
			moderator.SendMessage("CheckPosition", news);
		}

		Colli = false;
	}

	void QLearning_start(bool init)
    {
		episode += 1;
		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();
		
		scriptEngine = IronPython.Hosting.Python.CreateEngine();
		scriptScope = scriptEngine.CreateScope();
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("CONTINUE", false);
		scriptScope.SetVariable("INIT", init);
		scriptScope.SetVariable("EPISODE", episode);
		scriptScope.SetVariable("SIZE", MazeSize);
		scriptScope.SetVariable("ROW", new_row);
		scriptScope.SetVariable("COL", new_col);
		scriptScope.SetVariable("EPSILON", EPSILON);
		
		scriptSource.Execute(scriptScope);
		// 選択した行動
		action = scriptScope.GetVariable<int>("ACT");
		// 行動が決まったので移動する
		Walk(action);
	}

	int[] position2rowcol(Vector3 pos)
	{
		int row = -(((int)pos.z + 1) / 2);
		int col = ((int)pos.x - 1) / 2;
		int[] arr = new int[] { row, col };
		// UnityEngine.Debug.Log("position : (" + row + ", " + col + ")");
		return arr;
	}

	void Position(int[] arr)
	{
		new_row = arr[0];
		new_col = arr[1];
		// robot.transform.position = new Vector3((new_col * 2) + 1, 1, -((new_row * 2) + 1));
		startPosition = robot.transform.position;
	}

	/* 行動が決まったので移動する */
	void Walk(int act)
	{
		step++;
		UnityEngine.Debug.Log("step : " + step);
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
		walk = true;
	}

	void sendReward(double value)
	{
		UnityEngine.Debug.Log("移動前 position : (" + old_row + ", " + old_col + ")");
		UnityEngine.Debug.Log("移動後 position : (" + new_row + ", " + new_col + ")");

		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();

		scriptEngine = IronPython.Hosting.Python.CreateEngine();
		scriptScope = scriptEngine.CreateScope();
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("CONTINUE", true);
		scriptScope.SetVariable("SIZE", MazeSize);
		scriptScope.SetVariable("OLD_ROW", old_row);
		scriptScope.SetVariable("OLD_COL", old_col);
		scriptScope.SetVariable("NEW_ROW", new_row);
		scriptScope.SetVariable("NEW_COL", new_col);
		scriptScope.SetVariable("EPSILON", EPSILON);
		scriptScope.SetVariable("GAMMA", GAMMA);
		scriptScope.SetVariable("BETA", BETA);
		scriptScope.SetVariable("ACT", action);
		scriptScope.SetVariable("REWARD", value);
		scriptSource.Execute(scriptScope);

		if (value == GOAL)
		{
			step = 0;
			moderator.SendMessage("NextEpisode");
		} else
		{
			startPosition = endPosition;
			// 選択した行動
			action = scriptScope.GetVariable<int>("ACT");
			// 行動が決まったので移動する
			Walk(action);
		}

	}

	// Q値に従って行動する
	void Moving_start(string status)
	{
		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/RobotController.py";
		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();

		scriptEngine = IronPython.Hosting.Python.CreateEngine();
		scriptScope = scriptEngine.CreateScope();
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("SIZE", MazeSize);
		scriptScope.SetVariable("ROW", new_row);
		scriptScope.SetVariable("COL", new_col);

		scriptSource.Execute(scriptScope);

		if (status == "continue") startPosition = endPosition;
		else
		{
			step = 0;
		}// 選択した行動
		action = scriptScope.GetVariable<int>("ACT");
		// 行動が決まったので移動する
		Walk(action);
	}

    void STOP()
    {
        Debug.Log("*******************");
    }

}