using UnityEngine;

public class JoustJumpState : IJoustPlayerState
{
    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Jump;
    }
}
