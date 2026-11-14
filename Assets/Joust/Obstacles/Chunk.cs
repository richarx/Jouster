using System.Collections.Generic;
using System.Collections.ObjectModel;
using LDtkUnity;
using UnityEngine;

public class ObstacleInCHunk
{
    public ObstacleType obstacleType;
    public float distance;
    public int laneIndex;

    public ObstacleInCHunk(ObstacleType obstacle, float distanceFromStart, int lane)
    {
        obstacleType = obstacle;
        distance = distanceFromStart;
        laneIndex = lane;
    }
}

public class Chunk
{
    private List<ObstacleInCHunk> obstacleInChunks;
    private float chunkDistance;
    private float chunkBaseSpeed;

    public ReadOnlyCollection<ObstacleInCHunk> ObstacleInChunks => obstacleInChunks.AsReadOnly();
    public bool IsEmpty => obstacleInChunks.Count < 1;
    public float ChunkDistance => chunkDistance;
    public float ChunkBaseSpeed => chunkBaseSpeed;

    public Chunk(GameObject chunkLevel)
    {
        obstacleInChunks = new List<ObstacleInCHunk>();

        chunkDistance = chunkLevel.GetComponent<LDtkComponentLevel>().Size.y;
        chunkBaseSpeed = chunkLevel.GetComponent<LDtkFields>().GetFloat("BaseSpeed");

        Transform obstacleHolder = chunkLevel.transform.GetChild(0);

        for (int i = 0; i < obstacleHolder.childCount; i++)
        {
            Transform obstacle = obstacleHolder.GetChild(i);

            ObstacleType obstacleType = obstacle.GetComponent<LDtkFields>().GetEnum<ObstacleType>(nameof(ObstacleType));
            float distanceFromStart = obstacle.localPosition.y;
            int laneIndex = Mathf.RoundToInt(obstacle.localPosition.x);

            obstacleInChunks.Add(new ObstacleInCHunk(obstacleType, distanceFromStart, laneIndex));
        }
    }

    public void RemoveAt(int index)
    {
        obstacleInChunks.RemoveAt(index);
    }
}
