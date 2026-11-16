using UnityEngine;

public class ObstacleScroller : MonoBehaviour
{
    [SerializeField] private JoustPlayerHealth playerHealth;

    [Space]
    [SerializeField] private Transform damageLine;
    [SerializeField] private Transform destroyLine;

    private JoustPlayerStateMachine joustPlayer;
    private ObstacleSpawner obstacleSpawner;

    private void Start()
    {
        joustPlayer = JoustPlayerStateMachine.instance;
        obstacleSpawner = GetComponent<ObstacleSpawner>();
    }

    private void LateUpdate()
    {
        float movement = Odometer.instance.CurrentMovement;
        float damagePosition = damageLine.position.y;
        float destroyPosition = destroyLine.position.y;

        for (int i = obstacleSpawner.obstacles.Count - 1; i >= 0; i--)
        {
            Obstacle obstacle = obstacleSpawner.obstacles[i];
            Transform child = obstacle.objectTransform;
            if (child.position.y <= destroyPosition)
            {
                obstacleSpawner.obstacles.RemoveAt(i);
                Destroy(child.gameObject);
                continue;
            }
            else if (!obstacle.hasDealtDamage && child.position.y <= damagePosition)
            {
                TryDealDamageToPlayer(obstacle);
                obstacle.hasDealtDamage = true;
            }

            child.position += Vector3.down * movement;
        }
    }

    private void TryDealDamageToPlayer(Obstacle obstacle)
    {
        int lane = obstacle.assignedLane;

        if (obstacle.chunkData.obstacleType == ObstacleType.Knight)
        {
            lane = obstacle.objectTransform.GetComponent<JoustEnemyMovement>().CurrentLane;
        }

        if (playerHealth.CanTakeDamage(obstacle, lane))
        {
            playerHealth.TakeDamage();
        }
    }
}
