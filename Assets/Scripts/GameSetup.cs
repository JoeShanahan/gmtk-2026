using UnityEngine;

public class GameSetup : MonoBehaviour
{
    [SerializeField] private SaveData _saveData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Application.targetFrameRate = 60;
        _saveData?.LoadFromPrefs();
    }
}
