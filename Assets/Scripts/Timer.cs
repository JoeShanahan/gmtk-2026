using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TimeManager _timeManager;
    
    private TMP_Text timerText; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timeManager = FindAnyObjectByType<TimeManager>();
        timerText = gameObject.GetComponentInChildren<TMP_Text>();
        _timeManager.StartTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeManager.timerRunning)
        {
            timerText.text = _timeManager.formattedTimer;
        }
    }
}
