using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonState : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed { get; private set; }
    
    public bool issy;

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
        issy = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
        issy = false;
    }
}
