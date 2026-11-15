using BoomLib.Inputs;
using PrimeTween;
using UnityEngine;

public class JoustPlayerStateMachine : MonoBehaviour
{
    public JoustPlayerData data;

    public static JoustPlayerStateMachine instance;

    [HideInInspector] public JoustPlayerHealth playerHealth;

    public JoustIdleState joustIdleState = new JoustIdleState();
    public JoustMoveState joustMoveState = new JoustMoveState();
    public JoustJumpState joustJumpState = new JoustJumpState();
    public JoustStunState joustStunState = new JoustStunState();
    public JoustDeadState joustDeadState = new JoustDeadState();

    public IJoustPlayerState currentState;

    private InputPacker inputPacker = new InputPacker();
    public InputPackage inputPackage = new InputPackage();

    [HideInInspector] public int currentIndex = 0;
    public int CurrentLaneIndex => currentIndex;

    private int laneCount = 3;
    public int LaneCount => laneCount;

    public int centerIndex => laneCount / 2;
    private Vector3 centerPosition;
    public Vector3 CenterPosition => centerPosition;

    public Sequence moveSequence;

    [HideInInspector] public float velocity;
    [HideInInspector] public float currentSpeedTime;
    [HideInInspector] public float lastMovement;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerHealth = GetComponent<JoustPlayerHealth>();
        playerHealth.OnTakeArmorDamage.AddListener(() => ChangeBehaviour(joustStunState));
        playerHealth.OnTakeDamage.AddListener(() => ChangeBehaviour(joustStunState));
        playerHealth.OnDie.AddListener(() => ChangeBehaviour(joustDeadState));

        centerPosition = transform.position;

        currentState = joustIdleState;
        joustIdleState.StartBehaviour(this, IJoustPlayerState.StateType.Idle);
    }

    public void Setup(JoustDifficultyData data)
    {
        laneCount = data.laneCount;
        currentIndex = centerIndex;
    }

    public void ChangeBehaviour(IJoustPlayerState nextState)
    {
        if (nextState == null || nextState == currentState)
            return;

        IJoustPlayerState.StateType currentType = currentState.GetStateType();

        currentState.StopBehaviour(this, nextState.GetStateType());

        currentState = nextState;
        currentState.StartBehaviour(this, currentType);
    }

    private void Update()
    {
        inputPackage = inputPacker.ComputeInputPackage();
        currentState.UpdateBehaviour(this);
        Odometer.instance.SetPlayerSpeed(velocity, data.maxSpeed);
    }
}
