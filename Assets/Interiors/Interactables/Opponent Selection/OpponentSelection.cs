using Unity.Cinemachine;
using UnityEngine;

public class OpponentSelection : MonoBehaviour
{
    [SerializeField] private CinemachineCamera camera;
    [SerializeField] private SelectionShield selectionShieldPrefab;

    private IconTrigger iconTrigger;

    private bool isSelectionOpen;

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
            selectionShield.Setup(data);

            spawnPosition += Vector3.right * 1.5f;
        }
    }

    private void Update()
    {
        if (!isSelectionOpen)
            return;

        if (PlayerMovement.instance.CurrentInputs.GetQuitInteraction.wasPressedThisFrame)
            CloseOpponentSelection();
    }

    private void OpenOpponentSelection()
    {
        if (isSelectionOpen)
            return;

        isSelectionOpen = true;

        iconTrigger.HideIcon();
        camera.gameObject.SetActive(true);
    }

    private void CloseOpponentSelection()
    {
        if (!isSelectionOpen)
            return;

        isSelectionOpen = false;

        if (!JoustPreparation.instance.IsOpponentSelected)
            iconTrigger.DisplayIcon();
        camera.gameObject.SetActive(false);
    }
}
