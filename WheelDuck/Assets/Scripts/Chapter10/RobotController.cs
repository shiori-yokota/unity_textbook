using UnityEngine;
using System;

public class RobotController : MonoBehaviour
{
    GameObject robot;
    
    bool walk;
    bool spine;
    bool input;
    bool execute = true;
    float distance;

    int action = -1;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private string datetimeStr;

    void Start()
    {
        robot = GameObject.Find("RobotPy");
        startPosition = robot.transform.position;

        walk = false;
        spine = false;
        input = false;
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
                startPosition = robot.transform.position;

                execute = false;
                input = true;
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
                // Moving();
                Debug.Log("回転する");
                Rotate();
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
                //Moving();
                Debug.Log("回転する");
                Rotate();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                Application.CaptureScreenshot(Application.dataPath + "/ScreenShot/" + datetimeStr + ".jpg");
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

                startPosition = endPosition;
                input = true;
            }
        }
        if (spine)
        {
            if (action == 1)
            {
                robot.transform.Rotate(transform.right, 1);
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

    void Rotate()
    {
        if (action == 1)
        {
            Debug.Log("回転させる");
            robot.transform.Rotate(transform.right);
        }
    }

}