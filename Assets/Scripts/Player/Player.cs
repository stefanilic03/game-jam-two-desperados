using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, PlayerEntity
{
    public Animator animator;
    static Player player;

    public static UnityAction takeDamage;

    float randomDamageTaken;

    public float MaximumHealthPoints { get; set; }
    public float CurrentHealthPoints { get; set; }
    public float HealthDepletionMultiplier { get; set; }

    //int minimunHealFromDefeatedEnemy = 25;
    //int maximunHealFromDefeatedEnemy = 40;

    const string deathAnimation = "death";

    void Awake()
    {
        player = this;

        takeDamage += TakeDamage;

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

    public void TakeDamage()
    {
        CurrentHealthPoints -= randomDamageTaken;
        if (CurrentHealthPoints <= 0)
        {
            animator.Play(deathAnimation);
            //Finish run
            return;
        }

        
    }
}
