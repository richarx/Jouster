using UnityEngine;

public interface IJoustPlayerState
{
    public enum StateType
    {
        Idle,
        Moving,
        Jump,
        Stun,
        Dead
    }

    public void StartBehaviour(JoustPlayerStateMachine player, StateType previous);
    public void UpdateBehaviour(JoustPlayerStateMachine player);
    public void StopBehaviour(JoustPlayerStateMachine player, StateType next);
    public StateType GetStateType();
}
