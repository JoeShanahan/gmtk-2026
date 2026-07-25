using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TimeManager _timeManager;
    
    [SerializeField] float timer;
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
            timer += Time.deltaTime;
            _timeManager.formattedTimer = timer.ToString("F2");
            timerText.text = _timeManager.formattedTimer;
        }
    }
    
    public void ResetTimer()
    {
        _timeManager.timerRunning = false;
        timer = 0;
        _timeManager.StartTime();
    }
}
