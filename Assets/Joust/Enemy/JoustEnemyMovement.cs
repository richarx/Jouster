using System;
using System.Collections;
using BoomTools;
using Map;
using PrimeTween;
using UnityEngine;
using static JoustEnemyData;

public class JoustEnemyMovement : MonoBehaviour
{
    private JoustEnemyData data;
    public JoustEnemyData Data => data;

    private int currentLane;
    private int laneCount;

    private bool isSetup;

    private bool isMoving;
    public bool IsMoving => isMoving;

    private float distanceLaneToLane = 1.9f;

    public int CurrentLane => currentLane;
    public int CenterIndex => laneCount / 2;
    public int LaneCount => laneCount;

    public void Setup(JoustEnemyData data, int assignedLane, int laneCount)
    {
        this.data = data;
        this.laneCount = laneCount;
        this.currentLane = assignedLane;
        data.strategy.SetupBehaviour(this);
        isSetup = true;
    }

    private void Update()
    {
        if (!isSetup)
            return;

        data.strategy.PerformBehaviour(this);
    }

    public void PerformSideMovement(float move)
    {
        if (Mathf.Abs(move) >= 0.05f)
        {
            float moveDirection = Mathf.Sign(move);
            int targetIndex = Math.Clamp(currentLane + (int)moveDirection, 0, laneCount - 1);

            if (targetIndex != currentLane)
            {
                StopAllCoroutines();
                StartCoroutine(MoveTo(targetIndex, ComputeDistanceFromIndex(targetIndex)));
            }
        }
    }

    private IEnumerator MoveTo(int targetIndex, float targetDistance)
    {
        isMoving = true;

        float duration = ComputeTimeToChangeLane(data.weightClass);
        float timer = 0.0f;

        float startingPosition = transform.position.x;

        while (timer <= duration)
        {
            float positionX = Tools.NormalizeValueInRange(timer, 0.0f, duration, startingPosition, targetDistance);
            Vector3 position = transform.localPosition;
            position.x = positionX;
            transform.localPosition = position;

            if (currentLane != targetIndex && timer >= duration * 0.7f)
                currentLane = targetIndex;

            yield return null;
            timer += Time.deltaTime;
        }

        isMoving = false;
    }

    private float ComputeDistanceFromIndex(int targetIndex)
    {
        int indexDelta = targetIndex - CenterIndex;
        float distanceDelta = indexDelta * distanceLaneToLane;
        return distanceDelta;
    }

    public float ComputeTimeToChangeLane(WeightClass weightClass)
    {
        switch (weightClass)
        {
            case WeightClass.Light:
                return 0.5f;
            default:
            case WeightClass.Medium:
                return 0.8f;
            case WeightClass.Heavy:
                return 1.0f;
        }
    }

    private Ease ComputeLaneChangingEase(WeightClass weightClass)
    {
        switch (weightClass)
        {
            case WeightClass.Light:
                return Ease.InBack;
            default:
            case WeightClass.Medium:
                return Ease.OutBack;
            case WeightClass.Heavy:
                return Ease.OutBounce;
        }
    }

    internal float ComputeReactionTime(ReactionTime reactionTime)
    {
        switch (reactionTime)
        {
            case ReactionTime.Quick:
                return 0.2f;
            default:
            case ReactionTime.Medium:
                return 0.55f;
            case ReactionTime.Slow:
                return 0.9f;
        }
    }
}
