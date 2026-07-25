using TMPro;
using UnityEngine;

public class StartLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private LevelTimeListItem _birdieTime;
    [SerializeField] private LevelTimeListItem _parTime;
    [SerializeField] private LevelTimeListItem _myTime;

    public void SetLevel(LevelData toLoad)
    {
        _levelName.text = toLoad.LevelName;
        _birdieTime.Init(toLoad.SecondsBirdie, toLoad.MyBestTime);
        _parTime.Init(toLoad.SecondsPar, toLoad.MyBestTime);
        _myTime.SetMyTime(toLoad.MyBestTime);
    }
}
