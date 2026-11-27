using System;
using UnityEngine;
using UnityEngine.Events;

public class SelectionShield : MonoBehaviour
{
    [SerializeField] private SpriteRenderer interiorSprite;
    [SerializeField] private SpriteRenderer darkSprite;
    [SerializeField] private SpriteRenderer selectedSprite;

    public UnityEvent<int> OnClickShield = new UnityEvent<int>();

    private OpponentSelection opponentSelection;
    private JoustDifficultyData data;
    public JoustDifficultyData Data => data;

    public int index { get; private set; }
    public bool isInterractable { get; private set; }

    public void Setup(OpponentSelection parent, JoustDifficultyData joustDifficultyData, int shieldIndex, bool isShieldInterractable)
    {
        opponentSelection = parent;
        data = joustDifficultyData;
        index = shieldIndex;
        isInterractable = isShieldInterractable;

        interiorSprite.color = data.EnemyData.knightColor;

        opponentSelection.OnValidateShield.AddListener(() => UpdateValidationVisual(true));
        opponentSelection.OnOpen.AddListener(() => UpdateValidationVisual(true));
        opponentSelection.OnClose.AddListener(() => UpdateValidationVisual(false));
    }

    private void UpdateValidationVisual(bool isSelectionOpen)
    {
        if (isSelectionOpen)
        {
            darkSprite.gameObject.SetActive(!isInterractable || (opponentSelection.hasValidatedSelection && opponentSelection.CurrentValidatedIndex != index));
            selectedSprite.gameObject.SetActive(isInterractable && opponentSelection.hasValidatedSelection && opponentSelection.CurrentValidatedIndex == index);
        }
        else
        {
            darkSprite.gameObject.SetActive(false);
            selectedSprite.gameObject.SetActive(isInterractable && opponentSelection.hasValidatedSelection && opponentSelection.CurrentValidatedIndex == index);
        }
    }

    public void OnMouseDown()
    {
        OnClickShield?.Invoke(index);
    }
}
