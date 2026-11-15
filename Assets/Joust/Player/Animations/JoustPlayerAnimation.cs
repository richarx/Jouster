using System;
using UnityEngine;

public class JoustPlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void LateUpdate()
    {
        animator.Play(ComputeAnimation(Odometer.instance.NormalizedSpeed));
    }

    private string ComputeAnimation(float normalizedSpeed)
    {
        if (normalizedSpeed <= 0.0f)
            return "Idle";
        if (normalizedSpeed <= 0.5f)
            return "Walk";
        if (normalizedSpeed <= 0.8f)
            return "WalkFast";

        return "Run";
    }
}
