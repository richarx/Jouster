using UnityEngine;
using UnityEngine.Events;

public class JoustIdleState : IJoustPlayerState
{
    [HideInInspector] public UnityEvent OnStartRun = new UnityEvent();

    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
        player.joustMoveState.PerformSideMovement(player);

        if (player.lastMovement != 0)
            player.ChangeBehaviour(player.joustMoveState);
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
        OnStartRun?.Invoke();
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Idle;
    }
}
