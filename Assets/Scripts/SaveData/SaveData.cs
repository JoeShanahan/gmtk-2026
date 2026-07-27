using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveData", menuName = "Scriptable Objects/SaveData")]
public class SaveData : ScriptableObject
{
    [SerializeField]
    private List<float> _bestTimes;

    public bool IsLevelComplete(int levelIndex)
    {
        return GetBestTime(levelIndex) > 0;
    }
    
    public float GetBestTime(int levelIndex)
    {
        if (_bestTimes.Count <= levelIndex)
            return 0;

        return _bestTimes[levelIndex];
    }

    public void SetBestTime(int levelIndex, float seconds)
    {
        while (_bestTimes.Count <= levelIndex)
        {
            _bestTimes.Add(0);
        }

        if (_bestTimes[levelIndex] < 0.1f || seconds < _bestTimes[levelIndex])
        {
            _bestTimes[levelIndex] = seconds;
            SaveToPrefs();
        }
    }
    
    public void SaveToPrefs()
    {
#if !UNITY_EDITOR
        PlayerPrefs.SetString("SaveData", JsonUtility.ToJson(this));
        PlayerPrefs.Save();
#endif
    }
    
    public void LoadFromPrefs()
    {
#if !UNITY_EDITOR
        if (PlayerPrefs.HasKey("SaveData"))
        {
            string jsonBlob = PlayerPrefs.GetString("SaveData");
            JsonUtility.FromJsonOverwrite(jsonBlob, this);
        }
#endif
    }
}
