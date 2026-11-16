using UnityEngine;

[CreateAssetMenu(fileName = "AttackEnemyStrategy", menuName = "Scriptable Objects/Strategy/AttackEnemyStrategy")]
public class AttackEnemyStrategy : Strategy
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

        if (isOnSameLineAsPlayer)
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
                enemyMovement.PerformSideMovement(joustPlayerStateMachine.CurrentLaneIndex - enemyMovement.CurrentLane);
                lastMovementTimestamp = Time.time;
                reactionTimestamp = -1.0f;
            }
        }
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
