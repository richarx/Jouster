using UnityEngine;

public class SelectionShield : MonoBehaviour
{
    [SerializeField] private SpriteRenderer interiorSprite;

    private JoustDifficultyData data;

    public void Setup(JoustDifficultyData joustDifficultyData)
    {
        data = joustDifficultyData;

        interiorSprite.color = data.EnemyData.knightColor;
    }
}
