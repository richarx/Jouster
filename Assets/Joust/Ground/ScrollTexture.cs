using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Vector2 scrollSpeed;

    private Vector2 currentOffset;

    private void LateUpdate()
    {
        currentOffset += scrollSpeed * Odometer.instance.CurrentMovement / 2.0f;

        currentOffset.x %= 1f;
        currentOffset.y %= 1f;

        targetMaterial.SetFloat("_ScrollX", currentOffset.x);
        targetMaterial.SetFloat("_ScrollY", currentOffset.y);
    }
}
