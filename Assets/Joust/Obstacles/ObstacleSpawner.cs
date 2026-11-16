using System.Collections.Generic;
using System.Linq;
using BoomTools;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDistance;
    [SerializeField] private float distanceBetweenObstacleFactor;

    private JoustDifficultyData joustDifficultyData;

    private Vector3 centerPosition => Vector3.zero + Vector3.up * spawnDistance;

    [HideInInspector] public List<Obstacle> obstacles = new List<Obstacle>();

    private Chunk currentChunk;
    private int chunkSpawnedCount;
    private float currentDistanceInChunk;
    private float scaledChunkDistance;
    private float currentScaleBonus;
    private bool hasReachedEnd;
    private float endDistance;
    private bool isSpawningOver;

    public void Setup(JoustDifficultyData data)
    {
        joustDifficultyData = data;
        SpawnFirstChunk();
    }

    private void Update()
    {
        currentDistanceInChunk += Odometer.instance.CurrentMovement;

        if (isSpawningOver)
            return;

        if (hasReachedEnd)
        {
            if (currentDistanceInChunk >= endDistance + spawnDistance)
            {
                SpawnKnight();
                isSpawningOver = true;
            }
            return;
        }

        if (currentChunk == null || currentDistanceInChunk >= scaledChunkDistance)
        {
            if (currentDistanceInChunk >= joustDifficultyData.totalDistance)
            {
                hasReachedEnd = true;
                endDistance = currentDistanceInChunk;
                return;
            }
            else
            {
                SelectNewChunk();
            }
        }

        if (!currentChunk.IsEmpty)
            CheckChunkForSpawn();
    }

    private void SpawnKnight()
    {
        Obstacle enemy = SpawnObstacle(new ObstacleInCHunk(ObstacleType.Knight, currentDistanceInChunk, Random.Range(0, joustDifficultyData.laneCount)));
        enemy.objectTransform.GetComponent<JoustEnemyMovement>().Setup(joustDifficultyData.EnemyData, enemy.assignedLane, joustDifficultyData.laneCount);
    }

    private void SpawnFirstChunk()
    {
    }

    private void SelectNewChunk()
    {
        bool isLateGame = chunkSpawnedCount > 3;

        if (isLateGame)
            currentChunk = new Chunk(joustDifficultyData.lateChunks.GetRandomElements().First());
        else
            currentChunk = new Chunk(joustDifficultyData.earlyChunks.GetRandomElements().First());

        chunkSpawnedCount += 1;
        currentScaleBonus = Mathf.Max(1, Odometer.instance.CurrentVelocity / currentChunk.ChunkBaseSpeed) * distanceBetweenObstacleFactor;
        scaledChunkDistance = (currentChunk.ChunkDistance + 1) * currentScaleBonus;
        currentDistanceInChunk = 0.0f;
    }

    private void CheckChunkForSpawn()
    {
        for (int i = currentChunk.ObstacleInChunks.Count - 1; i >= 0; i--)
        {
            ObstacleInCHunk obstacle = currentChunk.ObstacleInChunks[i];

            float relativeDistance = obstacle.distance * currentScaleBonus;

            if (relativeDistance <= currentDistanceInChunk)
            {
                SpawnObstacle(obstacle);
                currentChunk.RemoveAt(i);
            }
        }
    }

    private Obstacle SpawnObstacle(ObstacleInCHunk obstacle)
    {
        switch (obstacle.obstacleType)
        {
            default:
            case ObstacleType.Short:
                return CreateObstacle(obstacle, joustDifficultyData.ShortObstacles.GetRandomElements().First(), obstacle.laneIndex);
            case ObstacleType.Tall:
                return CreateObstacle(obstacle, joustDifficultyData.TallObstacles.GetRandomElements().First(), obstacle.laneIndex);
            case ObstacleType.TallX2:
                CreateObstacle(obstacle, joustDifficultyData.Tall_X2_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 1);
                return CreateObstacle(obstacle, joustDifficultyData.Tall_X2_Obstacles.GetRandomElements().First(), obstacle.laneIndex);
            case ObstacleType.ShortX3:
                CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 1);
                CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 2);
                return CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex);
            case ObstacleType.Knight:
                return CreateObstacle(obstacle, joustDifficultyData.EnemyData.knightPrefab, obstacle.laneIndex);
        }
    }

    private Obstacle CreateObstacle(ObstacleInCHunk obstacle, GameObject prefab, int laneIndex)
    {
        GameObject item = Instantiate(prefab, ComputePosition(obstacle, laneIndex), Quaternion.identity, transform);
        Obstacle newObstacle = new Obstacle(item.transform, laneIndex, obstacle);
        obstacles.Add(newObstacle);

        return newObstacle;
    }

    private Vector3 ComputePosition(ObstacleInCHunk obstacle, int laneIndex)
    {
        int centerIndex = joustDifficultyData.laneCount / 2;
        int indexDelta = laneIndex - centerIndex;
        float distanceLaneToLane = 1.9f;
        float distanceDelta = indexDelta * distanceLaneToLane;
        Vector3 position = obstacle.obstacleType == ObstacleType.Knight ? centerPosition + Vector3.up * spawnDistance : centerPosition;
        return position + Vector3.right * distanceDelta;
    }
}
