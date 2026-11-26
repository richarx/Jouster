using Map;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : IconTrigger
{
    [SerializeField] private RuntimeDialogueGraph dialogueGraph;

    public UnityEvent<DialogueEvent> OnDialogueCallback = new UnityEvent<DialogueEvent>();

    protected override void Start()
    {
        base.Start();
        OnTrigger.AddListener(StartDialogue);
    }

    private void StartDialogue()
    {
        if (!isActive)
            return;

        DialogueManager.instance.StartDialogue(dialogueGraph, EndDialogueCallback, EventCallback);
        interactIcon.HideIcon();
    }

    private void EndDialogueCallback()
    {
        interactIcon.DisplayIcon();
    }

    protected virtual void EventCallback(DialogueEvent dialogueEvent)
    {
        OnDialogueCallback?.Invoke(dialogueEvent);
    }

    public void SetDialogue(RuntimeDialogueGraph newDialogue)
    {
        dialogueGraph = newDialogue;
    }
}
