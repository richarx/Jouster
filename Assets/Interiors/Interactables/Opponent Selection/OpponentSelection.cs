using System;
using System.Collections.Generic;
using BoomLib.Inputs;
using BoomTools;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class OpponentSelection : MonoBehaviour
{
    [SerializeField] private SelectionShield selectionShieldPrefab;
    [SerializeField] private Vector2 offsetPlayerPosition;

    public UnityEvent OnOpen = new UnityEvent();
    public UnityEvent OnClose = new UnityEvent();
    public UnityEvent OnSelectShield = new UnityEvent();
    public UnityEvent OnValidateShield = new UnityEvent();

    private int currentShieldIndex;
    private int currentValidatedIndex = -1;
    public int CurrentValidatedIndex => currentValidatedIndex;
    public bool hasValidatedSelection => currentValidatedIndex >= 0;
    private List<SelectionShield> shields = new List<SelectionShield>();
    public JoustDifficultyData CurrentSelection => shields[currentShieldIndex].Data;
    public JoustDifficultyData CurrentValidation => hasValidatedSelection ? shields[currentValidatedIndex].Data : null;
    public Vector3 CurrentSelectionPosition => shields[currentShieldIndex].transform.position;

    private Vector3 playerTargetPosition => new Vector3(transform.position.x + offsetPlayerPosition.x, transform.position.y + offsetPlayerPosition.y, transform.position.z);

    private IconTrigger iconTrigger;

    private bool isSelectionOpen;

    private bool hasMoveInputReset = true;
    private bool hasInteractInputReset = true;


    private void Start()
    {
        iconTrigger = GetComponent<IconTrigger>();
        iconTrigger.OnTrigger.AddListener(OpenOpponentSelection);

        SetupShields();
    }

    private void SetupShields()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0.0f, -0.6f, 0.0f);
        foreach (JoustDifficultyData data in JoustPreparation.instance.JoustDifficultyDatas)
        {
            SelectionShield selectionShield = Instantiate(selectionShieldPrefab, spawnPosition, Quaternion.identity, transform);
            selectionShield.Setup(this, data, shields.Count);
            selectionShield.OnClickShield.AddListener(SelectShield);
            shields.Add(selectionShield);

            spawnPosition += Vector3.right * 1.5f;
        }
    }

    private void SelectShield(int index)
    {
        if (!isSelectionOpen)
            return;

        index = Math.Clamp(index, 0, shields.Count - 1);

        if (index == currentShieldIndex)
            return;

        currentShieldIndex = index;
        OnSelectShield?.Invoke();
    }

    private void ValidateShield()
    {
        if (currentValidatedIndex == currentShieldIndex)
            return;

        if (!JoustPreparation.instance.IsRegistered)
            return;

        currentValidatedIndex = currentShieldIndex;
        OnValidateShield?.Invoke();
    }

    private void Update()
    {
        if (!isSelectionOpen)
            return;

        InputPackage inputPackage = PlayerMovement.instance.CurrentInputs;

        if (inputPackage.GetQuitInteraction.wasPressedThisFrame)
        {
            CloseOpponentSelection();
            return;
        }

        if (inputPackage.GetInteract.wasPressedThisFrame)
        {
            if (hasInteractInputReset)
            {
                ValidateShield();
                hasInteractInputReset = false;
                return;
            }
        }
        else
            hasInteractInputReset = true;

        if (inputPackage.GetMove.magnitude >= 0.15f)
        {
            if (hasMoveInputReset)
            {
                SelectShield(currentShieldIndex + Math.Sign(PlayerMovement.instance.CurrentInputs.GetMove.x));
                hasMoveInputReset = false;
            }
        }
        else
            hasMoveInputReset = true;
    }

    private void OpenOpponentSelection()
    {
        if (isSelectionOpen)
            return;

        if (hasValidatedSelection)
            currentShieldIndex = currentValidatedIndex;
        else
            currentShieldIndex = 0;
        isSelectionOpen = true;
        hasInteractInputReset = false;

        PlayerMovement.instance.SetLockState(true, playerTargetPosition, Vector2.up);

        iconTrigger.HideIcon();
        OnOpen?.Invoke();
        OnSelectShield?.Invoke();
    }

    private void CloseOpponentSelection()
    {
        if (!isSelectionOpen)
            return;

        isSelectionOpen = false;
        PlayerMovement.instance.SetLockState(false);

        iconTrigger.DisplayIcon();
        OnClose?.Invoke();
    }
}
