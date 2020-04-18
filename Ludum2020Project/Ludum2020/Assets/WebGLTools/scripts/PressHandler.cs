using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class PressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler
{
    [Serializable]
    public class ButtonPressEvent : UnityEvent { }

    public ButtonPressEvent OnPress = new ButtonPressEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}