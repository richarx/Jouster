using BoomTools;
using UnityEngine;

public class Odometer : MonoBehaviour
{
    public static Odometer instance;

    private float currentVelocity;
    private float currentMovement;
    private float currentDistance;
    private float normalizedSpeed;

    public float CurrentVelocity => currentVelocity;
    public float CurrentMovement => currentMovement;
    public float CurrentDistance => currentDistance;
    public float NormalizedSpeed => normalizedSpeed;

    private void Awake()
    {
        instance = this;
    }

    public void SetPlayerSpeed(float velocity, float maxSpeed)
    {
        currentVelocity = velocity;
        currentMovement = velocity * Time.deltaTime;
        currentDistance += currentMovement;
        normalizedSpeed = Tools.NormalizeValue(velocity, 0.0f, maxSpeed);
    }
}
