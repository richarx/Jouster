using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject worldMapPlayerPrefab;
    [SerializeField] private GameObject interiorPlayerPrefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public static PlayerSpawner instance;

    public CinemachineCamera CinemachineCamera => cinemachineCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (SpawnPosition.instance != null)
            SpawnPlayer(SpawnPosition.instance.Position);
    }

    public void SpawnPlayer(Vector3 position)
    {
        GameObject player = Instantiate(ComputePlayerPrefab(), position, Quaternion.identity);

        cinemachineCamera.Target.TrackingTarget = player.transform;

        Debug.Log($"Position : {position}");
    }

    private GameObject ComputePlayerPrefab()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Interior"))
            return interiorPlayerPrefab;

        return worldMapPlayerPrefab;
    }
}
