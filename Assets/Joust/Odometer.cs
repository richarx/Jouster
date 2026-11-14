using UnityEngine;

public class Odometer : MonoBehaviour
{
    public static Odometer instance;

    private float currentMovement;
    private float currentDistance;

    public float CurrentMovement => currentMovement;
    public float CurrentDistance => currentDistance;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerSpeed(float velocity)
    {
        currentMovement = velocity * Time.deltaTime;
        currentDistance += currentMovement;
    }
}
