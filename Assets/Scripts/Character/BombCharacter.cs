using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldToCanvas;

[RequireComponent(typeof(CharacterMovement))]
public class BombCharacter : MonoBehaviour
{
    InputSystem_Actions _input;
    private Vector2 _moveInput;
    CharacterMovement _movement;
    Camera _mainCam;

    public int RemainingTime => Mathf.Max(0, Mathf.CeilToInt(_remainingTime * 10));
    public bool IsBeingControlled { get; private set; }
    
    [SerializeField, Tooltip("30 = 3 seconds")] 
    private int _startTime;

    [SerializeField]
    private float _remainingTime;

    [SerializeField] 
    private GameObject _uiPrefab;

    private BombCharacterUI _uiInstance;
    
    void Start()
    {
        _movement = GetComponent<CharacterMovement>();
        
        _input = new();
        _mainCam = Camera.main;

        _remainingTime = _startTime / 10f;

        _uiInstance = W2CManager.InstantiateAs<BombCharacterUI>(_uiPrefab);
        _uiInstance.Init(this);
        
        BombManager.Register(this);
    }

    public void TakeControlOf()
    {
        _input.Enable();
        IsBeingControlled = true;
    }

    public void ReleaseControlOf()
    {
        _input.Disable();
        IsBeingControlled = false;
    }

    public void Explode()
    {
        GetComponent<ExplosionBase>()?.Explode(transform.position, transform.forward);
        Destroy(gameObject);
    }

    void Update()
    {
        HandlePlayerInput();
        _remainingTime -= Time.deltaTime;
        _remainingTime = Mathf.Max(_remainingTime, 0);
        
        _uiInstance.ProcessUpdate(this);

        if (_remainingTime <= 0)
        {
            Explode();
        }
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

    private void OnDestroy()
    {
        if (_uiInstance != null)
        {
            Destroy(_uiInstance.gameObject);
        }
        
        BombManager.Unregister(this);
        _input.Disable();
    }
}
