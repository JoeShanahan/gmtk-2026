using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarlicManager : MonoBehaviour
{
    // Initial spawner object
    [SerializeField] GameObject initialSpawner;
    
    // Current active spawner
    [SerializeField] GameObject currentSpawner;
    
    // Current Garlic Object
    GameObject currentGarlic;
    
    // Garlic Prefab
    [SerializeField] GameObject garlicPrefab;
    
    private InputSystem_Actions _input;
    
    void Awake()
    {
        InitSpawner();
        
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    private void Update()
    {
        // Check if the retry key is pressed and then respawn the garlic
        var control = _input.GameControl;
        if (control.Retry.IsPressed())
        {
            RespawnGarlic(currentGarlic);
        }
    }

    /// <summary>
    /// Used on initilisation and to reset spawner back to default.
    /// </summary>
    void InitSpawner()
    {
        currentSpawner = initialSpawner.gameObject;
        currentGarlic = Instantiate(garlicPrefab, currentSpawner.transform);
    }

    /// <summary>
    /// Updates the currently used spawner to a new one - used for checkpoints.
    /// </summary>
    /// <param name="newSpawner">Gameobject of the new spawner object</param>
    public void UpdateCurrentSpawner(GameObject newSpawner)
    {
        Debug.Log("Updating spawner to: " + newSpawner.name);
        currentSpawner = newSpawner;
    }

    /// <summary>
    /// Respawn the garlic prefab to the currently set spawner. 
    /// </summary>
    /// <param name="existingGarlic"></param>
    public void RespawnGarlic(GameObject existingGarlic)
    {
        Destroy(existingGarlic);
        currentGarlic = Instantiate(garlicPrefab, currentSpawner.transform);
    }
}
