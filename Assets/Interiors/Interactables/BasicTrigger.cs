using UnityEngine;
using UnityEngine.Events;

public class BasicTrigger : MonoBehaviour
{
    [SerializeField] private bool resizeHitboxOnSpawn;

    public UnityEvent OnPlayerEnterTrigger = new UnityEvent();
    public UnityEvent OnPlayerExitTrigger = new UnityEvent();

    protected bool isPlayerInRange;

    protected bool isActive = true;
    public bool IsActive => isActive;

    protected virtual void Start()
    {
        if (resizeHitboxOnSpawn)
            ResizeCollider();
    }

    [ContextMenu("ResizeCollider")]
    public void ResizeCollider()
    {
        Vector3 scale = transform.localScale;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(0.5f, -0.5f);
    }

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
