using UnityEngine;
using System;

public class RobotController11 : MonoBehaviour
{
    GameObject robot;
    
    bool execute = true;

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

            }
        }
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
            Debug.Log("*** Capture ***");
            Application.CaptureScreenshot(Application.dataPath + "/ScreenShot/" + datetimeStr + ".jpg");
        }
            
    }
    
}