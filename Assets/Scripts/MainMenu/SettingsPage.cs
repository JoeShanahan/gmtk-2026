using System;
using TMPro;
using UnityEngine;

public static class ScreenShakeSettings
{
    public static float DEFAULT = 1.0f;
    public static float LESS = 0.4f;
    public static float STOPPED = 0.0f;
}

public class SettingsPage : MonoBehaviour
{
    [SerializeField] private GameSettings _settings;

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
    
    [SerializeField] private TextMeshProUGUI _shakeDefaultText;
    [SerializeField] private TextMeshProUGUI _shakeLessText;
    [SerializeField] private TextMeshProUGUI _shakeNoneText;
    
    public void OnEnable()
    {
        RefreshTexts();
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
        
        _shakeDefaultText.gameObject.SetActive(Mathf.Approximately(_settings.ScreenShakeModifier, ScreenShakeSettings.DEFAULT));
        _shakeLessText.gameObject.SetActive(Mathf.Approximately(_settings.ScreenShakeModifier, ScreenShakeSettings.LESS));
        _shakeNoneText.gameObject.SetActive(Mathf.Approximately(_settings.ScreenShakeModifier, ScreenShakeSettings.STOPPED));
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
    
    public void ButtonPressScreenShake()
    {
        if (Mathf.Approximately(_settings.ScreenShakeModifier, ScreenShakeSettings.DEFAULT))
        {
            _settings.ScreenShakeModifier = ScreenShakeSettings.LESS;
        }
        else if (Mathf.Approximately(_settings.ScreenShakeModifier, ScreenShakeSettings.LESS))
        {
            _settings.ScreenShakeModifier = ScreenShakeSettings.STOPPED;
        }
        else
        {
            _settings.ScreenShakeModifier = ScreenShakeSettings.DEFAULT;
        }
        _settings.SaveToPrefs();
        RefreshTexts();
    }
}
