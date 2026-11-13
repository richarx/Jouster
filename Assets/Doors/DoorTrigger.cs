using System;
using LDtkUnity;
using Map;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private LDtkComponentEntity componentEntity;
    private Map.Cities targetCity;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        componentEntity = GetComponent<LDtkComponentEntity>();
        LDtkFields lDtkFields = GetComponent<LDtkFields>();

        targetCity = lDtkFields.GetEnum<Cities>(nameof(Cities));

        SetupCollider();
    }

    private void SetupCollider()
    {
        Vector2 size = componentEntity.Size;

        boxCollider2D.enabled = true;
        boxCollider2D.size = size;
        boxCollider2D.offset = new Vector2(size.x / 2.0f, -size.y / 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Trigger : {targetCity}");
        }
    }
}
