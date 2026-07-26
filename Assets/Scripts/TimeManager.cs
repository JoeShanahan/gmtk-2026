using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool timerRunning = false;
    public string formattedTimer;
    public float timer;

    public void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            formattedTimer = timer.ToString("F2");
        }
    }

    public void StartTime()
    {
        timer = 0;
        timerRunning = true;                    
        Time.timeScale = 1;
    }

    public void PauseTime()
    {
        timerRunning = false;
        Time.timeScale = 0;
    }

    public void ResumeTime()
    {
        timerRunning = true;
        Time.timeScale = 1;
    }
}
