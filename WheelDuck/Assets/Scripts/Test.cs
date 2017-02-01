using UnityEngine;
using System.Collections;
using System.IO;

public class Test : MonoBehaviour {

	private int MazeSize;
	private int GOAL_COL;
	private int GOAL_ROW;

	void Start()
	{
		string script;
		var filename = Application.dataPath + "/../Python/Test.py";

		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
		{
			script = sr.ReadToEnd();
		}

		var scriptEngine = IronPython.Hosting.Python.CreateEngine();
		var scriptScope = scriptEngine.CreateScope();
		var scriptSource = scriptEngine.CreateScriptSourceFromString(script);

		scriptSource.Execute(scriptScope);

		/* Moderator.pyを実行した結果を取得 */
		// 迷路のサイズを設定
		MazeSize = scriptScope.GetVariable<int>("SIZE");
		// カメラの設定
		SetCamera(MazeSize);
		// 光源の設定
		SetLight(MazeSize);
		// 迷路の設定
		SetMaze (MazeSize);

		// ロボットの初期位置を設定
		InitRobotPosition(MazeSize);

		// ゴール位置を設定
		MazeSize = scriptScope.GetVariable<int>("SIZE");
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			// ロボットの初期位置を設定
			InitRobotPosition(MazeSize);
		}
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
		int row_num = Random.Range (0, size);
		int col_num = Random.Range (0, size);
		UnityEngine.Debug.Log (row_num);
		UnityEngine.Debug.Log (col_num);

		GameObject robot = GameObject.Find ("RobotPy");
		robot.transform.position = new Vector3 ((col_num * 2) + 1, 1, -((row_num * 2) + 1));
		UnityEngine.Debug.Log (robot.transform.position.z + ", " + robot.transform.position.x);
	}
}
