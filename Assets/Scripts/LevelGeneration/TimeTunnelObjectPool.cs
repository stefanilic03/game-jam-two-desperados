using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTunnelObjectPool
{
    public TimeTunnelObjectPool()
    {

    }

    public TimeTunnelObjectPool(List<TimeTunnel> tunnels, int poolLimit, int timeTunnelLength)
    {
        this.tunnelPool = tunnels;
        this.poolLimit = poolLimit;
        this.timeTunnelLength = timeTunnelLength;
    }

    public List<TimeTunnel> tunnelPool = new List<TimeTunnel>();
    public int poolLimit = 10;
    public int timeTunnelLength = 37;
}
