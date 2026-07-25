using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum CurrentControllerType { Unknown, PlayStation, Xbox, Nintendo, KeyboardMouse };

public class ControlsManager : MonoBehaviour
{
    private static CurrentControllerType _currentController = CurrentControllerType.KeyboardMouse;
    private InputSystem_Actions _input;

    public CurrentControllerType CurrentControllerType => _currentController;

    private void OnDestroy()
    {
        _input.Disable();
    }

    private void Start()
    {
        _input = new();

        _input.Enable();
        _input.GameControl.Pause.performed += InputSystemEvent;
        _input.GameControl.Menu.performed += InputSystemEvent;
        _input.GameControl.Retry.performed += InputSystemEvent;
        _input.GameControl.Swap.performed += InputSystemEvent;
        _input.GameControl.FFWD.performed += InputSystemEvent;
        _input.Player.Move.performed += JoystickEvent;
        _input.Player.Look.performed += JoystickEvent;

        OnControlTypeChanged(_currentController);
    }

    private void OnControlTypeChanged(CurrentControllerType newType)
    {
        _currentController = newType;
        Debug.Log($"our control type is now: {newType}");

        foreach (var visuals in FindObjectsByType<DynamicControllerVisuals>(FindObjectsInactive.Include))
        {
            visuals.ControllerChanged(newType);
        }
    }

    private void JoystickEvent(InputAction.CallbackContext context)
    {
        if (context.ReadValue<Vector2>().magnitude > 0.1f)
            InputSystemEvent(context);
    }

    private void InputSystemEvent(InputAction.CallbackContext context)
    {
        CurrentControllerType conType = GetInputType(context.control.device);

        if (conType !=  _currentController)
            OnControlTypeChanged(conType);
    }

    private CurrentControllerType GetInputType(InputDevice dev)
    {
        if (dev.description.deviceClass == "Keyboard" || dev.description.deviceClass == "Mouse")
            return CurrentControllerType.KeyboardMouse;

        if (dev.description.manufacturer == "Sony Interactive Entertainment")
            return CurrentControllerType.PlayStation;

        if (dev.description.manufacturer == "Nintendo")
            return CurrentControllerType.Nintendo;
        
        if (dev.description.interfaceName == "XInput")
            return CurrentControllerType.Xbox;

        return CurrentControllerType.Unknown;
    }
}
