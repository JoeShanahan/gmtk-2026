using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] 
    private Transform _pauseBorder;

    private InputSystem_Actions _input;
    private bool _isPaused;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new();
        _input.Enable();
        _pauseBorder.gameObject.SetActive(_isPaused);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.GameControl.Pause.WasPressedThisFrame())
        {
            TogglePause();
        }

        if (_isPaused)
            return;

        Time.timeScale = _input.GameControl.FFWD.IsPressed() ? 4 : 1;
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0.05f : 1;
        _pauseBorder.gameObject.SetActive(_isPaused);
    }
}
