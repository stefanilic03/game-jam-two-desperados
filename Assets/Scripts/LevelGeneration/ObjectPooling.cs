using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling
{
    public ObjectPooling()
    {
        
    }

    public ObjectPooling(int poolLimit)
    {
        this.poolLimit = poolLimit;
    }

    public ObjectPooling(List<Platform> platformPool, int poolLimit, float middleOfABlock)
    {
        this.platformPool = platformPool;
        this.poolLimit = poolLimit;
        this.middleOfABlock = middleOfABlock;
    }


    public List<Platform> platformPool = new List<Platform>();

    public int poolLimit = 80;

    public float middleOfABlock;
}
