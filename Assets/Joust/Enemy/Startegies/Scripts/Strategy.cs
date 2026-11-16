using UnityEngine;

public abstract class Strategy : ScriptableObject
{
    public abstract void SetupBehaviour(JoustEnemyMovement enemyMovement);
    public abstract void PerformBehaviour(JoustEnemyMovement enemyMovement);
}
