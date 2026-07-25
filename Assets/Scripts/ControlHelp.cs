using UnityEngine;

public class ControlHelp : MonoBehaviour
{
    private InputSystem_Actions _input;

    [SerializeField] private UIButtonState _menuButton;
    [SerializeField] private UIButtonState _pauseButton;
    [SerializeField] private UIButtonState _swapButton;
    [SerializeField] private UIButtonState _retryButton;
    
    private Vector3 SMALL = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 NORMAL = Vector3.one;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }
    
    private void OnDestroy()
    {
        _input.Disable();
    }

    public void ButtonPressMenu()
    {
        
    }

    public void ButtonPressPause()
    {
        FindAnyObjectByType<PauseManager>().TogglePause();
    }

    public void ButtonPressSwap()
    {
        FindAnyObjectByType<BombManager>().HandleSwap();
    }

    public void ButtonPressRetry()
    {
        FindAnyObjectByType<LevelManager>().RetryCurrentLevel();
    }

    // Update is called once per frame
    void Update()
    {
        var control = _input.GameControl;
        _menuButton.transform.localScale = control.Menu.IsPressed() || _menuButton.IsPressed ? SMALL : NORMAL;
        _pauseButton.transform.localScale = control.Pause.IsPressed() || _pauseButton.IsPressed ? SMALL : NORMAL;
        _swapButton.transform.localScale = control.Swap.IsPressed() || _swapButton.IsPressed ? SMALL : NORMAL;
        _retryButton.transform.localScale = control.Retry.IsPressed() || _retryButton.IsPressed ? SMALL : NORMAL;
    }
}
