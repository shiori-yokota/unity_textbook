using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class ParticleFilter : MonoBehaviour
{
    GameObject robot;

    // python
    ScriptEngine scriptEngine;  // スクリプト実行用のScriptEngine
    ScriptScope scriptScope;    // スクリプトに値を渡すためのScriptScope
    ScriptSource scriptSource;  // スクリプトのソースを指定するためのScriptSource

    //string robotfile = Application.dataPath + "/../Python/Chapter9/ParticleFilter.py";
    bool walk;
    bool input;
    bool execute = true;
    float distance;

    int SIZE = 0;
    double TRANS;
    double KANSOKU;    

    IronPython.Runtime.List PRTCL = new IronPython.Runtime.List { };
    IronPython.Runtime.List WALLS = new IronPython.Runtime.List { };
    int PA;
    int action = -1;
    int TrialCount = 0;

    private Vector3 startPosition;
    private Vector3 endPosition;

    void Start()
    {
        robot = GameObject.Find("RobotPy");

        walk = false;
        input = false;

        string script;
        string moderaterfile = Application.dataPath + "/../Python/Chapter9/Moderator.py";

        using (StreamReader sr = new StreamReader(moderaterfile, System.Text.Encoding.UTF8))
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

        SIZE = scriptScope.GetVariable<int>("SIZE");
        TRANS = scriptScope.GetVariable<double>("TRANS");
        KANSOKU = scriptScope.GetVariable<double>("KANSOKU");
        PA = scriptScope.GetVariable<int>("PA");
        WALLS = scriptScope.GetVariable<IronPython.Runtime.List>("WALLS");
    }

    private void Update()
    {
        if (execute)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                UnityEngine.Debug.Log("Particle Filter Start");
                FirstParticle();
                execute = false;
            }
        }
        if (input)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                action = 0;
                input = false;
                Moving();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                action = 1;
                input = false;
                Moving();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                action = 2;
                input = false;
                Moving();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                action = 3;
                input = false;
                Moving();
            }
        }
        if (walk)
        {
            distance += Time.deltaTime * 2;
            robot.transform.position = Vector3.MoveTowards(startPosition, endPosition, distance);
            if (Vector3.Distance(robot.transform.position, endPosition) < 0.1)
            { // 移動完了
                walk = false;
                distance = 0.0f;
                FromTheSecondTime();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        UnityEngine.Debug.Log("*** 壁にぶつかった ***");
        walk = false;
        distance = 0.0f;
        // 壁にぶつかったらもといた場所に戻る
        endPosition = startPosition;
        robot.transform.position = endPosition;
        
        FromTheSecondTime();
    }

    void FirstParticle()
    {
        startPosition = robot.transform.position;

        string script;
        string filename = Application.dataPath + "/../Python/Chapter9/ParticleFilter.py";

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
        scriptScope.SetVariable("TimeCount", TrialCount);
        scriptScope.SetVariable("PA", PA);
        scriptScope.SetVariable("SIZE", SIZE);
        // Moderator.pyのソースを実行する
        scriptSource.Execute(scriptScope);

        PRTCL = scriptScope.GetVariable<IronPython.Runtime.List>("PRTCL");

        viewProb(PRTCL);
        input = true;
    }

    void FromTheSecondTime()
    {
        TrialCount++;
        startPosition = robot.transform.position;

        string WallList = getWallStatus(startPosition);

        string script;
        string filename = Application.dataPath + "/../Python/Chapter9/ParticleFilter.py";

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
        scriptScope.SetVariable("TimeCount", TrialCount);
        scriptScope.SetVariable("PA", PA);
        scriptScope.SetVariable("SIZE", SIZE);
        scriptScope.SetVariable("TRANS", TRANS);
        scriptScope.SetVariable("KANSOKU", KANSOKU);
        scriptScope.SetVariable("PRTCL", PRTCL);
        scriptScope.SetVariable("ACTION", action);
        scriptScope.SetVariable("WALL", WallList);
        scriptScope.SetVariable("tmpWALLS", WALLS);
        // Moderator.pyのソースを実行する
        scriptSource.Execute(scriptScope);

        PRTCL = scriptScope.GetVariable<IronPython.Runtime.List>("PRTCL");

        viewProb(PRTCL);
        input = true;
    }

    void Moving()
    {
        if (action == 0)
            endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + 2.0f);
        else if (action == 1)
            endPosition = new Vector3(startPosition.x + 2.0f, startPosition.y, startPosition.z);
        else if (action == 2)
            endPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z - 2.0f);
        else if (action == 3)
            endPosition = new Vector3(startPosition.x - 2.0f, startPosition.y, startPosition.z);
        else
            UnityEngine.Debug.Log("error : not difine endPosition");
        walk = true;
    }

    void viewProb(IronPython.Runtime.List prob)
    {
        GameObject moderator = GameObject.Find("GameObject");
        List<double> stateVal = prob.Cast<double>().ToList();
        moderator.SendMessage("ViewProb", stateVal);
    }

    string getWallStatus(Vector3 pos)
    {
        int state = position2state(pos);
        List<string> stateVal = WALLS.Cast<string>().ToList();
        // UnityEngine.Debug.Log("walls : " + stateVal[state]);
        string walls = stateVal[state];

        return walls;
    }

    int position2state(Vector3 pos)
    {
        int z = (Mathf.RoundToInt(pos.x) - 1) / 2;
        int x = ((-(Mathf.RoundToInt(pos.z)) - 1) / 2);
        
        int statenum = 5 * x + z;
        // UnityEngine.Debug.Log("state : " + statenum);

        return statenum;
    }
}