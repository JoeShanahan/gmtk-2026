using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public static class SpeedSettings
{
    public static float DEFAULT = 0.05f;
    public static float SLOW = 0.02f;
    public static float STOPPED = 0.0f;
}

public class InGamePauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _grp;
    [SerializeField] private RectTransform _firstButton;
    [SerializeField] private GameSettings _settings;
    [SerializeField] private PauseManager _pauseMan;
    [SerializeField] private Transform _mainGameUI;

    [Header("Texts")] 
    [SerializeField] private TextMeshProUGUI _soundOnText;
    [SerializeField] private TextMeshProUGUI _soundOffText;
    [SerializeField] private TextMeshProUGUI _musicOnText;
    [SerializeField] private TextMeshProUGUI _musicOffText;
    [SerializeField] private TextMeshProUGUI _autoPauseOnText;
    [SerializeField] private TextMeshProUGUI _autoPauseOffText;
    [SerializeField] private TextMeshProUGUI _pauseSpeedDefaultText;
    [SerializeField] private TextMeshProUGUI _pauseSpeedSlowText;
    [SerializeField] private TextMeshProUGUI _pauseSpeedStoppedText;
    
    private bool _isActive;
    
    private InputSystem_Actions _input;
    
    public void ToggleMenu()
    {
        if (!_mainGameUI.gameObject.activeSelf)
            return;
        
        _isActive = !_isActive;

        if (_isActive)
        {
            RefreshTexts();
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
            _pauseMan.OnPauseMenuClosed();
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

    private void RefreshTexts()
    {
        _soundOnText.gameObject.SetActive(_settings.SoundEnabled);
        _soundOffText.gameObject.SetActive(_settings.SoundEnabled == false);
        _musicOnText.gameObject.SetActive(_settings.MusicEnabled);
        _musicOffText.gameObject.SetActive(_settings.MusicEnabled == false);
        _autoPauseOnText.gameObject.SetActive(_settings.AutoPauseOnSwap);
        _autoPauseOffText.gameObject.SetActive(_settings.AutoPauseOnSwap == false);
        _pauseSpeedDefaultText.gameObject.SetActive(Mathf.Approximately(_settings.PauseSpeed, SpeedSettings.DEFAULT));
        _pauseSpeedSlowText.gameObject.SetActive(Mathf.Approximately(_settings.PauseSpeed, SpeedSettings.SLOW));
        _pauseSpeedStoppedText.gameObject.SetActive(Mathf.Approximately(_settings.PauseSpeed, SpeedSettings.STOPPED));
    }

    public void ButtonPressSound()
    {
        _settings.SoundEnabled = !_settings.SoundEnabled;
        _settings.SaveToPrefs();
        RefreshTexts();
    }

    public void ButtonPressMusic()
    {
        _settings.MusicEnabled = !_settings.MusicEnabled;
        _settings.SaveToPrefs();
        RefreshTexts();
    }

    public void ButtonPressAutoPause()
    {
        _settings.AutoPauseOnSwap = !_settings.AutoPauseOnSwap;
        _settings.SaveToPrefs();
        RefreshTexts();
    }

    public void ButtonPressSpeed()
    {
        if (Mathf.Approximately(_settings.PauseSpeed, SpeedSettings.DEFAULT))
        {
            _settings.PauseSpeed = SpeedSettings.SLOW;
        }
        else if (Mathf.Approximately(_settings.PauseSpeed, SpeedSettings.SLOW))
        {
            _settings.PauseSpeed = SpeedSettings.STOPPED;
        }
        else
        {
            _settings.PauseSpeed = SpeedSettings.DEFAULT;
        }
        _settings.SaveToPrefs();
        RefreshTexts();
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
