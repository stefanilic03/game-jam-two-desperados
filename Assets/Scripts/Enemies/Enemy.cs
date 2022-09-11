using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    Animator animator;

    public bool flyingEnemy = false;

    const string deathAnimation = "death";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void DestroyEnemy()
    {
        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        animator.Play(deathAnimation);
        yield return new WaitForSeconds(2f);
        //Add back to pool, disable
    }
}
