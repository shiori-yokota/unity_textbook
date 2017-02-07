using UnityEngine;
using System.Collections;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class Moderator : MonoBehaviour {

	GameObject robot;

	private int MazeSize;
	private int GOAL_COL;
	private int GOAL_ROW;

	private double GOAL_REWARD;
	private double HIT_WALL_PENALTY;
	private double ONE_STEP_PENALTY;

	// python
	ScriptEngine scriptEngine;	// スクリプト実行用のScriptEngine
	ScriptScope scriptScope;	// スクリプトに値を渡すためのScriptScope
	ScriptSource scriptSource;	// スクリプトのソースを指定するためのScriptSource

	void Start()
	{
		robot = GameObject.Find("RobotPy");

		string script;
		string filename = Application.dataPath + "/../Python/Chapter7/Moderator.py";

		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
		{
			script = sr.ReadToEnd();
		}

		// Pythonスクリプト実行エンジン
		scriptEngine = Python.CreateEngine();
		// 実行エンジンに渡す値を設定する
		scriptScope = scriptEngine.CreateScope();
		// pythonのソースを指定
		scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		// Moderator.pyのソースを実行する
		scriptSource.Execute(scriptScope);

		/* Moderator.pyを実行した結果を取得 */
		// 迷路のサイズを設定
		MazeSize = scriptScope.GetVariable<int>("SIZE");
		// ゴール位置を設定
		GOAL_COL = (scriptScope.GetVariable<int>("GOAL_COL") * 2) + 1;
		GOAL_ROW = (scriptScope.GetVariable<int>("GOAL_ROW") * 2) + 1;
		// 報酬を設定
		GOAL_REWARD = scriptScope.GetVariable<double>("GOAL_REWARD");
		HIT_WALL_PENALTY = scriptScope.GetVariable<double>("HIT_WALL_PENALTY");
		ONE_STEP_PENALTY = scriptScope.GetVariable<double>("ONE_STEP_PENALTY");

		// カメラの設定
		SetCamera(MazeSize);
		// 光源の設定
		SetLight(MazeSize);
		// 迷路の設定
		SetMaze(MazeSize);
		// ロボットの初期位置を設定する
		InitRobotPosition(MazeSize);

		/* 環境設定が終わったので，Q-Learningを開始する */
        robot.SendMessage("QLearning_start");
    }

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			InitRobotPosition(MazeSize);
			robot.SendMessage("QLearning_start");
		}
		if (Input.GetKeyDown (KeyCode.C)) { // ゴールにいるかどうかを判定
			bool GoalPos = CheckGoalPosition ();
			if (GoalPos) {
				UnityEngine.Debug.Log ("----- GOAL -----");
				NextScene ();
			}
		}
	}

	void NextScene()
	{
		//
		//
	}

	void SetCamera(int size)
	{
		GameObject cam = GameObject.Find ("Camera");
		cam.transform.position = new Vector3 (size, size * 2.5f, -size);
	}

	void SetLight(int size)
	{
		GameObject light = GameObject.Find ("Directional light");
		light.transform.position = new Vector3 (size, size * 2, -size);
	}

	void SetMaze(int size)
	{
		// 床の設定
		GameObject floor = GameObject.Find ("Floor");
		floor.transform.localScale = new Vector3 (size * 2, 0.5f, size * 2);
		floor.transform.position = new Vector3 (size, 0, -size);
		//外壁の設定
		SetOuterWall(size);
		// 内壁の設定
		SetInnerWall();
	}

	void SetOuterWall(int size)
	{
		Vector3[] wall = new Vector3[4];
		wall [0] = new Vector3 (size, 1, 0);
		wall [1] = new Vector3 (size * 2 , 1, -size);
		wall [2] = new Vector3 (size, 1, -(size * 2));
		wall [3] = new Vector3 (0, 1, -size);

		GameObject[] OuterWallFabs = new GameObject[4];
		GameObject prefab = (GameObject)Resources.Load ("Prefabs/OuterWall");
		prefab.transform.localScale = new Vector3 (0.2f, 2, size * 2);
		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (0, 90, 0);
		for (int i = 0; i < 4; i++) {
			if (i % 2 == 0)
				OuterWallFabs[i] = Instantiate (prefab, wall [i], rot) as GameObject;
			else
				OuterWallFabs[i] = Instantiate (prefab, wall [i], Quaternion.identity) as GameObject;
		}
	}

	void SetInnerWall()
	{
		Vector3[] wall = new Vector3[16];
		wall [0] = new Vector3 (2, 1, -1);
		wall [1] = new Vector3 (8, 1, -1);
		wall [2] = new Vector3 (8, 1, -3);
		wall [3] = new Vector3 (2, 1, -5);
		wall [4] = new Vector3 (6, 1, -5);
		wall [5] = new Vector3 (6, 1, -7);
		wall [6] = new Vector3 (4, 1, -9);
		wall [7] = new Vector3 (6, 1, -9);

		wall [8] = new Vector3 (3, 1, -2);
		wall [9] = new Vector3 (5, 1, -2);
		wall [10] = new Vector3 (3, 1, -4);
		wall [11] = new Vector3 (5, 1, -4);
		wall [12] = new Vector3 (3, 1, -6);
		wall [13] = new Vector3 (7, 1, -6);
		wall [14] = new Vector3 (1, 1, -8);
		wall [15] = new Vector3 (9, 1, -8);

		GameObject[] InnerWallFabs = new GameObject[wall.Length];
		GameObject prefab = (GameObject)Resources.Load ("Prefabs/InnerWall");
		prefab.transform.localScale = new Vector3 (0.2f, 2, 2);
		Quaternion rot = Quaternion.identity;
		for (int i = 0; i < 16; i++) {
			if (i < 8) {
				InnerWallFabs[i] = Instantiate (prefab, wall [i], rot) as GameObject;
			} else {
				rot.eulerAngles = new Vector3 (0, 90, 0);
				InnerWallFabs[i] = Instantiate (prefab, wall [i], rot) as GameObject;
			}
		}
	}

	void InitRobotPosition(int size)
	{
		int row = Random.Range (0, size);
		int col = Random.Range (0, size);
		UnityEngine.Debug.Log ("Init robot Pos : (" + row + ", " + col + ")");
		robot.transform.position = new Vector3 ((col * 2) + 1, 1, -((row * 2) + 1));
		if (row == GOAL_ROW && col == GOAL_COL) InitRobotPosition(size);
		else setPosition(row, col);
	}

	void setPosition(int row, int col)
	{
		// 初期位置をロボットに伝える
		int[] state = new int[] { row, col };
		robot.SendMessage("Position", state);
	}

	int[] transPos2rowcol(Vector3 pos)
	{
		int row = -(((int)pos.z + 1) / 2);
		int col = ((int)pos.x - 1) / 2;
		int[] arr = new int[] { row, col };
		return arr;
	}

	/* Robotがゴール位置にいるかどうかを判定 */
	bool CheckGoalPosition()
	{
		GameObject robot = GameObject.Find ("RobotPy");
		if (robot.transform.position.x == GOAL_COL && -(robot.transform.position.z) == GOAL_ROW)
			return true;
		else
			return false;
	}

	/* Robotの行動に対して報酬を答える */
	void GetReward(bool colli)
	{
		double reward_value;
		if (CheckGoalPosition())
			reward_value = GOAL_REWARD;
		else
			if (colli) reward_value = HIT_WALL_PENALTY;
			else reward_value = ONE_STEP_PENALTY;
		robot.SendMessage("sendReward", reward_value);
	}
}
