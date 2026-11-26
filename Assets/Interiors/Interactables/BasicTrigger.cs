using UnityEngine;
using UnityEngine.Events;

public class BasicTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnterTrigger = new UnityEvent();
    public UnityEvent OnPlayerExitTrigger = new UnityEvent();

    protected bool isPlayerInRange;

    protected bool isActive = true;
    public bool IsActive => isActive;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (isActive)
                OnPlayerEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (isActive)
                OnPlayerExitTrigger?.Invoke();
        }
    }

    public void SetActiveState(bool state)
    {
        if (state == isActive)
            return;

        isActive = state;

        if (isActive && isPlayerInRange)
            OnPlayerEnterTrigger?.Invoke();
    }
}
