using System;
using System.Collections.Generic;
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
    [SerializeField] GameObject garlicClovePrefab;
    
    [SerializeField] List<GameObject> cloves = new();
    
    private InputSystem_Actions _input;

    [SerializeField] private int miny;
    [SerializeField] private int maxy;
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
        
        // Respawns garlic if it falls off the map, or goes so high it'll take too long to land
        if (currentGarlic.transform.position.y < miny || currentGarlic.transform.position.y > maxy)
            RespawnGarlic(currentGarlic);
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
        SpawnCloves();
        currentGarlic = Instantiate(garlicPrefab, currentSpawner.transform);
    }
    
    public void SpawnCloves()
    {
        Transform pos = currentGarlic.transform;
        // Spawn 8 cloves. Spawning ontop of each other should cause a little explosion of sorts?   
        for (int i = 0; cloves.Count < 8; i++)
        {
            GameObject clove = Instantiate(garlicClovePrefab, pos.position, pos.rotation);
            cloves.Add(clove);
        }
    }
}
