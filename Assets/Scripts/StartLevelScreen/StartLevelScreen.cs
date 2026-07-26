using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartLevelScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private LevelTimeListItem _birdieTime;
    [SerializeField] private LevelTimeListItem _parTime;
    [SerializeField] private LevelTimeListItem _myTime;
    [SerializeField] private Transform _buttonToSelect;

    public void OnEnable()
    {
        if (_buttonToSelect != null)
            EventSystem.current.SetSelectedGameObject(_buttonToSelect.gameObject);
    }

    public void SetLevel(LevelData toLoad)
    {
        _levelName.text = toLoad.LevelName;
        _birdieTime.Init(toLoad.SecondsBirdie, toLoad.MyBestTime);
        _parTime.Init(toLoad.SecondsPar, toLoad.MyBestTime);
        _myTime.SetMyTime(toLoad.MyBestTime);
    }

    public void ButtonPressMenu()
    {
        PersistentUI.DoTransition("MainMenuScene");

    }

    public void ButtonPressStart()
    {
        FindAnyObjectByType<LevelManager>()?.BeginLevel();
    }
}
