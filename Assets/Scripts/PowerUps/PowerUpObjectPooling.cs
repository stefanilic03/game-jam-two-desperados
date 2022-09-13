using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpObjectPooling
{
    public PowerUpObjectPooling()
    {

    }

    public PowerUpObjectPooling(int poolLimit)
    {
        this.poolLimit = poolLimit;
    }

    public PowerUpObjectPooling(List<PowerUp> powerUpPool, int poolLimit)
    {
        this.powerUpPool = powerUpPool;
        this.poolLimit = poolLimit;
    }


    public List<PowerUp> powerUpPool = new List<PowerUp>();

    public int poolLimit = 40;
}
