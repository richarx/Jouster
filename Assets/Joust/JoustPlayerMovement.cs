using System;
using BoomLib.Inputs;
using UnityEngine;

public class JoustPlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private AnimationCurve accelerationCurve;

    private InputPacker inputPacker = new InputPacker();
    private InputPackage inputPackage = new InputPackage();

    private void Start()
    {

    }

    private void Update()
    {
        inputPackage = inputPacker.ComputeInputPackage();

        PerformSideMovement();
        PerformForwardMovement();
    }

    private void PerformSideMovement()
    {

    }

    private void PerformForwardMovement()
    {

    }
}
