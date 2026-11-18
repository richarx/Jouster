using UnityEngine;
using UnityEngine.Events;

public class BasicTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnterTrigger = new UnityEvent();
    public UnityEvent OnPlayerExitTrigger = new UnityEvent();

    protected bool isPlayerInRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            OnPlayerEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            OnPlayerExitTrigger?.Invoke();
        }
    }
}
