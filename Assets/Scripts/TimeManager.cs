using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool timerRunning = false;
    public string formattedTimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void StartTime()
    {
        timerRunning = true;                    
        Time.timeScale = 1;
    }

    public void StopTime()
    {
        timerRunning = false;
        Time.timeScale = 0;
    }
}
