using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerWorldMapMovement playerWorldMapMovement;

    private void Start()
    {
        playerWorldMapMovement = GetComponent<PlayerWorldMapMovement>();
    }

    private void Update()
    {
        string animation = ComputeCurrentAnimation();
        string direction = ComputeDirectionFromLook(playerWorldMapMovement.LastLookDirection);

        animator.Play($"{animation}_{direction}");
    }

    private string ComputeCurrentAnimation()
    {
        if (playerWorldMapMovement.isRunning)
            return "Run";
        if (playerWorldMapMovement.isMoving)
            return "Walk";
        return "Idle";
    }

    private string ComputeDirectionFromLook(int lastLookDirection)
    {
        if (lastLookDirection == 0)
            return "R";
        if (lastLookDirection == 1)
            return "B";
        if (lastLookDirection == 2)
            return "L";
        return "F";
    }
}
