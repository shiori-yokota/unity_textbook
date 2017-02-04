using UnityEngine;
using System.Collections;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class QLearning : MonoBehaviour
{
	int episode = 0;
	double EPSILON;
	double GAMMA;
	double BETA;
	int row;
	int col;

	void Start()
	{
		string script;
		var filename = Application.dataPath + "/../Python/Chapter7/Moderator.py";

		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();
		
		// Pythonスクリプト実行エンジン
		var scriptEngine = Python.CreateEngine();
		// 実行エンジンに渡す値を設定する
		var scriptScope = scriptEngine.CreateScope();
		// pythonのソースを指定
		var scriptSource = scriptEngine.CreateScriptSourceFromString(script);
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

    void QLearning_start()
    {
		episode += 1;
		UnityEngine.Debug.Log("Start QLearning");
		string script;
		var filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";

		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
			script = sr.ReadToEnd();
		

		var scriptEngine = IronPython.Hosting.Python.CreateEngine();
		var scriptScope = scriptEngine.CreateScope();
		var scriptSource = scriptEngine.CreateScriptSourceFromString(script);
		/* QLearning.pyを実行 */
		scriptScope.SetVariable("SIZE", 5);
		scriptScope.SetVariable("EPISODE", episode);
		scriptScope.SetVariable("EPSILON", EPSILON);
		scriptScope.SetVariable("GAMMA", GAMMA);
		scriptScope.SetVariable("BETA", BETA);
		scriptScope.SetVariable("ROW", row);
		scriptScope.SetVariable("COL", col);
		scriptSource.Execute(scriptScope);
	}

	void Position(int[] arr)
	{
		row = arr[0];
		col = arr[1];
	}
}