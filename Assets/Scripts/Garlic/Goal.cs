using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Just incase something is wanted from the garlic
    private GarlicManager _garlicManager;
    private TimeManager _timeManager;
    private LevelManager _levelManager;

    [SerializeField] private BoxCollider _collider;
    
    private Timer _timerScript;
    private bool _alreadyWon;
    
    private void Start()
    {
        _garlicManager = FindAnyObjectByType<GarlicManager>();

        _levelManager = FindAnyObjectByType<LevelManager>();

        _timerScript = FindAnyObjectByType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyWon)
            return;
        
        if (!other.CompareTag("Garlic")) return;
        Win();
        _alreadyWon = true;
    }

    private void Win()
    {
        Debug.Log("Won!");
        _levelManager.EndLevel();
    }

    private void OnDrawGizmos()
    {
        if (_collider == null)
            return;
        
        Gizmos.color = Color.HSVToRGB((Time.unscaledTime / 2) % 1, 1, 1);
        Gizmos.DrawWireCube(transform.position + _collider.center, _collider.size);
    }
}