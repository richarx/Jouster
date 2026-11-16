using BoomTools;
using UnityEngine;

[CreateAssetMenu(fileName = "SmartEnemyStrategy", menuName = "Scriptable Objects/Strategy/SmartEnemyStrategy")]
public class SmartEnemyStrategy : Strategy
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
        bool isOnSameLineAsPlayer = enemyMovement.CurrentLane == joustPlayerStateMachine.CurrentLaneIndex;
        bool isInDanger = joustPlayerStateMachine.playerPower.HasEnoughPower;
        bool isOnValidLine = isInDanger ? !isOnSameLineAsPlayer : isOnSameLineAsPlayer;

        if (isOnValidLine)
            return;

        if (enemyMovement.IsMoving)
            return;

        if (reactionTimestamp < 0.0f)
        {
            reactionTimestamp = Time.time;
        }

        if (Time.time - reactionTimestamp >= ComputeReactionTime(enemyMovement))
        {
            if (Time.time - lastMovementTimestamp > ComputeWaitTime(enemyMovement))
            {
                float target = isInDanger ? ComputeMoveDirection(enemyMovement) : joustPlayerStateMachine.CurrentLaneIndex - enemyMovement.CurrentLane;
                enemyMovement.PerformSideMovement(target);
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
