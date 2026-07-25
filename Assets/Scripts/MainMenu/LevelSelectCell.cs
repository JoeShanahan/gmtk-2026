using TMPro;
using UnityEngine;

public class LevelSelectCell : MonoBehaviour
{
    [SerializeField] private Transform[] _stars;
    [SerializeField] private TextMeshProUGUI _text;
    private MainGameLevelSet _levelSet;
    private LevelData _level;
    
    public void Init(LevelData levelData, MainGameLevelSet levelSet)
    {
        _text.text = (levelData.MainGameIndex + 1).ToString();
        _levelSet = levelSet;
        _level = levelData;
        
        foreach (Transform t in _stars)
        {
            t.gameObject.SetActive(false);
        }
    }
    
    public void SetThisLevel()
    {
        _levelSet.SelectLevel(_level.MainGameIndex);
    }
}
