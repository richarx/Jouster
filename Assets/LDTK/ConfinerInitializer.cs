using Unity.Cinemachine;
using UnityEngine;

public class ConfinerInitializer : MonoBehaviour
{
    private void Start()
    {
        CinemachineConfiner2D cinemachineConfiner2D = PlayerSpawner.instance.CinemachineCamera.GetComponent<CinemachineConfiner2D>();
        cinemachineConfiner2D.BoundingShape2D = GetComponent<PolygonCollider2D>();
    }
}
