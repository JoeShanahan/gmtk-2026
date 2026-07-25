using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainGameLevelSet", menuName = "Scriptable Objects/MainGameLevelSet")]
public class MainGameLevelSet : ScriptableObject
{
    public IReadOnlyList<LevelData> Levels => _levels;
    
    [SerializeField] private List<LevelData> _levels;
}
