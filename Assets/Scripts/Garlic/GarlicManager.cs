using UnityEngine;

public class GarlicManager : MonoBehaviour
{
    // Initial spawner object
    [SerializeField] GameObject initialSpawner;
    
    // Current active spawner
    GameObject currentSpawner;
    
    // Garlic Prefab
    [SerializeField] GameObject garlicPrefab;
    
    void Awake()
    {
        InitSpawner();
    }

    /// <summary>
    /// Used on initilisation and to reset spawner back to default.
    /// </summary>
    void InitSpawner()
    {
        currentSpawner = initialSpawner.gameObject;
        Instantiate(garlicPrefab, currentSpawner.transform);
    }

    /// <summary>
    /// Updates the currently used spawner to a new one - used for checkpoints.
    /// </summary>
    /// <param name="newSpawner">Gameobject of the new spawner object</param>
    public void UpdateCurrentSpawner(GameObject newSpawner)
    {
        newSpawner = currentSpawner.gameObject;
    }

    /// <summary>
    /// Respawn the garlic prefab to the currently set spawner.
    /// </summary>
    /// <param name="existingGarlic"></param>
    public void RespawnGarlic(GameObject existingGarlic)
    {
        Destroy(existingGarlic);
        Instantiate(garlicPrefab, currentSpawner.transform);
    }
}
