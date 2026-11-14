using UnityEngine;

public class GroundManager : MonoBehaviour
{
    [SerializeField] private Transform roadTransform;
    [SerializeField] private Transform grassTransform;
    [SerializeField] private Transform damageLineTransform;
    [SerializeField] private Transform destroyLineTransform;
    [SerializeField] private Material roadMaterial;

    public void Setup(JoustDifficultyData data)
    {
        roadMaterial.SetFloat("_TilingX", data.laneCount);

        Vector3 roadScale = roadTransform.localScale;
        roadScale.x = data.laneCount * 2;
        roadTransform.localScale = roadScale;

        float largeScale = (data.laneCount * 2) + 2;

        Vector3 grassScale = grassTransform.localScale;
        grassScale.x = largeScale;
        grassTransform.localScale = grassScale;

        Vector3 damageLineScale = damageLineTransform.localScale;
        damageLineScale.x = largeScale;
        damageLineTransform.localScale = damageLineScale;

        Vector3 destroyLineScale = destroyLineTransform.localScale;
        destroyLineScale.x = largeScale;
        destroyLineTransform.localScale = destroyLineScale;
    }
}
