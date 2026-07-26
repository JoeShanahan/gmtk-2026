using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Just incase something is wanted from the garlic
    private GarlicManager _garlicManager;
    private TimeManager _timeManager;
    private LevelManager _levelManager;
    
    private Timer _timerScript;
    
    private void Start()
    {
        _garlicManager = FindAnyObjectByType<GarlicManager>();

        _levelManager = FindAnyObjectByType<LevelManager>();

        _timerScript = FindAnyObjectByType<Timer>();
        
        gameObject.GetComponent<Collider>().isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Garlic")) return;
        Win();
    }

    private void Win()
    {
        Debug.Log("Won!");
        _levelManager.EndLevel();
    }
}