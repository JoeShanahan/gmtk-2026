using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InGamePauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _grp;
    [SerializeField] private RectTransform _firstButton;
    [SerializeField] private GameSettings _settings;
    [SerializeField] private TimeManager _timeMan;
    [SerializeField] private Transform _mainGameUI;
    
    private bool _isActive;
    
    private InputSystem_Actions _input;
    
    public void ToggleMenu()
    {
        if (!_mainGameUI.gameObject.activeSelf)
            return;
        
        _isActive = !_isActive;

        if (_isActive)
        {
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(_firstButton.gameObject);
            _grp.interactable = true;
            _grp.gameObject.SetActive(true);
            DOTween.Kill(_grp);
            var tween = _grp.DOFade(1, 0.2f).SetEase(Ease.OutSine);
            tween.SetUpdate(true);
        }
        else
        {
            _grp.interactable = false;
            DOTween.Kill(_grp);
            var tween = _grp.DOFade(0, 0.2f).SetEase(Ease.OutSine).OnComplete(() => _grp.gameObject.SetActive(false));
            tween.SetUpdate(true);
            _timeMan.OnPauseMenuClosed();
        }
    }


    private void OnDestroy()
    {
        _input?.Disable();   
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
        _grp.gameObject.SetActive(false);
        _grp.alpha = 0;
    }

    public void ButtonPressContinue()
    {
        ToggleMenu();
    }

    public void ButtonPressSound()
    {
        
    }

    public void ButtonPressMusic()
    {
        
    }

    public void ButtonPressAutoPause()
    {
        
    }

    public void ButtonPressSpeed()
    {
        
    }
    
    public void ButtonPressClose()
    {
        _grp.interactable = false;
        PersistentUI.DoTransition("MainMenuScene");
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.GameControl.Menu.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }
}
