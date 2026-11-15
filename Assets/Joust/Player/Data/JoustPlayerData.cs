using PrimeTween;
using UnityEngine;

[CreateAssetMenu(fileName = "JoustPlayerData", menuName = "Scriptable Objects/JoustPlayerData")]
public class JoustPlayerData : ScriptableObject
{
    [Header("Sideway Movement")]
    public float timeToChangeLane;
    public Ease laneChangeEase;
    public float distanceLaneToLane;

    [Space]
    [Header("Forward Movement")]
    public float maxSpeed;
    public float timeToMaxSpeed;
    public AnimationCurve accelerationCurve;

    [Space]
    [Header("Jump")]
    public float jumpDuration;
    public float jumpMaxHeight;
    public AnimationCurve jumpCurve;

    [Space]
    [Header("Stun")]
    public float knockbackPower;
    public float knockbackDuration;

    [Space]
    [Header("Death")]
    public float deathDeceleration;

    [Space]
    [Header("Power")]
    public float maxPower;
    public float powerPerSpeed;
}
