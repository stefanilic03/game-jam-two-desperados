using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAI : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Animator animator;
    public Transform groundCheck;
    public bool grounded;
    public float groundCheckRange = 0.2f;
    public LayerMask whatIsGround;

    public float attackTimer;
    public float attackLength;
    public float attackHeight;


    const string idleAnimation = "idle";
    const string jumpAttackAnimation = "jumpAttack";

    private void Awake()
    {
        InvokeRepeating(nameof(JumpAttack), 3.5f, 5); 
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

    void JumpAttack()
    {
        Vector2 jjump = new Vector2(rigidBody2D.velocity.x - attackLength, rigidBody2D.velocity.y + attackHeight);
        rigidBody2D.AddForce(jjump, ForceMode2D.Impulse);
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
