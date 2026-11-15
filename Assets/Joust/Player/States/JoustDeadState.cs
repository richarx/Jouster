using System;
using UnityEngine;

public class JoustDeadState : IJoustPlayerState
{
    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
        SlowDown(player);
    }

    private void SlowDown(JoustPlayerStateMachine player)
    {
        player.velocity = Mathf.Max(0.0f, player.velocity - (player.data.deathDeceleration * Time.deltaTime));
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Dead;
    }
}
