using System;
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
        float damagePosition = damageLine.position.z;
        float destroyPosition = destroyLine.position.z;

        for (int i = obstacleSpawner.obstacles.Count - 1; i >= 0; i--)
        {
            Obstacle obstacle = obstacleSpawner.obstacles[i];
            Transform child = obstacle.objectTransform;
            if (child.position.z <= destroyPosition)
            {
                obstacleSpawner.obstacles.RemoveAt(i);
                Destroy(child.gameObject);
                continue;
            }
            else if (!obstacle.hasDealtDamage && child.position.z <= damagePosition)
            {
                DealDamageToPlayer(obstacle);
                obstacle.hasDealtDamage = true;
            }

            child.position += Vector3.back * movement;
        }
    }

    private void DealDamageToPlayer(Obstacle obstacle)
    {
        if (joustPlayer.CurrentLaneIndex == obstacle.assignedLane)
            Debug.Log("Player was hit !");
    }
}
