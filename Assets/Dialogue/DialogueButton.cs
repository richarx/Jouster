using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueButton : MonoBehaviour, IPointerClickHandler
{
    private Action onClick;

    public void Setup()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    public void SetOnClick(Action callback)
    {
        onClick = callback;
    }
}
