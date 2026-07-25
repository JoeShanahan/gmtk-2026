using DG.Tweening;
using UnityEngine;

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
}
