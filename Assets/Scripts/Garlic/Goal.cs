using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Just incase something is wanted from the garlic
    private GarlicManager _garlicManager;
    private Timer _timerScript;
    
    private void Start()
    {
        _garlicManager = FindAnyObjectByType<GarlicManager>();
        
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
        Time.timeScale = 0;
        Debug.Log("Won!");
        // Do other win things here
        
        
    }
}