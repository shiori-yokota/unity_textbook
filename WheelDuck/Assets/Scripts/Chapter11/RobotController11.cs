using UnityEngine;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

public class RobotController11 : MonoBehaviour
{
    GameObject robot;

    // python
    ScriptEngine scriptEngine;  // スクリプト実行用のScriptEngine
    ScriptScope scriptScope;    // スクリプトに値を渡すためのScriptScope
    ScriptSource scriptSource;  // スクリプトのソースを指定するためのScriptSource

    bool execute = true;
    bool input = false;

    float speed = 0.03f;
    private string datetimeStr;

    void Start()
    {
        robot = GameObject.Find("RobotPy");
    }

    private void Update()
    {
        datetimeStr = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Day.ToString().PadLeft(2, '0') + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                        + System.DateTime.Now.Minute.ToString().PadLeft(2, '0') + System.DateTime.Now.Second.ToString().PadLeft(2, '0');
        
        if (execute)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                UnityEngine.Debug.Log(" Start ");
                execute = false;
                input = true;

            }
        }
        if (input)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                robot.transform.position += robot.transform.forward * speed;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                robot.transform.position += -robot.transform.forward * speed;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                robot.transform.Rotate(new Vector3(0, 1f, 0));
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                robot.transform.Rotate(new Vector3(0, -1f, 0));
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                input = false;
                Debug.Log("*** Capture ***");
                Application.CaptureScreenshot(Application.dataPath + "/ScreenShot/" + datetimeStr + ".jpg");
                Recognition();
            }

        }

    }

    void Recognition()
    {
        string script;
        string filename = Application.dataPath + "/../Python/Chapter11/PatternRecognition.py";

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
        // Moderator11.pyのソースを実行する
        scriptSource.Execute(scriptScope);


        // ロボットが移動できるようにする
        Debug.Log(" can move ");
        input = true;
    }

}