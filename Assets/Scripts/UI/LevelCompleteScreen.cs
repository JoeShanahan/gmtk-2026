using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _topBarTransform;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _timeValueText;
    [SerializeField] private TextMeshProUGUI _bestText;
    [SerializeField] private TextMeshProUGUI _bestValueText;
    [SerializeField] private TextMeshProUGUI _newRecordText;
    [SerializeField] private TextMeshProUGUI[] _rankings;

    [SerializeField] private LevelTimeListItem _parTime;
    [SerializeField] private LevelTimeListItem _birdieTime;

    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private RectTransform _bottomRect;
    [SerializeField] private Vector2 _bottomRectEndPos;
    [SerializeField] private Vector2 _bottomRectStartPos;
    
    [SerializeField] private Transform _nextLevelButton;
    [SerializeField] private Transform _retryButton;
    [SerializeField] private Transform _mainMenuButton;
    [SerializeField] private Transform _buttonParent;

    [SerializeField] private MainGameLevelSet _allLevels;
    
    [ContextMenu("TestA")]
    public void TestA()
    {
        CompleteLevel(10.4999f, 5.5f, _allLevels.SelectedLevel, LevelRank.Birdie);
    }
    
    [ContextMenu("TestB")]
    public void TestB()
    {
        CompleteLevel(3.5f, 5.5f, _allLevels.SelectedLevel, LevelRank.Bogey);
    }

    [ContextMenu("TestC")]
    public void TestC()
    {
        CompleteLevel(16.5412f, 16.5f, _allLevels.SelectedLevel, LevelRank.Par);
    }

    private LevelData _currentLevel;
    
    public void ButtonPressNext()
    {
        _allLevels.IncrementLevel();
        PersistentUI.DoTransition("RealGameScene");
        Time.timeScale = 1;
    }

    public void ButtonPressRetry()
    {
        FindAnyObjectByType<LevelManager>().RetryCurrentLevel();
    }

    public void ButtonPressMenu()
    {
        PersistentUI.DoTransition("MainMenuScene");
        Time.timeScale = 1;
    }
    
    public void CompleteLevel(float time, float bestTime, LevelData data, LevelRank rank)
    {
        _currentLevel = data;
        gameObject.SetActive(true);
        _topBarTransform.localScale = new Vector3(1, 0, 1);

        time = Mathf.FloorToInt(time * 10) / 10f;

        _timeValueText.text = time.ToString("F1") + "s";
        _bestValueText.text = bestTime.ToString("F1") + "s";
        _bottomRect.anchoredPosition = _bottomRectStartPos;
        
        _titleText.transform.localScale = Vector3.zero;
        _timeText.transform.localScale = Vector3.zero;
        _timeValueText.transform.localScale = Vector3.zero;
        _bestText.transform.localScale = Vector3.zero;
        _bestValueText.transform.localScale = Vector3.zero;
        _newRecordText.transform.localScale = Vector3.zero;
        _rankings[0].transform.localScale = Vector3.zero;
        _rankings[1].transform.localScale = Vector3.zero;
        _rankings[2].transform.localScale = Vector3.zero;
        _levelNameText.text = data.LevelName;
        
        _rankings[0].gameObject.SetActive(rank == LevelRank.Bogey);
        _rankings[1].gameObject.SetActive(rank == LevelRank.Par);
        _rankings[2].gameObject.SetActive(rank == LevelRank.Birdie);
        
        _parTime.Init(data.SecondsPar, bestTime);
        _birdieTime.Init(data.SecondsBirdie, bestTime);
        _buttonParent.gameObject.SetActive(false);
        _buttonParent.localScale = Vector3.zero;

        bool isBestTime = time < bestTime || bestTime < 0.1f;
        _bestText.gameObject.SetActive(!isBestTime);
        _bestValueText.gameObject.SetActive(!isBestTime);
        _newRecordText.gameObject.SetActive(isBestTime);

        bool isLastLevel = _allLevels.IsLastLevel();
        
        StartCoroutine(AnimateRoutine(isBestTime, isLastLevel));
    }

    private IEnumerator AnimateRoutine(bool isBestTime, bool isLastLevel)
    {
        _topBarTransform.DOScaleY(1, 0.7f).SetEase(Ease.OutExpo).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.2f);

        _titleText.transform.DOScale(1, 0.65f).SetEase(Ease.OutBack).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.5f);
        
        _timeText.transform.DOScale(1, 0.6f).SetEase(Ease.OutExpo).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.2f);
        _timeValueText.transform.DOScale(1, 0.6f).SetEase(Ease.OutExpo).SetUpdate(true);
        yield return new WaitForSecondsRealtime(0.8f);

        if (isBestTime)
        {
            _newRecordText.transform.DOScale(1, 0.6f).SetEase(Ease.OutBack).SetUpdate(true);
        }
        else
        {
            _bestText.transform.DOScale(1, 0.6f).SetEase(Ease.OutExpo).SetUpdate(true);
            yield return new WaitForSecondsRealtime(0.2f);
            _bestValueText.transform.DOScale(1, 0.6f).SetEase(Ease.OutExpo).SetUpdate(true);
        }
        
        yield return new WaitForSecondsRealtime(0.9f);

        _rankings[0].transform.DOScale(1, 0.65f).SetEase(Ease.OutBack).SetUpdate(true);
        _rankings[1].transform.DOScale(1, 0.65f).SetEase(Ease.OutBack).SetUpdate(true);
        _rankings[2].transform.DOScale(1, 0.65f).SetEase(Ease.OutBack).SetUpdate(true);

        yield return new WaitForSecondsRealtime(0.8f);

        _bottomRect.DOAnchorPos(_bottomRectEndPos, 0.8f).SetEase(Ease.OutExpo).SetUpdate(true);
        
        _retryButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);
        _buttonParent.gameObject.SetActive(true);
        _buttonParent.DOScale(1, 0.6f).SetEase(Ease.OutExpo).SetUpdate(true);

        if (!isLastLevel)
        {
            _nextLevelButton.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_nextLevelButton.gameObject);
        }
        else
        {
            _nextLevelButton.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_mainMenuButton.gameObject);
        }
    }
}
