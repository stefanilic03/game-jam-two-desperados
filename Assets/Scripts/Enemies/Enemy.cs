using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    Animator animator;

    public bool flyingEnemy = false;
    public bool InTheTimeTunnel { get; set; }

    const string deathAnimation = "death";

    int deathAnimationDelay = 1;

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
        yield return new WaitForSeconds(deathAnimationDelay);
        this.gameObject.SetActive(false);
    }
}
