using PrimeTween;
using UnityEngine;

[CreateAssetMenu(fileName = "JoustPlayerData", menuName = "Scriptable Objects/JoustPlayerData")]
public class JoustPlayerData : ScriptableObject
{
    public float timeToChangeLane;
    public Ease laneChangeEase;

    [Space]
    public float distanceLaneToLane;

    [Space]
    public float maxSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve accelerationCurve;

    [Space]
    public float knockbackPower;
    public float knockbackDuration;

    [Space]
    public float deathDeceleration;
}
