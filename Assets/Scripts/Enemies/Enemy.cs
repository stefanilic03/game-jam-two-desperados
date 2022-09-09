using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public float PointsWorth { get; set; }
    public float HitPointsReplenished { get; set; }
    public ParticleSystem OnDeathParticles { get; set; }

    public float pointsWorthMinimum;
    public float pointsWorthMaximum;

    public float hitPointsReplenishedMinimum;
    public float hitPointsReplenishedMaximum;

    private void Awake()
    {
        PointsWorth = Random.Range(pointsWorthMinimum, pointsWorthMaximum) * GameMaster.gameMaster.difficultyMultiplier;
        HitPointsReplenished = Random.Range(hitPointsReplenishedMinimum, hitPointsReplenishedMaximum);
    }

    public void DestroyEnemy()
    {
        Destroy(this);
    }

    public void ReplenishHitPoints()
    {
        throw new System.NotImplementedException();
    }
}
