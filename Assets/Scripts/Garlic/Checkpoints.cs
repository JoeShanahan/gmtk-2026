using System;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private GarlicManager _garlicManager;
    
    private void Start()
    {
        _garlicManager = FindAnyObjectByType<GarlicManager>();
        gameObject.GetComponent<Collider>().isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Garlic")) return;
        Debug.Log("Updating checkpoint");
        _garlicManager.UpdateCurrentSpawner(this.gameObject);
        gameObject.GetComponent<Collider>().isTrigger = false;
    }
}
