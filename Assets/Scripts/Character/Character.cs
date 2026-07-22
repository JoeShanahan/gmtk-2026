using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class Character : MonoBehaviour
{
    InputSystem_Actions _input;
    private Vector2 _moveInput;
    CharacterMovement _movement;
    Camera _mainCam;

    void Start()
    {
        _movement = GetComponent<CharacterMovement>();
        
        _input = new();
        _input.Enable();
        _mainCam = Camera.main;
    }

    void Update()
    {
        HandlePlayerInput();
    }
    
    void HandlePlayerInput()
    {
        _moveInput = _input.Player.Move.ReadValue<Vector2>();
        _movement ??= GetComponent<CharacterMovement>();
            
        var playerInput = Vector2.ClampMagnitude(_moveInput, 1);

        Vector3 camRight = _mainCam.transform.right;
        Vector3 camForward = _mainCam.transform.forward;
        camRight.y = camForward.y = 0;
        
        camRight.Normalize();
        camForward.Normalize();

        Vector3 xComponent = playerInput.x * camRight;
        Vector3 yComponent = playerInput.y * camForward;

        _movement.SetDesiredDirection(xComponent + yComponent);
    }
}
