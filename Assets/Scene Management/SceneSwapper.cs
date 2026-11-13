using System;
using System.Collections;
using BoomTools;
using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public static SceneSwapper instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(Cities targetCity, Target targetEnvironment)
    {
        StartCoroutine(LoadSceneCoroutine(targetCity, targetEnvironment));
    }

    private IEnumerator LoadSceneCoroutine(Cities targetCity, Target targetEnvironment)
    {
        DoorsHolder.instance.ResetDoors();

        string targetScene = ComputeTargetScene(targetCity, targetEnvironment);

        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);

        yield return new WaitUntil(() => operation.isDone);
        yield return null;

        if (targetEnvironment != Target.Joust)
        {
            Vector3 spawnPosition = ComputeSpawnPosition(targetCity);
            PlayerSpawner.instance.SpawnPlayer(spawnPosition);
        }
    }

    private Vector3 ComputeSpawnPosition(Cities targetCity)
    {
        DoorTrigger doorTrigger = DoorsHolder.instance.GetDoor(targetCity);

        return doorTrigger != null ? doorTrigger.SpawnPosition : Vector3.zero;
    }

    private string ComputeTargetScene(Cities targetCity, Target targetEnvironment)
    {
        if (targetEnvironment == Target.Exterior)
            return "WorldMap";

        if (targetEnvironment == Target.Joust)
            return "Joust";

        return $"{targetCity}_Interior";
    }
}
