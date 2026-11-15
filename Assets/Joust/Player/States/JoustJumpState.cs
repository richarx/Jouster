using System;
using BoomTools;
using UnityEngine;

public class JoustJumpState : IJoustPlayerState
{
    private float startJumpTimestamp;

    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
        startJumpTimestamp = Time.time;
    }

    public bool CanJumpAbove(Obstacle obstacle)
    {
        ObstacleType obstacleType = obstacle.chunkData.obstacleType;

        if (obstacleType == ObstacleType.Short || obstacleType == ObstacleType.ShortX3)
            return true;

        return false;
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
        float timeSinceStartStun = Time.time - startJumpTimestamp;

        if (timeSinceStartStun > player.data.jumpDuration)
        {
            player.ChangeBehaviour(player.joustMoveState);
            return;
        }

        float normalizedTime = Tools.NormalizeValue(timeSinceStartStun, 0.0f, player.data.jumpDuration);

        float jumpHeight = player.data.jumpCurve.Evaluate(normalizedTime) * player.data.jumpMaxHeight;

        SetGraphicsHeight(player, jumpHeight);
    }

    private void SetGraphicsHeight(JoustPlayerStateMachine player, float jumpHeight)
    {
        player.graphics.localPosition = Vector3.back * jumpHeight;
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
        SetGraphicsHeight(player, 0.0f);
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Jump;
    }
}
