using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public bool SoundEnabled = true;
    public bool MusicEnabled = true;
    public float PauseSpeed = 0.05f;
    public bool AutoPauseOnSwap;
    public bool ScreenShakeModifier = true; // On by default
    
    public void SaveToPrefs()
    {
#if !UNITY_EDITOR
        PlayerPrefs.SetString("SettingsData", JsonUtility.ToJson(this));
        PlayerPrefs.Save();
#endif
    }

    public void LoadFromPrefs()
    {
#if !UNITY_EDITOR
        if (PlayerPrefs.HasKey("SettingsData"))
        {
            string jsonBlob = PlayerPrefs.GetString("SettingsData");
            JsonUtility.FromJsonOverwrite(jsonBlob, this);
        }
#endif
    }
}
