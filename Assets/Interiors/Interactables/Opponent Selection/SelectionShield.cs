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

    private int index;
    public int Index => index;

    public void Setup(OpponentSelection parent, JoustDifficultyData joustDifficultyData, int shieldIndex)
    {
        opponentSelection = parent;
        data = joustDifficultyData;
        index = shieldIndex;

        interiorSprite.color = data.EnemyData.knightColor;

        opponentSelection.OnValidateShield.AddListener(UpdateValidationVisual);
    }

    private void UpdateValidationVisual()
    {
        darkSprite.gameObject.SetActive(opponentSelection.hasValidatedSelection && opponentSelection.CurrentValidatedIndex != index);
        selectedSprite.gameObject.SetActive(opponentSelection.hasValidatedSelection && opponentSelection.CurrentValidatedIndex == index);
    }

    public void OnMouseDown()
    {
        OnClickShield?.Invoke(index);
    }
}
