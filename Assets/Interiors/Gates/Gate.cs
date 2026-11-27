using LDtkUnity;
using Map;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;
    [SerializeField] private GameObject hitbox;

    private GateType gateType;

    private bool isOpen = true;

    private void Start()
    {
        LDtkFields lDtkFields = GetComponent<LDtkFields>();
        gateType = lDtkFields.GetEnum<GateType>(nameof(GateType));

        RegisterToEvents();
        CloseGateInstant();
    }

    private void RegisterToEvents()
    {
        switch (gateType)
        {
            case GateType.Doctor:
                break;
            case GateType.Joust:
                JoustPreparation.instance.OnSelectOpponent.AddListener(OpenGate);
                break;
        }
    }

    private void OpenGate()
    {
        if (isOpen)
            return;

        isOpen = true;

        spriteRenderer.sprite = openSprite;
        hitbox.SetActive(false);
    }

    private void CloseGate()
    {
        if (!isOpen)
            return;

        isOpen = false;
        spriteRenderer.sprite = closeSprite;
        hitbox.SetActive(true);
    }

    private void CloseGateInstant()
    {
        if (!isOpen)
            return;

        isOpen = false;
        spriteRenderer.sprite = closeSprite;
        hitbox.SetActive(true);
    }
}
