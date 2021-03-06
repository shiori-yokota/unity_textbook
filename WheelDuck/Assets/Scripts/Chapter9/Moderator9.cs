﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class Moderator9 : MonoBehaviour {

	GameObject robot;

	private int MazeSize;

	// python
	ScriptEngine scriptEngine;	// スクリプト実行用のScriptEngine
	ScriptScope scriptScope;	// スクリプトに値を渡すためのScriptScope
	ScriptSource scriptSource;	// スクリプトのソースを指定するためのScriptSource



	void Start()
	{
		robot = GameObject.Find("RobotPy");

		string script;
		string filename = Application.dataPath + "/../Python/Chapter9/Moderator.py";

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
		// Moderator9.pyのソースを実行する
		scriptSource.Execute(scriptScope);

		/* Moderator9.pyを実行した結果を取得 */
		// 迷路のサイズを設定
		MazeSize = scriptScope.GetVariable<int>("SIZE");

		// カメラの設定
		SetCamera(MazeSize);
		// 光源の設定
		SetLight(MazeSize);
		// 迷路の設定
		SetMaze(MazeSize);
		// ロボットの初期位置を設定する
		InitRobotPosition(MazeSize);
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
		int wall_num = 0;
		if (MazeSize == 5) wall_num = 16;
		else if (MazeSize == 3) wall_num = 4;
		Vector3[] wall = new Vector3[wall_num];
		if (MazeSize == 5)
		{
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
		} else if (MazeSize == 3)
		{
			wall[0] = new Vector3(2, 1, -1);
			wall[1] = new Vector3(4, 1, -3);

			wall[2] = new Vector3(1, 1, -4);
			wall[3] = new Vector3(5, 1, -2);
		}

		GameObject[] InnerWallFabs = new GameObject[wall.Length];
		GameObject prefab = (GameObject)Resources.Load ("Prefabs/InnerWall");
		prefab.transform.localScale = new Vector3 (0.2f, 2, 2);
		Quaternion rot = Quaternion.identity;
		for (int i = 0; i < wall_num; i++) {
			if (i < wall_num / 2) {
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
	}

    void ViewProb(List<double> val)
    {
        var stateDef = new[]
        {
            new {
                Name = val[0].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -0.5f),
            },
            new {
                Name = val[1].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = val[2].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = val[3].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = val[4].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -0.5f),
            },
            new
            {
                Name = val[5].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = val[6].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = val[7].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = val[8].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = val[9].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -2.5f),
            },
            new
            {
                Name = val[10].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = val[11].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = val[12].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = val[13].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = val[14].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -4.5f),
            },
            new
            {
                Name = val[15].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = val[16].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = val[17].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = val[18].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = val[19].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -6.5f),
            },
            new
            {
                Name = val[20].ToString("F2"),
                Position = new Vector3(0.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = val[21].ToString("F2"),
                Position = new Vector3(2.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = val[22].ToString("F2"),
                Position = new Vector3(4.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = val[23].ToString("F2"),
                Position = new Vector3(6.5f, 1.5f, -8.5f),
            },
            new
            {
                Name = val[24].ToString("F2"),
                Position = new Vector3(8.5f, 1.5f, -8.5f),
            },

        };

        GameObject[] states = new GameObject[stateDef.Length];
        Quaternion rot = Quaternion.identity;
        rot.eulerAngles = new Vector3(90, 0, 0);
        for (int i = 0; i < stateDef.Length; i++)
        {
            states[i] = Instantiate(stateDef[i].Position, rot, stateDef[i].Name) as GameObject;
            Destroy(states[i], 4.0f);
        }
    }

    public static GameObject Instantiate(Vector3 pos, Quaternion rot, string text)
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/StateText"), pos, rot) as GameObject;
        obj.name = text;
        obj.GetComponent<TextMesh>().text = text;
        obj.GetComponent<TextMesh>().fontSize = 30;
        obj.GetComponent<TextMesh>().characterSize = 0.15f;

        return obj;
    }

}
