using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timer;
    private TMP_Text timerText;

    public string formattedTimer;
    
    [SerializeField] bool timerRunning;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timerText = gameObject.GetComponentInChildren<TMP_Text>();
        StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            formattedTimer = timer.ToString("F2");
            timerText.text = formattedTimer;
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
    
    public void ResetTimer()
    {
        timerRunning = false;
        timer = 0;
        StartTimer();
    }
}
