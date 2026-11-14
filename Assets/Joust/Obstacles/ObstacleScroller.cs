using UnityEngine;

public class ObstacleScroller : MonoBehaviour
{
    [SerializeField] private Transform damageLine;
    [SerializeField] private Transform destroyLine;

    private JoustPlayerMovement joustPlayer;
    private ObstacleSpawner obstacleSpawner;

    private void Start()
    {
        joustPlayer = JoustPlayerMovement.instance;
        obstacleSpawner = GetComponent<ObstacleSpawner>();
    }

    private void LateUpdate()
    {
        float movement = joustPlayer.Velocity * Time.deltaTime;
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
            else if (!obstacle.hasDealtDamage && child.position.z <= damagePosition)
            {
                TryDealDamageToPlayer(obstacle);
                obstacle.hasDealtDamage = true;
            }

            child.position += Vector3.down * movement;
        }
    }

    private void TryDealDamageToPlayer(Obstacle obstacle)
    {
        if (joustPlayer.CurrentLaneIndex == obstacle.assignedLane)
            Debug.Log("Player was hit !");
    }
}
