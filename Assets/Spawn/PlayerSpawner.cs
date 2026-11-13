using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public static PlayerSpawner instance;

    public CinemachineCamera CinemachineCamera => cinemachineCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Vector3 position = SpawnPosition.instance.Position;
        GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);

        cinemachineCamera.Target.TrackingTarget = player.transform;
    }
}
