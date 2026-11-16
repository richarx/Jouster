using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RushEnemyStrategy", menuName = "Scriptable Objects/Strategy/RushEnemyStrategy")]
public class RushEnemyStrategy : Strategy
{
    private float lastMovementTimestamp = -1.0f;

    public override void SetupBehaviour(JoustEnemyMovement enemyMovement)
    {
        lastMovementTimestamp = Time.time;
    }

    public override void PerformBehaviour(JoustEnemyMovement enemyMovement)
    {
        bool isOnCenterLine = enemyMovement.CurrentLane == enemyMovement.CenterIndex;

        if (isOnCenterLine)
            return;

        if (!enemyMovement.IsMoving && Time.time - lastMovementTimestamp > ComputeWaitTime(enemyMovement))
        {
            enemyMovement.PerformSideMovement(enemyMovement.CenterIndex - enemyMovement.CurrentLane);
            lastMovementTimestamp = Time.time;
        }
    }

    public float ComputeWaitTime(JoustEnemyMovement enemyMovement)
    {
        return enemyMovement.ComputeTimeToChangeLane(enemyMovement.Data.weightClass);
    }
}
