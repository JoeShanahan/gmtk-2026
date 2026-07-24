using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] 
    private LevelData _levelToTest;

    private LevelData _selectedLevel;
    private SpawnedLevel _levelInstance;
    private float _currentSeconds;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_levelToTest != null)
        {
            InstantiateLevel(_levelToTest);
        }
    }

    public void InstantiateLevel(LevelData level)
    {
        if (_levelInstance != null)
        {
            Destroy(_levelInstance.gameObject);
        }
        
        _selectedLevel = level;
        _currentSeconds = 0;
        _levelInstance = Instantiate(level.Prefab).GetComponent<SpawnedLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        _currentSeconds += Time.deltaTime;
    }
}
