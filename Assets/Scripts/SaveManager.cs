using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] SaveObject saveObject = new();
    
    public static SaveManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(saveObject);

#if UNITY_EDITOR
            File.WriteAllText(Application.persistentDataPath + "/Save.json", json);
#else
            PlayerPrefs.SetString("SaveData", json);
            PlayerPrefs.Save();
#endif  
            //return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save Failed: " + e);
            //return false;
        }
    }

    public void Load()
    {
        try
        {
#if UNITY_EDITOR 
            if (File.Exists(Application.persistentDataPath + "/Save.json"))
            {
                try
                {
                    string json = File.ReadAllText(Application.persistentDataPath + "/Save.json");
                    saveObject = JsonUtility.FromJson<SaveObject>(json);
                }
                catch
                {
                    Debug.Log("Save load failed, probably no save file so it's fine.");
                }
#else
            if (PlayerPrefs.HasKey("SaveData"))
            {
                string json = PlayerPrefs.GetString("SaveData");
                saveObject = JsonUtility.FromJson<SaveObject>(json);
#endif
                
                //return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Load Failed: " + e);
            //return false;
        }
    }

    public void SetLevelInfo(int level, float time)
    {
        LevelInfo existingData = saveObject.levelInfos.Find(info => info.level == level);

        if (existingData != null)
        {
            if (time < existingData.bestTime)
            {
                existingData.bestTime = time;
            }
        }
        else
        {
            LevelInfo newInfo = new LevelInfo();
            newInfo.level = level;
            newInfo.bestTime = time;
            saveObject.levelInfos.Add(newInfo);
        }
    }

}

[System.Serializable]
public class SaveObject
{
    public List<LevelInfo> levelInfos = new();
}

[System.Serializable]
public class LevelInfo
{
    public int level;
    public float bestTime;
}