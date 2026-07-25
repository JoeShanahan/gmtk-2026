using TMPro;
using UnityEngine;

public class StartLevelScreen : MonoBehaviour
{
    public LevelData DebugToLoad;

    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private LevelTimeListItem _birdieTime;
    [SerializeField] private LevelTimeListItem _parTime;
    [SerializeField] private LevelTimeListItem _myTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetLevel(DebugToLoad);
    }

    public void SetLevel(LevelData toLoad)
    {
        _levelName.text = toLoad.LevelName;
        _birdieTime.Init(toLoad.SecondsBirdie, toLoad.MyBestTime);
        _parTime.Init(toLoad.SecondsPar, toLoad.MyBestTime);
        _myTime.SetMyTime(toLoad.MyBestTime);
    }
}
