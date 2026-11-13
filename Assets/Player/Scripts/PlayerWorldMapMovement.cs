using BoomLib.Inputs;
using BoomTools;
using UnityEngine;

public class PlayerWorldMapMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float normalizedRunThreshold;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundDeceleration;

    private Rigidbody2D rb;
    private InputPacker imputPacker;
    private InputPackage currentInputs;

    private Vector2 moveVelocity;
    private int lastLookDirection;
    public int LastLookDirection => lastLookDirection;
    public bool isMoving => currentInputs.GetMove.magnitude > 0.05f;
    public bool isRunning => Tools.NormalizeValue(moveVelocity.magnitude, 0.0f, moveSpeed) >= normalizedRunThreshold;

    private void Start()
    {
        imputPacker = new InputPacker();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        currentInputs = imputPacker.ComputeInputPackage();
    }

    private void FixedUpdate()
    {
        if (currentInputs == null)
            return;

        Vector2 moveInput = currentInputs.GetMove;
        Vector2 move = moveInput.normalized * moveSpeed;

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
}
