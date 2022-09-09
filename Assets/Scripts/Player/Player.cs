using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, PlayerEntity
{
    public float MaximumHealthPoints { get; set; }
    public float CurrentHealthPoints { get; set; }
    public float HealthDepletionMultiplier { get; set; }

    void Awake()
    {
        MaximumHealthPoints = 100;
        SetCurrentHealthToMaximum();
    }

    public void HealHealth()
    {
        //TODO: Heal health over time based on the value from the killed enemy
    }

    public void SetCurrentHealth(float health)
    {
        CurrentHealthPoints = health;
    }

    public void SetCurrentHealthToMaximum()
    {
        CurrentHealthPoints = MaximumHealthPoints;
    }
}
