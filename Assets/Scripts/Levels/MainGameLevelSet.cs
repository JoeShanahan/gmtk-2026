using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainGameLevelSet", menuName = "Scriptable Objects/MainGameLevelSet")]
public class MainGameLevelSet : ScriptableObject
{
    public IReadOnlyList<LevelData> Levels => _levels;

    public int LevelCount => _levels.Count;

    [SerializeField] private SaveData _saveData;

    public LevelData GetLevel(int levelIndex)
    {
        LevelData result = _levels[levelIndex];
        result.MainGameIndex = levelIndex;
        return result;
    }

    public int GetNumberOfStars()
    {
        // TODO
        return 0;
    }
    
    [SerializeField] private List<LevelData> _levels;
}
