using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] 
    private Transform _pauseBorder;

    [SerializeField] 
    private GameSettings _settings;
    
    private InputSystem_Actions _input;
    private bool _isPaused;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new();
        _input.Enable();
        _pauseBorder.gameObject.SetActive(_isPaused);
    }
    
    private void OnDestroy()
    {
        _input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.GameControl.Pause.WasPressedThisFrame())
        {
            TogglePause();
        }

        // if (_isPaused)
        //     return;
        // Time.timeScale = _input.GameControl.FFWD.IsPressed() ? 4 : 1;
    }

    private float GetSlowSpeed()
    {
        return _settings == null ? SpeedSettings.DEFAULT : _settings.PauseSpeed;
    }

    public void OnPauseMenuClosed()
    {
        Time.timeScale = _isPaused ? GetSlowSpeed() : 1;
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? GetSlowSpeed() : 1;
        _pauseBorder.gameObject.SetActive(_isPaused);
    }
}
