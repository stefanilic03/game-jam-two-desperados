using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{ 
    public float PointsWorth { get; set; }
    public float HitPointsReplenished { get; set; }
    public void DestroyEnemy();
    public void ReplenishHitPoints();
    public ParticleSystem OnDeathParticles { get; set; }
}
