using System;
using Unity.VisualScripting;
using UnityEngine;

public enum LevelRank
{
    None = 0,
    Bogey = 1,
    Par = 2,
    Birdie = 3
}

public class LevelManager : MonoBehaviour
{
    public event Action OnLevelComplete;
    
    private LevelData _selectedLevel;
    private SpawnedLevel _levelInstance;
    
    private TimeManager _timeManager;
    private Timer _timer;

    public LevelRank currentMedal;

    [SerializeField] 
    private DebugLevelSelectList _debugUi;

    [SerializeField] 
    private MainGameLevelSet _mainGameLevels;

    [SerializeField] private LevelCompleteScreen _levelCompleteScreen;
    
    [SerializeField] private Transform _mainGameUi;
    [SerializeField] private Transform _startGameUi;
    
    private InputSystem_Actions _input;

    [SerializeField] 
    private SaveData _secondarySaveData;
    
    
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
        
        PersistentUI.DoTransition(() =>
        {
            InstantiateLevel(_selectedLevel, false);
            FindAnyObjectByType<LevelCompleteScreen>()?.gameObject.SetActive(false);
            FindAnyObjectByType<PauseManager>()?.OnRestartLevel();
        });
        
        _timeManager.StartTime();
    }

    public void EndLevel()
    {
        float levelTime = _timeManager.timer;
        
        // _timeManager.PauseTime();
        currentMedal = CalculateMedal(Mathf.FloorToInt(levelTime * 10));
        // SaveManager.Instance.SetLevelInfo(_selectedLevel.name, _timeManager.formattedTimer);
        
        float previousBest = _secondarySaveData.GetBestTime(_selectedLevel.MainGameIndex);
        _secondarySaveData.SetBestTime(_selectedLevel.MainGameIndex, levelTime);

        _levelCompleteScreen.CompleteLevel(levelTime, previousBest, _selectedLevel, currentMedal);
        //Show UI elements
        Debug.Log("Level Complete");
        
        OnLevelComplete?.Invoke();
    }
    
    LevelRank CalculateMedal(int timer)
    {
        if (timer > _selectedLevel.SecondsPar)
            return LevelRank.Bogey;

        if (timer > _selectedLevel.SecondsBirdie)
            return LevelRank.Par;

        return LevelRank.Birdie;
    }
}
