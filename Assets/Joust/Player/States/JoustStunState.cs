using System;
using BoomTools;
using UnityEngine;

public class JoustStunState : IJoustPlayerState
{
    private float startStunTimestamp;
    private float targetVelocity;

    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
        startStunTimestamp = Time.time;
        player.currentSpeedTime = player.currentSpeedTime * (1.0f - Mathf.Clamp01(player.data.knockbackPower));
        targetVelocity = player.joustMoveState.ComputeVelocityFromTime(player, player.currentSpeedTime);
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
        player.joustMoveState.PerformSideMovement(player);

        if (Time.time - startStunTimestamp >= player.data.knockbackDuration)
        {
            player.ChangeBehaviour(player.joustMoveState);
            return;
        }

        ComputeKnockBack(player);
    }

    private void ComputeKnockBack(JoustPlayerStateMachine player)
    {
        float knockbackVelocity = 0.0f;

        float timeSinceStartStun = Time.time - startStunTimestamp;
        player.velocity = Tools.NormalizeValueInRange(timeSinceStartStun, 0.0f, player.data.knockbackDuration, knockbackVelocity, targetVelocity);
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Stun;
    }
}
