using UnityEngine;

public class JoustManager : MonoBehaviour
{
    [SerializeField] private JoustDifficultyData data;

    [Space]
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private JoustPlayerMovement joustPlayer;
    [SerializeField] private GroundManager groundManager;

    private void Start()
    {
        obstacleSpawner.Setup(data);
        joustPlayer.Setup(data);
        groundManager.Setup(data);
    }
}
