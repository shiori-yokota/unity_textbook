using UnityEngine;
using System.Collections;
using System.IO;

public class QLearning : MonoBehaviour
{
	private int episode;
	private int step;

	private double EPSILON;
	private double GAMMA;
	private double BETA;

	void Start()
	{

	}

	void Update()
	{

	}

    void QLearning_start(bool start)
    {
		string script;
		var filename = Application.dataPath + "/../Python/Chapter7/QLearning.py";

		using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.UTF8))
		{
			script = sr.ReadToEnd();
		}

		var scriptEngine = IronPython.Hosting.Python.CreateEngine();
		var scriptScope = scriptEngine.CreateScope();
		var scriptSource = scriptEngine.CreateScriptSourceFromString(script);

		scriptSource.Execute(scriptScope);

		/* QLearning.pyを実行した結果を取得 */

	}
}