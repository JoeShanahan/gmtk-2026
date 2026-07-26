using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MainGameLevelSet", menuName = "Scriptable Objects/MainGameLevelSet")]
public class MainGameLevelSet : ScriptableObject
{
    public IReadOnlyList<LevelData> Levels => _levels;
    public LevelData SelectedLevel { get; private set; }
    
    public int LevelCount => _levels.Count;

    [SerializeField] private SaveData _saveData;

    public void IncrementLevel()
    {
        int currentIdx = _levels.IndexOf(SelectedLevel);
        currentIdx++;
        SelectedLevel = _levels[currentIdx];
    }
    
    public LevelData GetLevel(int levelIndex)
    {
        if (levelIndex >= _levels.Count)
            return null;
        
        LevelData result = _levels[levelIndex];
        result.MainGameIndex = levelIndex;
        return result;
    }

    public bool IsLastLevel()
    {
        return _levels.Last() == SelectedLevel;
    }

    public int GetNumberOfStars()
    {
        // TODO
        return 0;
    }

    public void SelectLevel(int levelIndex)
    {
        SelectedLevel = GetLevel(levelIndex);
    }
    
    [SerializeField] private List<LevelData> _levels;
}
