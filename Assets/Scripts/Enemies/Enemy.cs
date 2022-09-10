using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public void DestroyEnemy()
    {
        //TODO: Particles on death
        Destroy(this);
    }
}
