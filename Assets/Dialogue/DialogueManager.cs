using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Map;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choice Button UI")]
    [SerializeField] private DialogueButton choiceButtonPrefab;
    [SerializeField] private Transform choiceButtonPanel;

    public static DialogueManager instance;

    private Dictionary<string, RuntimeDialogueNode> nodeLookup = new Dictionary<string, RuntimeDialogueNode>();
    private RuntimeDialogueNode currentNode;

    private Action onDialogueEnd;
    private Action<DialogueEvent> onDialogueEvent;

    private void Awake()
    {
        instance = this;
    }

    public void StartDialogue(RuntimeDialogueGraph dialogueGraph, Action endDialogueCallback, Action<DialogueEvent> dialogueEventCallback)
    {
        onDialogueEnd = null;
        onDialogueEvent = null;
        currentNode = null;
        nodeLookup.Clear();

        onDialogueEnd = endDialogueCallback;
        onDialogueEvent = dialogueEventCallback;

        PlayerMovement.instance.SetLockState(true);

        foreach (RuntimeDialogueNode node in dialogueGraph.AllNodes)
        {
            nodeLookup[node.NodeID] = node;
        }

        if (!string.IsNullOrEmpty(dialogueGraph.EntryNodeID))
        {
            ShowNode(dialogueGraph.EntryNodeID);
        }
        else
        {
            EndDialogue();
        }
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && currentNode != null && currentNode.Choices.Count == 0)
        {
            if (!string.IsNullOrEmpty(currentNode.NextNodeID))
            {
                ShowNode(currentNode.NextNodeID);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void ShowNode(string nodeID)
    {
        if (!nodeLookup.ContainsKey(nodeID))
        {
            EndDialogue();
            return;
        }

        currentNode = nodeLookup[nodeID];

        bool isEventNode = currentNode.dialogueEvent != DialogueEvent.None;

        if (isEventNode)
        {
            SendEvent(currentNode.dialogueEvent);

            if (!string.IsNullOrEmpty(currentNode.NextNodeID))
            {
                ShowNode(currentNode.NextNodeID);
            }
            else
            {
                EndDialogue();
            }
            return;
        }

        dialoguePanel.SetActive(true);
        speakerNameText.SetText(currentNode.SpeakerName);
        dialogueText.SetText(currentNode.DialogueText);

        CleanPreviousButtons();

        bool isChoiceNode = currentNode.Choices.Count > 0;

        choiceButtonPanel.gameObject.SetActive(isChoiceNode);

        if (isChoiceNode)
        {
            foreach (var choice in currentNode.Choices)
            {
                DialogueButton button = Instantiate(choiceButtonPrefab, choiceButtonPanel);
                TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                    buttonText.text = choice.ChoiceText;

                if (button != null)
                {
                    button.SetOnClick(() =>
                    {
                        if (!string.IsNullOrEmpty(choice.DestinationNodeID))
                        {
                            ShowNode(choice.DestinationNodeID);
                        }
                        else
                        {
                            EndDialogue();
                        }
                    });
                }
            }
        }
    }

    private void SendEvent(DialogueEvent dialogueEvent)
    {
        switch (dialogueEvent)
        {
            case DialogueEvent.QuitJoust:
                Debug.Log("Quit Joust !");
                break;
            case DialogueEvent.Inscription:
                Debug.Log("Inscription !");
                break;
            case DialogueEvent.OpponentSelection:
                Debug.Log("Opponent Selection !");
                break;
            default:
            case DialogueEvent.None:
                Debug.Log("None !");
                break;
        }

        onDialogueEvent?.Invoke(dialogueEvent);
    }

    private void CleanPreviousButtons()
    {
        foreach (Transform child in choiceButtonPanel)
            Destroy(child.gameObject);
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNode = null;

        PlayerMovement.instance.SetLockState(false);

        CleanPreviousButtons();

        onDialogueEnd?.Invoke();
        onDialogueEnd = null;
        onDialogueEvent = null;
    }
}
