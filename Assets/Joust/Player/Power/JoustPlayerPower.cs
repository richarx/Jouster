using System;
using UnityEngine;
using UnityEngine.Events;

public class JoustPlayerPower : MonoBehaviour
{
    public UnityEvent OnUpdateTarget = new UnityEvent();

    private float targetPower = 15;
    private float currentPower;
    public float CurrentPower => currentPower;
    public float TargetPower => targetPower;
    public float MaxPower => joustPlayer.data.maxPower;
    public bool HasEnoughPower => currentPower >= targetPower;

    JoustPlayerStateMachine joustPlayer;

    public void Setup(JoustDifficultyData data)
    {
        joustPlayer = JoustPlayerStateMachine.instance;
        targetPower = data.targetPower;
        OnUpdateTarget?.Invoke();
    }

    private void Update()
    {
        currentPower = ComputeCurrentPower();
    }

    private float ComputeCurrentPower()
    {
        float power = joustPlayer.velocity * joustPlayer.data.powerPerSpeed;

        return power;
    }
}
