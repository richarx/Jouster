using System;
using BoomTools;
using LDtkUnity;
using Map;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;
    private LDtkComponentEntity componentEntity;
    private Map.Cities targetCity;
    private Map.Target targetEnvironment;
    private Vector3 spawnPosition;

    public Cities TargetCity => targetCity;
    public Vector3 SpawnPosition => spawnPosition;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        componentEntity = GetComponent<LDtkComponentEntity>();
        LDtkFields lDtkFields = GetComponent<LDtkFields>();

        targetCity = lDtkFields.GetEnum<Cities>(nameof(Cities));
        targetEnvironment = lDtkFields.GetEnum<Target>(nameof(Target));

        spawnPosition = transform.GetChild(0).position;

        SetupCollider();
        DoorsHolder.instance.RegisterNewDoor(this);
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
            SceneSwapper.instance.LoadScene(targetCity, targetEnvironment);
        }
    }
}
