using UnityEngine;
using UnityEngine.UI;

public class HitPoint : MonoBehaviour
{
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private Image image;

    private bool isActive = true;
    public bool IsActive => isActive;

    private void Start()
    {
        SetSprite();
    }

    public void SetState(bool state)
    {
        isActive = state;
        SetSprite();
    }

    private void SetSprite()
    {
        if (image == null)
            image = GetComponent<Image>();

        image.sprite = isActive ? activeSprite : inactiveSprite;
    }
}
