using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAI : MonoBehaviour
{
    //Self references
    public Rigidbody2D rigidBody2D;
    public Animator animator;
    public Transform groundCheck;
    public GameObject attackIndicator;

    //Ground checking
    bool grounded;
    public float groundCheckRange = 0.2f;
    public LayerMask whatIsGround;

    //Attack system
    public float attackTimer;
    public float attackLength;
    public float attackHeight;
    Vector2 jump;

    const string idleAnimation = "idle";
    const string jumpAttackAnimation = "jumpAttack";

    private void Awake()
    {
        InvokeRepeating(nameof(ShowAttackIndicatorThenAttack), 3.5f, 3); 
    }

    private void FixedUpdate()
    {
        GroundCheck();

        if (!grounded)
        {
            animator.Play(jumpAttackAnimation);
            return;
        }
        animator.Play(idleAnimation);
    }

    void ShowAttackIndicatorThenAttack()
    {   
        if (gameObject.activeInHierarchy)
        {
            attackIndicator.SetActive(true);
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackTimer);
        jump = new Vector2(rigidBody2D.velocity.x - attackLength, rigidBody2D.velocity.y + attackHeight);
        rigidBody2D.AddForce(jump, ForceMode2D.Impulse);
        attackIndicator.SetActive(false);
    }

    void GroundCheck()
    {
        Collider2D[] groundHits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRange, whatIsGround);
        if (groundHits.Length > 0)
        {
            grounded = true;
            return;
        }
        grounded = false;
    }
}
