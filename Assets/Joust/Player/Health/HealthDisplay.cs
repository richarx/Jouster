using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private JoustPlayerHealth playerHealth;
    [SerializeField] private HitPoint heartPrefab;
    [SerializeField] private HitPoint armorPrefab;

    private List<HitPoint> heartList = new List<HitPoint>();
    private List<HitPoint> armorList = new List<HitPoint>();

    private void Start()
    {
        SetupHealthBar();

        playerHealth.OnTakeDamage.AddListener(LoseHealth);
        playerHealth.OnDie.AddListener(LoseHealth);
        playerHealth.OnTakeArmorDamage.AddListener(LoseArmor);
    }

    private void SetupHealthBar()
    {
        for (int i = 0; i < playerHealth.CurrentHealth; i++)
        {
            heartList.Add(SpawnIcon(heartPrefab));
        }

        for (int i = 0; i < playerHealth.CurrentArmor; i++)
        {
            armorList.Add(SpawnIcon(armorPrefab));
        }
    }

    private HitPoint SpawnIcon(HitPoint prefab)
    {
        return Instantiate(prefab, transform);
    }

    private void LoseHealth()
    {
        for (int i = heartList.Count - 1; i >= 0; i--)
        {
            if (heartList[i].IsActive)
            {
                heartList[i].SetState(false);
                break;
            }
        }
    }

    private void LoseArmor()
    {
        for (int i = armorList.Count - 1; i >= 0; i--)
        {
            if (armorList[i].IsActive)
            {
                armorList[i].SetState(false);
                break;
            }
        }
    }
}
