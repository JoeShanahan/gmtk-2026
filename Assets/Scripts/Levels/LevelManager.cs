using System;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private LevelData _selectedLevel;
    private SpawnedLevel _levelInstance;
    
    private TimeManager _timeManager;
    private Timer _timer;

    public string currentMedal;

    [SerializeField] 
    private DebugLevelSelectList _debugUi;

    [SerializeField] 
    private MainGameLevelSet _mainGameLevels;

    [SerializeField] private Transform _mainGameUi;
    [SerializeField] private Transform _startGameUi;
    
    private InputSystem_Actions _input;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timeManager = FindAnyObjectByType<TimeManager>();
        _timer = FindAnyObjectByType<Timer>();
        
        _input = new();
        _input.Enable();
        if (_mainGameLevels != null && _mainGameLevels.SelectedLevel != null)
        {
            InstantiateLevel(_mainGameLevels.SelectedLevel, true);
        }
    }

    private void OnDestroy()
    {
        _input.Disable();
    }

    public void InstantiateLevel(LevelData level, bool showStartScreen)
    {
        if (_levelInstance != null)
        {
            Destroy(_levelInstance.gameObject);
        }
        
        _selectedLevel = level;
        _levelInstance = Instantiate(level.Prefab).GetComponent<SpawnedLevel>();
        _debugUi?.SetActive(false);

        var startScreen = FindAnyObjectByType<StartLevelScreen>();

        if (showStartScreen && startScreen!= null)
        {
            startScreen.SetLevel(level);
            Time.timeScale = 0;
        }
        else
        {
            _levelInstance.SwapToMainCam();
        }
    }

    public void BeginLevel()
    {
        if (_startGameUi != null)
            _startGameUi.gameObject.SetActive(false);
        
        if (_mainGameUi != null)
            _mainGameUi.gameObject.SetActive(true);
        
        _timeManager.StartTime();
        
        FindAnyObjectByType<BombManager>()?.SetPause(false);
        _levelInstance.SwapToMainCam();

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
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
        
        _timeManager.PauseTime();
        
        PersistentUI.DoTransition(() => InstantiateLevel(_selectedLevel, false));
        
        _timeManager.StartTime();
    }

    public void EndLevel()
    {
        _timeManager.PauseTime();
        CalculateMedal(Mathf.FloorToInt(_timeManager.timer));
        SaveManager.Instance.SetLevelInfo(_selectedLevel.name, _timeManager.formattedTimer);
        //Show UI elements
    }
    
    void CalculateMedal(int timer)
    {
        if (_selectedLevel.SecondsPar > timer && _selectedLevel.SecondsBirdie < timer)
            currentMedal = "Par";
        else if (_selectedLevel.SecondsBirdie > timer)
            currentMedal = "Birdie";
        else if (_selectedLevel.SecondsPar < timer)
            currentMedal = "Par";
    }
}
