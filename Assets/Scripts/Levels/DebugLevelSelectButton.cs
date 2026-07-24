using TMPro;
using UnityEngine;

public class DebugLevelSelectButton : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _text;

    private LevelData _levelData;

    public void Init(LevelData data)
    {
        _text.text = data.name;
        _levelData = data;
    }
    
    public void OnPress()
    {
        FindAnyObjectByType<LevelManager>().InstantiateLevel(_levelData);
    }
}
