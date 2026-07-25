using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelData _selectedLevel;
    private SpawnedLevel _levelInstance;
    private float _currentSeconds;

    [SerializeField] 
    private DebugLevelSelectList _debugUi;

    [SerializeField] 
    private MainGameLevelSet _mainGameLevels;
    
    private InputSystem_Actions _input;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new();
        _input.Enable();
        if (_mainGameLevels != null && _mainGameLevels.SelectedLevel != null)
        {
            InstantiateLevel(_mainGameLevels.SelectedLevel);
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
        _debugUi?.SetActive(false);
        
        FindAnyObjectByType<StartLevelScreen>()?.SetLevel(level);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _currentSeconds += Time.deltaTime;

        if (_input.GameControl.DebugLevelSelect.WasPressedThisFrame())
        {
            _debugUi?.SetActive(!_debugUi.gameObject.activeInHierarchy);
        }

        if (_input.GameControl.Retry.WasPressedThisFrame())
        {
            RetryCurrentLevel();    
        }
    }

    public void RetryCurrentLevel()
    {
        if (_selectedLevel == null)
            return;
        
        PersistentUI.DoTransition(() => InstantiateLevel(_selectedLevel));
    }
}
