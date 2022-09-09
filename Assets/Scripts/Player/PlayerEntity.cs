using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerEntity
{
    float MaximumHealthPoints { get; set; }
    float CurrentHealthPoints { get; set; }
    float HealthDepletionMultiplier { get; set; }
    bool IsDead { get; set; }
    void SetCurrentHealthToMaximum();
    void SetCurrentHealth(float health);
    void HealHealth();
}
