using UnityEngine;
using UnityEngine.Events;

public class JoustPlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingHP;
    [SerializeField] private int startingArmor;
    [SerializeField] private float invincibilityDuration;

    [HideInInspector] public UnityEvent OnTakeArmorDamage = new UnityEvent();
    [HideInInspector] public UnityEvent OnTakeDamage = new UnityEvent();
    [HideInInspector] public UnityEvent OnDie = new UnityEvent();

    private JoustPlayerStateMachine joustPlayer;

    private int currentHealth;
    private int currentArmor;
    public int CurrentHealth => currentHealth;
    public int CurrentArmor => currentArmor;

    public bool IsDead => currentHealth <= 0;
    public bool IsArmored => currentArmor > 0;
    public bool IsInvincible => Time.time - lastDamageTimestamp <= invincibilityDuration;

    private float lastDamageTimestamp = 0.0f;

    private void Awake()
    {
        currentHealth = startingHP;
        currentArmor = startingArmor;
    }

    private void Start()
    {
        joustPlayer = GetComponent<JoustPlayerStateMachine>();
    }

    public bool CanTakeDamage(Obstacle obstacle, int lane)
    {
        if (IsInvincible)
            return false;

        if (joustPlayer.CurrentLaneIndex == lane)
        {
            if (joustPlayer.currentState.GetStateType() == IJoustPlayerState.StateType.Jump)
            {
                bool canJumpAbove = joustPlayer.joustJumpState.CanJumpAbove(obstacle);
                return !canJumpAbove;
            }

            return true;
        }

        return false;
    }

    public void TakeDamage()
    {
        if (IsArmored)
        {
            currentArmor -= 1;
            OnTakeArmorDamage?.Invoke();
        }
        else
        {
            currentHealth -= 1;

            if (IsDead)
                OnDie?.Invoke();
            else
                OnTakeDamage?.Invoke();
        }

        lastDamageTimestamp = Time.time;
    }
}
