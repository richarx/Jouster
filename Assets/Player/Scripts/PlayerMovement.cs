using BoomLib.Inputs;
using BoomTools;
using PrimeTween;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float normalizedRunThreshold;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundDeceleration;

    public static PlayerMovement instance;

    private Rigidbody2D rb;
    private InputPacker imputPacker;
    private InputPackage currentInputs;
    public InputPackage CurrentInputs => currentInputs;

    private Vector2 moveVelocity;
    private int lastLookDirection;
    public int LastLookDirection => lastLookDirection;
    public bool isRunning => Tools.NormalizeValue(moveVelocity.magnitude, 0.0f, moveSpeed) >= normalizedRunThreshold;
    private bool isMoving;
    public bool IsMoving => isMoving;

    private bool isLocked;
    public bool IsLocked => isLocked;

    private Sequence lockedMovementSequence;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        imputPacker = new InputPacker();
        rb = GetComponent<Rigidbody2D>();

        if (JoustPreparation.instance != null)
            JoustPreparation.instance.OnGoToStartJoust.AddListener(() =>
            {
                if (lockedMovementSequence.isAlive)
                    lockedMovementSequence.Stop();
            });
    }

    private void Update()
    {
        currentInputs = imputPacker.ComputeInputPackage();
    }

    private void FixedUpdate()
    {
        if (currentInputs == null || isLocked)
            return;

        Vector2 moveInput = currentInputs.GetMove;
        Vector2 move = moveInput.normalized * moveSpeed;

        isMoving = moveInput.magnitude > 0.15f;

        if (isMoving)
        {
            moveVelocity.x = Mathf.MoveTowards(moveVelocity.x, move.x, groundAcceleration * Time.fixedDeltaTime);
            moveVelocity.y = Mathf.MoveTowards(moveVelocity.y, move.y, groundAcceleration * Time.fixedDeltaTime);
        }
        else
        {
            moveVelocity.x = Mathf.MoveTowards(moveVelocity.x, 0.0f, groundDeceleration * Time.fixedDeltaTime);
            moveVelocity.y = Mathf.MoveTowards(moveVelocity.y, 0.0f, groundDeceleration * Time.fixedDeltaTime);
        }

        rb.linearVelocity = moveVelocity;

        if (isMoving)
            lastLookDirection = Tools.GetCardinalDirection(moveInput);
    }

    public void TeleportPlayer(Vector2 spawnPosition)
    {
        rb.position = spawnPosition;
        transform.position = spawnPosition.ToVector3();
    }

    public void SetLockState(bool state, Vector3 moveToPosition, Vector2 idleLookDirection, float moveDuration = 1.5f)
    {
        SetLockState(state);

        if (lockedMovementSequence.isAlive)
            lockedMovementSequence.Stop();

        Vector3 lookDirection = (moveToPosition - transform.position).normalized;
        lastLookDirection = Tools.GetCardinalDirection(lookDirection);
        rb.linearVelocity = Vector2.zero;

        lockedMovementSequence = Sequence.Create(updateType: UpdateType.FixedUpdate)
            .Chain(Tween.RigidbodyMovePosition(rb, moveToPosition, moveDuration))
            .ChainCallback(() => lastLookDirection = Tools.GetCardinalDirection(idleLookDirection));
    }

    public void SetLockState(bool state, Vector2 idleLookDirection)
    {
        lastLookDirection = Tools.GetCardinalDirection(idleLookDirection);
        SetLockState(state);
    }

    public void SetLockState(bool state)
    {
        if (!state && lockedMovementSequence.isAlive)
            lockedMovementSequence.Stop();

        isLocked = state;
    }
}
