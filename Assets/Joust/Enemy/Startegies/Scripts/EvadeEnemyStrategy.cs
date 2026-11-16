using System;
using BoomTools;
using UnityEngine;

[CreateAssetMenu(fileName = "EvadeEnemyStrategy", menuName = "Scriptable Objects/Strategy/EvadeEnemyStrategy")]
public class EvadeEnemyStrategy : Strategy
{
    private JoustPlayerStateMachine joustPlayerStateMachine;
    private float lastMovementTimestamp = -1.0f;
    private float reactionTimestamp = -1.0f;

    public override void SetupBehaviour(JoustEnemyMovement enemyMovement)
    {
        lastMovementTimestamp = Time.time;
        reactionTimestamp = -1.0f;
        joustPlayerStateMachine = JoustPlayerStateMachine.instance;
    }

    public override void PerformBehaviour(JoustEnemyMovement enemyMovement)
    {
        bool isOnDifferentLineAsPlayer = enemyMovement.CurrentLane != joustPlayerStateMachine.CurrentLaneIndex;

        if (isOnDifferentLineAsPlayer)
            return;

        if (enemyMovement.IsMoving)
            return;

        if (reactionTimestamp < 0.0f)
        {
            reactionTimestamp = Time.time;
            Debug.Log("Start Reaction");
        }

        if (Time.time - reactionTimestamp >= ComputeReactionTime(enemyMovement))
        {
            Debug.Log($"Reaction Time : {ComputeReactionTime(enemyMovement)}");
            if (Time.time - lastMovementTimestamp > ComputeWaitTime(enemyMovement))
            {
                enemyMovement.PerformSideMovement(ComputeMoveDirection(enemyMovement));
                lastMovementTimestamp = Time.time;
                reactionTimestamp = -1.0f;
            }
        }
    }

    private float ComputeMoveDirection(JoustEnemyMovement enemyMovement)
    {
        if (enemyMovement.CurrentLane == 0)
            return 1.0f;
        if (enemyMovement.CurrentLane == enemyMovement.LaneCount - 1)
            return -1.0f;
        return Tools.RandomBool() ? -1.0f : 1.0f;
    }

    private float ComputeReactionTime(JoustEnemyMovement enemyMovement)
    {
        return enemyMovement.ComputeReactionTime(enemyMovement.Data.reactionTime);
    }

    private float ComputeWaitTime(JoustEnemyMovement enemyMovement)
    {
        return enemyMovement.ComputeTimeToChangeLane(enemyMovement.Data.weightClass);
    }
}
