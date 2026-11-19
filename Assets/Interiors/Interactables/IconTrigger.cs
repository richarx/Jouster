using BoomLib.Inputs;
using UnityEngine;
using UnityEngine.Events;

public class IconTrigger : BasicTrigger
{
    [SerializeField] private InteractIcon interactIconPrefab;
    [SerializeField] private Vector2 iconOffset = new Vector2(0.0f, 1.0f);

    public UnityEvent OnTrigger = new UnityEvent();

    protected InteractIcon interactIcon;

    protected virtual void Start()
    {
        OnPlayerEnterTrigger.AddListener(DisplayIcon);
        OnPlayerExitTrigger.AddListener(HideIcon);
        InputPacker.OnChangeInputType.AddListener(UpdateIconType);

        interactIcon = Instantiate(interactIconPrefab, transform.position, Quaternion.identity, transform);
        interactIcon.Setup(iconOffset);
    }

    public void DisplayIcon()
    {
        interactIcon.DisplayIcon();
        UpdateIconType(PlayerMovement.instance.CurrentInputs.lastInputType);
    }

    public void HideIcon()
    {
        interactIcon.HideIcon();
    }

    private void Update()
    {
        if (isPlayerInRange && PlayerMovement.instance.CurrentInputs.GetInteract.wasPressedThisFrame)
        {
            OnTrigger?.Invoke();
        }

        if (interactIcon.IsDisplayed)
            UpdateIconType(PlayerMovement.instance.CurrentInputs.lastInputType);
    }

    private void UpdateIconType(InputType inputType)
    {
        interactIcon.SetIconType(inputType);
    }
}
