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

    public void Setup(JoustDifficultyData data)
    {
        joustDifficultyData = data;
        SpawnFirstChunk();
    }

    private void Update()
    {
        currentDistanceInChunk += Odometer.instance.CurrentMovement;

        if (currentChunk == null || currentDistanceInChunk >= scaledChunkDistance)
            SelectNewChunk();

        if (!currentChunk.IsEmpty)
            CheckChunkForSpawn();
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

    private void SpawnObstacle(ObstacleInCHunk obstacle)
    {
        switch (obstacle.obstacleType)
        {
            case ObstacleType.Short:
                CreateObstacle(obstacle, joustDifficultyData.ShortObstacles.GetRandomElements().First(), obstacle.laneIndex);
                break;
            case ObstacleType.Tall:
                CreateObstacle(obstacle, joustDifficultyData.TallObstacles.GetRandomElements().First(), obstacle.laneIndex);
                break;
            case ObstacleType.TallX2:
                CreateObstacle(obstacle, joustDifficultyData.Tall_X2_Obstacles.GetRandomElements().First(), obstacle.laneIndex);
                CreateObstacle(obstacle, joustDifficultyData.Tall_X2_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 1);
                break;
            case ObstacleType.ShortX3:
                CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex);
                CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 1);
                CreateObstacle(obstacle, joustDifficultyData.Short_X3_Obstacles.GetRandomElements().First(), obstacle.laneIndex + 2);
                break;
        }
    }

    private void CreateObstacle(ObstacleInCHunk obstacle, GameObject prefab, int laneIndex)
    {
        GameObject item = Instantiate(prefab, ComputePosition(laneIndex), Quaternion.identity, transform);
        obstacles.Add(new Obstacle(item.transform, laneIndex, obstacle));
    }

    private Vector3 ComputePosition(int laneIndex)
    {
        int centerIndex = joustDifficultyData.laneCount / 2;
        int indexDelta = laneIndex - centerIndex;
        float distanceLaneToLane = 1.9f;
        float distanceDelta = indexDelta * distanceLaneToLane;
        return centerPosition + Vector3.right * distanceDelta;
    }
}
