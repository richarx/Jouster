using System.Collections.Generic;
using System.Linq;
using BoomTools;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private JoustDifficultyData joustDifficultyData;

    [SerializeField] private float spawnDistance;

    private JoustPlayerMovement playerMovement;

    private float lastSpawnTimestamp = -1.0f;

    [HideInInspector] public List<Obstacle> obstacles = new List<Obstacle>();

    private Vector3 centerPosition => Vector3.zero + Vector3.forward * spawnDistance;

    private void Start()
    {
        playerMovement = JoustPlayerMovement.instance;
        playerMovement.OnStartRun.AddListener(() => lastSpawnTimestamp = Time.time);
    }

    private void LateUpdate()
    {
        if (lastSpawnTimestamp < 0.0f)
            return;

        if (Time.time - lastSpawnTimestamp >= joustDifficultyData.minTimeBetweenSpawn)
        {
            SpawnRandomObstacle();
            lastSpawnTimestamp = Time.time;
        }
    }

    private void SpawnRandomObstacle()
    {
        GameObject prefab = joustDifficultyData.ObstaclesPrefab.GetRandomElements<GameObject>().First();
        int laneIndex = ComputeLane();
        GameObject item = Instantiate(prefab, ComputePosition(laneIndex), Quaternion.identity, transform);

        obstacles.Add(new Obstacle(item.transform, laneIndex));
    }

    private Vector3 ComputePosition(int laneIndex)
    {
        int centerIndex = 1;
        int indexDelta = laneIndex - centerIndex;
        float distanceLaneToLane = 1.9f;
        float distanceDelta = indexDelta * distanceLaneToLane;
        return centerPosition + Vector3.right * distanceDelta;
    }

    private int ComputeLane()
    {
        return Random.Range(0, 3);
    }
}
