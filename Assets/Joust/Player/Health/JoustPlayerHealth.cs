using UnityEngine;
using UnityEngine.Events;

public class JoustPlayerHealth : MonoBehaviour
{
    [SerializeField] private int startingHP;
    [SerializeField] private int startingArmor;

    [HideInInspector] public UnityEvent OnTakeArmorDamage = new UnityEvent();
    [HideInInspector] public UnityEvent OnTakeDamage = new UnityEvent();
    [HideInInspector] public UnityEvent OnDie = new UnityEvent();

    private int currentHealth;
    private int currentArmor;
    public int CurrentHealth => currentHealth;
    public int CurrentArmor => currentArmor;

    public bool IsDead => currentHealth <= 0;
    public bool IsArmored => currentArmor > 0;

    private void Awake()
    {
        currentHealth = startingHP;
        currentArmor = startingArmor;
    }

    public void TakeDamage()
    {
        Debug.Log("Take Damage");

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
    }
}
