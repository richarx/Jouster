using System;
using BoomLib.Inputs;
using BoomTools;
using UnityEngine;
using UnityEngine.UI;

public class InteractIcon : MonoBehaviour
{
    [SerializeField] private Sprite gamepadIcon;
    [SerializeField] private Sprite keyboardIcon;

    private SpriteRenderer spriteRenderer;

    private InputType currentIconType;

    private bool isDisplayed;
    public bool IsDisplayed => isDisplayed;

    public void Setup(Vector2 iconOffset)
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.transform.localPosition = iconOffset;

        currentIconType = InputType.Keyboard;
        SetIconType(InputType.Gamepad);

        HideInstant();
    }

    public void DisplayIcon()
    {
        if (isDisplayed)
            return;

        isDisplayed = true;
        spriteRenderer.gameObject.SetActive(true);
    }

    public void HideIcon()
    {
        if (!isDisplayed)
            return;

        isDisplayed = false;

        spriteRenderer.gameObject.SetActive(false);
    }

    public void SetIconType(InputType inputType)
    {
        if (currentIconType == inputType)
            return;

        switch (inputType)
        {
            case InputType.Gamepad:
                spriteRenderer.sprite = gamepadIcon;
                break;
            case InputType.Keyboard:
                spriteRenderer.sprite = keyboardIcon;
                break;
        }

        currentIconType = inputType;
    }

    private void HideInstant()
    {
        spriteRenderer.gameObject.SetActive(false);
    }
}
