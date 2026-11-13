using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    public static SpawnPosition instance;

    public Vector3 Position => transform.position;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("Error in [SpawnPosition] : a SpawnPosition already exist");
        else
            instance = this;
    }
}
