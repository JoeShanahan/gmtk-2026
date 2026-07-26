using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuPage : MonoBehaviour
{
    public enum PageType
    {
        Root,
        LevelSelect,
        Settings,
        About
    }

    public PageType Type;

    [SerializeField] 
    private CanvasGroup _grp;

    [SerializeField] 
    private Transform _autoSelect;

    public void SnapTo(float x)
    {
        var rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(x, rt.anchoredPosition.y);
    }
    
    public void SlideTo(float x, float time, Ease ease)
    {
        var rt = GetComponent<RectTransform>();
        rt.DOAnchorPosX(x, time).SetEase(ease);
    }

    public void SetInteractable(bool yesNo)
    {
        _grp.interactable = yesNo;

        if (yesNo && _autoSelect != null)
        {
            EventSystem.current.SetSelectedGameObject(_autoSelect.gameObject);
        }
    }
}
