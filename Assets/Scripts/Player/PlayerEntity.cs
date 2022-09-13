using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerEntity
{
    float MaximumHealthPoints { get; set; }
    float CurrentHealthPoints { get; set; }
    void SetCurrentHealthToMaximum();
    void HealHealth();
}
