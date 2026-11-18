using Map;
using UnityEngine;

public class JoustInscription : MonoBehaviour
{
    [SerializeField] private RuntimeDialogueGraph dialogueAfterInscription;

    private void Start()
    {
        DialogueTrigger dialogueTrigger = GetComponent<DialogueTrigger>();

        if (JoustPreparation.instance.IsRegistered)
            dialogueTrigger.SetDialogue(dialogueAfterInscription);
        else
            dialogueTrigger.OnDialogueCallback.AddListener(ConfirmInscription);
    }

    private void ConfirmInscription(DialogueEvent dialogueEvent)
    {
        if (dialogueEvent == DialogueEvent.Inscription)
        {
            GetComponent<DialogueTrigger>().SetDialogue(dialogueAfterInscription);
            JoustPreparation.instance.RegisterPlayer();
        }
    }
}
