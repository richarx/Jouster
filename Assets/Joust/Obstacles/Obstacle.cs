using System;
using UnityEngine;

[Serializable]
public class Obstacle
{
    public Transform objectTransform;
    public int assignedLane;
    public bool hasDealtDamage;

    public Obstacle(Transform transform, int lane)
    {
        objectTransform = transform;
        assignedLane = lane;
        hasDealtDamage = false;
    }
}
