using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool canRun;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        string animation = ComputeCurrentAnimation();
        string direction = ComputeDirectionFromLook(playerMovement.LastLookDirection);

        animator.Play($"{animation}_{direction}");
    }

    private string ComputeCurrentAnimation()
    {
        if (canRun && playerMovement.isRunning)
            return "Run";
        if (playerMovement.isMoving)
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
