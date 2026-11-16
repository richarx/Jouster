using System;
using BoomTools;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplay : MonoBehaviour
{
    [SerializeField] private JoustPlayerPower playerPower;

    [Space]
    [SerializeField] private Image bar;
    [SerializeField] private RectTransform target;
    [SerializeField] private Sprite redBar;
    [SerializeField] private Sprite greenBar;
    [SerializeField] private float animationSmoothTime;

    private float barVelocity;
    private float targetVelocity;

    private float targetPosition;

    private void Awake()
    {
        playerPower.OnUpdateTarget.AddListener(SetTarget);
        bar.fillAmount = 0.0f;
    }

    private void LateUpdate()
    {
        UpdatePower();
        UpdateTarget();

    }

    private void UpdatePower()
    {
        float targetPower = Tools.NormalizeValue(playerPower.CurrentPower, 0.0f, playerPower.MaxPower);
        bar.fillAmount = Mathf.SmoothDamp(bar.fillAmount, targetPower, ref barVelocity, animationSmoothTime);
        bar.sprite = playerPower.HasEnoughPower ? greenBar : redBar;
    }

    private void UpdateTarget()
    {
        if (!target.gameObject.activeSelf)
            return;

        float newPosition = Mathf.SmoothDamp(target.localPosition.x, targetPosition, ref targetVelocity, animationSmoothTime);
        target.localPosition = new Vector3(newPosition, 0.0f, 0.0f);
    }

    private void SetTarget()
    {
        target.gameObject.SetActive(true);

        float normalizedTarget = Tools.NormalizeValue(playerPower.TargetPower, 0.0f, playerPower.MaxPower);
        float maxSize = bar.GetComponent<RectTransform>().sizeDelta.x;

        targetPosition = Tools.NormalizeValueInRange(normalizedTarget * maxSize, 0.0f, maxSize, -maxSize / 2.0f, maxSize / 2.0f);
    }
}
