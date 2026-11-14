using System;
using BoomLib.Inputs;
using BoomTools;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class JoustPlayerMovement : MonoBehaviour
{
    [SerializeField] private float timeToChangeLane;
    [SerializeField] private Ease laneChangeEase;

    [Space]
    [SerializeField] private float distanceLaneToLane;

    [Space]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float timeToReachMaxSpeed;
    [SerializeField] private AnimationCurve accelerationCurve;

    [HideInInspector] public UnityEvent OnStartRun = new UnityEvent();

    public static JoustPlayerMovement instance;

    private InputPacker inputPacker = new InputPacker();
    private InputPackage inputPackage = new InputPackage();

    private int currentIndex = 0;
    private int laneCount = 3;
    public int CurrentLaneIndex => currentIndex;

    private int centerIndex => laneCount / 2;
    private Vector3 centerPosition;

    private Sequence moveSequence;

    private float lastMovement;

    private float velocity;

    private float startMovingTimestamp = -1.0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        centerPosition = transform.position;
        currentIndex = centerIndex;
    }

    private void Update()
    {
        inputPackage = inputPacker.ComputeInputPackage();

        if (startMovingTimestamp < 0.0f && lastMovement != 0)
            StartMoving();

        PerformForwardMovement();
        PerformSideMovement();
        Odometer.instance.SetPlayerSpeed(velocity);
    }

    private void PerformSideMovement()
    {
        float move = inputPackage.GetMove.x;

        if (Mathf.Abs(move) >= 0.05f)
        {
            float moveDirection = Mathf.Sign(move);
            if (moveDirection != lastMovement)
            {
                int targetIndex = Math.Clamp(currentIndex + (int)moveDirection, 0, laneCount - 1);

                if (targetIndex != currentIndex)
                {
                    lastMovement = moveDirection;
                    currentIndex = targetIndex;
                    MoveTo(ComputePositionFromIndex(currentIndex));
                }
            }
        }
        else
        {
            lastMovement = 0.0f;
        }
    }

    private void MoveTo(Vector3 targetPosition)
    {
        if (moveSequence.isAlive)
            moveSequence.Complete();

        moveSequence = Sequence.Create()
        .Chain(Tween.Position(transform, targetPosition, timeToChangeLane, laneChangeEase));
    }

    private Vector3 ComputePositionFromIndex(int index)
    {
        int indexDelta = index - centerIndex;
        float distanceDelta = indexDelta * distanceLaneToLane;
        return centerPosition + Vector3.right * distanceDelta;
    }

    private void StartMoving()
    {
        startMovingTimestamp = Time.time;
        OnStartRun?.Invoke();
    }

    private void PerformForwardMovement()
    {
        if (startMovingTimestamp < 0.0f)
            return;

        float timeSinceStartMoving = Time.time - startMovingTimestamp;
        float clampedTime = Mathf.Clamp(timeSinceStartMoving, 0.0f, timeToReachMaxSpeed);
        float normalizedTime = Tools.NormalizeValue(clampedTime, 0.0f, timeToReachMaxSpeed);
        normalizedTime = Mathf.Clamp01(normalizedTime);

        velocity = accelerationCurve.Evaluate(normalizedTime) * maxSpeed;
    }
}
