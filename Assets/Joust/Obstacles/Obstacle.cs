using System;
using UnityEngine;

[Serializable]
public class Obstacle
{
    public Transform objectTransform;
    public int assignedLane;
    public bool hasDealtDamage;
    public ObstacleInCHunk chunkData;

    public Obstacle(Transform objectTransform, int assignedLane, ObstacleInCHunk chunkData)
    {
        this.objectTransform = objectTransform;
        this.assignedLane = assignedLane;
        this.chunkData = chunkData;
        hasDealtDamage = false;
    }
}
