using System;
using PrimeTween;
using UnityEngine;

public class JoustMoveState : IJoustPlayerState
{
    public void StartBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType previous)
    {
    }

    public void UpdateBehaviour(JoustPlayerStateMachine player)
    {
        PerformSideMovement(player);
        PerformForwardMovement(player);

        if (player.inputPackage.GetJump.WasPressedWithBuffer())
        {
            player.ChangeBehaviour(player.joustJumpState);
            return;
        }
    }

    public void PerformForwardMovement(JoustPlayerStateMachine player)
    {
        player.currentSpeedTime += Time.deltaTime;

        player.velocity = ComputeVelocityFromTime(player, player.currentSpeedTime);
    }

    public float ComputeVelocityFromTime(JoustPlayerStateMachine player, float time)
    {
        float normalizedTime = Mathf.Clamp01(time / player.data.timeToMaxSpeed);
        float speedMultiplier = player.data.accelerationCurve.Evaluate(normalizedTime);
        return player.data.maxSpeed * speedMultiplier;
    }

    public void PerformSideMovement(JoustPlayerStateMachine player)
    {
        float move = player.inputPackage.GetMove.x;

        if (Mathf.Abs(move) >= 0.05f)
        {
            float moveDirection = Mathf.Sign(move);
            if (moveDirection != player.lastMovement)
            {
                int targetIndex = Math.Clamp(player.currentIndex + (int)moveDirection, 0, player.LaneCount - 1);

                if (targetIndex != player.currentIndex)
                {
                    player.lastMovement = moveDirection;
                    player.currentIndex = targetIndex;
                    MoveTo(player, ComputePositionFromIndex(player));
                }
            }
        }
        else
        {
            player.lastMovement = 0.0f;
        }
    }

    private void MoveTo(JoustPlayerStateMachine player, Vector3 targetPosition)
    {
        if (player.moveSequence.isAlive)
            player.moveSequence.Stop();

        player.moveSequence = Sequence.Create()
        .Chain(Tween.Position(player.transform, targetPosition, player.data.timeToChangeLane, player.data.laneChangeEase));
    }

    private Vector3 ComputePositionFromIndex(JoustPlayerStateMachine player)
    {
        int indexDelta = player.currentIndex - player.centerIndex;
        float distanceDelta = indexDelta * player.data.distanceLaneToLane;
        return player.CenterPosition + Vector3.right * distanceDelta;
    }

    public void StopBehaviour(JoustPlayerStateMachine player, IJoustPlayerState.StateType next)
    {
    }

    public IJoustPlayerState.StateType GetStateType()
    {
        return IJoustPlayerState.StateType.Moving;
    }
}
