using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform _quitButton;
    [SerializeField] private int _activePos;
    [SerializeField] private int _inactivePos;
    [SerializeField] private float _transitionTime;
    [SerializeField] private Ease _ease;
    
    private MainMenuPage[] _allPages;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
// #if UNITY_WEBGL
    _quitButton.gameObject.SetActive(false);
// #endif
    }

    private void SwapPage(MainMenuPage.PageType ptype)
    {
        if (_allPages == null || _allPages.Length == 0)
        {
            _allPages = FindObjectsByType<MainMenuPage>(FindObjectsInactive.Include);

        }
        
        MainMenuPage foundPage = _allPages.FirstOrDefault(p => p.Type == ptype);

        if (foundPage == null)
            return;
        
        foreach (MainMenuPage p in _allPages)
        {
            if (p.Type == ptype)
            {
                p.SnapTo(_inactivePos);
                p.SlideTo(_activePos, _transitionTime, _ease);
            }
            else
            {
                p.SlideTo(_inactivePos, _transitionTime, _ease);
            }
        }
    }

    public void BtnPressQuit()
    {
        Application.Quit();
    }

    public void SwapToRoot() => SwapPage(MainMenuPage.PageType.Root);
    public void SwapToSettings() => SwapPage(MainMenuPage.PageType.Settings);
    public void SwapToAbout() => SwapPage(MainMenuPage.PageType.About);
    public void SwapToLevelSelect() => SwapPage(MainMenuPage.PageType.LevelSelect);
}
