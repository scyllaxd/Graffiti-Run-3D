using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public static class Extensions
{
    public static string Filter(this string str, List<char> charsToRemove)
    {
        foreach (char c in charsToRemove)
        {
            str = str.Replace(c.ToString(), String.Empty);
        }
        return str;
    }
}

public class idleFinishController : Singleton<idleFinishController>
{

    [BoxGroup("Idle Complete Debug Options")]
    public bool TimerComplete;
    public GameObject DebugCubeTimer;

    private DateTime currentDate;
    private DateTime oldDate;
    private List<char> charsToRemove = new List<char>() { ':', '.' };
    string str;
    // usage 41:50:35.74620
    // usage 2400000000000 / 1 day
    // usage 00:10:00.000000 // getting times format to convert for 10 minutes


    [BoxGroup("Idle Wait Hour Options")]
    [HideIf("isWaitMinutes")]
    public bool isWaitHour;
    [BoxGroup("Idle Wait Minutes Options")]
    [HideIf("isWaitHour")]
    public bool isWaitMinutes;

    [BoxGroup("Idle Wait Hour Options")]
    [Range(1,24)]
    [HideIf("isWaitMinutes")]
    [ShowIf("isWaitHour")]
    public long CompleteTimeHours;
    [BoxGroup("Idle Wait Minutes Options")]
    [Range(1,60)]
    [HideIf("isWaitHour")]
    [ShowIf("isWaitMinutes")]
    public long CompleteTimeMinutes;

    private long _completeIdleTime;
    private long _completeTimeBackup;
    private long _hourMultipler = 100000000000;
    private long _minutesMultipler = 100000000;
    private ComponentManager _componentManager;

    private void Awake()
    {
        _componentManager = ComponentManager.Instance;
    }
    
    void Start()
    {
        if (isWaitMinutes)
        {
            isWaitHour = false;
            _completeTimeBackup = CompleteTimeMinutes * _minutesMultipler;
            CompleteTimeMinutes = _completeTimeBackup;
            _completeIdleTime = CompleteTimeMinutes;
        }
        if (isWaitHour)
        {
            isWaitMinutes = false;
            _completeTimeBackup = CompleteTimeHours * _hourMultipler;
            CompleteTimeHours = _completeTimeBackup;
            _completeIdleTime = CompleteTimeHours;
        }
        //Debug.Log(_completeIdleTime);
        if (PlayerPrefs.HasKey("SystemTime"))
        {
            currentDate = System.DateTime.Now;
            long temp = Convert.ToInt64(PlayerPrefs.GetString("SystemTime"));
            DateTime oldDate = DateTime.FromBinary(temp);
            //Debug.Log("Start Date: " + oldDate);
            TimeSpan difference = currentDate.Subtract(oldDate);
            //Debug.Log("Elepsed Time : " + difference);

            // For Debug Purpose only
            _componentManager.RemaningTimeText.text = "Elapsed Time : " + difference.ToString();

            str = difference.ToString();
            str = str.Filter(charsToRemove);
            long corvertedStr = long.Parse(str);
            //Debug.Log(corvertedStr);
            if (corvertedStr >= _completeIdleTime)
            {
                
                TimerComplete = true;
            }
            else
            {
                Debug.Log("idle time not complete yet");
                TimerComplete = false;
            }
        }
        else
        {
            PlayerPrefs.SetString("SystemTime", System.DateTime.Now.ToBinary().ToString());
            Debug.Log("Saving this date to prefs: " + System.DateTime.Now);
        }

    }


    private void Update()
    {
        if (TimerComplete)
        {
            DebugCubeTimer.GetComponent<RotateObject>().enabled = true;
            PlayerPrefs.DeleteKey("SystemTime");
        }
    }

    void OnApplicationQuit()
    {

        //PlayerPrefs.SetString("SystemTime", System.DateTime.Now.ToBinary().ToString());
        //Debug.Log("Saving this date to prefs: " + System.DateTime.Now);
    }

}



