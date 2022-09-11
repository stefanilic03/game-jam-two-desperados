using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPooling
{
    public EnemyObjectPooling()
    {

    }

    public EnemyObjectPooling(int poolLimit)
    {
        this.poolLimit = poolLimit;
    }

    public EnemyObjectPooling(List<Enemy> enemyPool, int poolLimit)
    {
        this.enemyPool = enemyPool;
        this.poolLimit = poolLimit;
    }


    public List<Enemy> enemyPool = new List<Enemy>();

    public int poolLimit = 40;

}
