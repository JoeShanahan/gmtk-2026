using UnityEngine;

public class ControlHelp : MonoBehaviour
{
    private InputSystem_Actions _input;

    [SerializeField] private Transform _menuButton;
    [SerializeField] private Transform _pauseButton;
    [SerializeField] private Transform _swapButton;
    [SerializeField] private Transform _retryButton;

    private Vector3 SMALL = new Vector3(0.85f, 0.85f, 0.85f);
    private Vector3 NORMAL = Vector3.one;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        _menuButton.localScale = _input.GameControl.Menu.IsPressed() ? SMALL : NORMAL;
        _pauseButton.localScale = _input.GameControl.Pause.IsPressed() ? SMALL : NORMAL;
        _swapButton.localScale = _input.GameControl.Swap.IsPressed() ? SMALL : NORMAL;
        _retryButton.localScale = _input.GameControl.Retry.IsPressed() ? SMALL : NORMAL;
    }
}
