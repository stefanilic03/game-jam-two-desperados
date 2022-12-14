using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Animator animator;  

    public JetpackResourceBar jetpackBar;
    public float maximumJetpackStamina;
    public float currentJetpackStamina;

    //Check to see if the player is currently touching the ground
    public Transform groundCheck;
    public float groundCheckSize;
    public bool grounded;
    public LayerMask whatIsGround;

    //Jump
    public int jumpsAvailable = 3;
    public float jumpForce;
    public bool isFalling = false;
    public float fallFasterForce;
    bool jumped = false;

    //Attacking
    public bool currentlyAttacking = false;
    public float currentBasicAttackCooldown;
    public float basicAttackCooldown;

    //Jetpack
    public bool playerHoldingTheJetpackButton = false;
    public float jetpackForce;

    //Has the game started yet
    public bool hasStarted = false;

    //Animation parameters
    const string groundedParam = "grounded";
    const string isFallingParam = "isFalling";
    const string currentlyAttackingParam = "currentlyAttacking";
    const string playerHoldingTheJetpackButtonParam = "jetpackButtonHeld";

    private void FixedUpdate()
    {
        GroundCheck();

        isFalling = false;
        if (rigidBody2D.velocity.y < -2)
        {
            isFalling = true;
            //Make the player fall faster so jumping feels more natural -> adds a slight curve towards the end of the jump
            rigidBody2D.AddForce(Vector2.down * fallFasterForce);
        }

        //If the player is holding the jetpack button, slowly deplete the jetpack fuel
        JetpackButtonHoldCheck();

        //Animations
        SetAnimatorParameters();
    }

    private void GroundCheck()
    {
        grounded = false;
        Collider2D[] groundHits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckSize, whatIsGround);
        if (groundHits.Length > 0)
        {
            grounded = true;
            if (!jumped)
            {
                jumpsAvailable = 3;
            }
        }
    }

    private void JetpackButtonHoldCheck()
    {
        if (playerHoldingTheJetpackButton && jetpackBar.currentJetpackFuel > 0)
        {
            jetpackBar.currentJetpackFuel -= 4f;
            jetpackBar.regenerationDelay = Time.time + 0.75f;
            rigidBody2D.AddForce(Vector2.up * jetpackForce);
        }
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && jumpsAvailable > 0)
        {
            if (grounded)
            {
                jumped = true;
                StartCoroutine(JumpCounterDelay());
            }

            grounded = false;
            jumpsAvailable--;
            rigidBody2D.velocity = Vector2.up * jumpForce;
        }
    }

    public void OnBasicAttack(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && !currentlyAttacking && Time.time > currentBasicAttackCooldown)
        {
            currentlyAttacking = true;
            currentBasicAttackCooldown = Time.time + basicAttackCooldown;
        }
    }

    public void OnJetpackUse(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            playerHoldingTheJetpackButton = true;
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
        }
        if (callbackContext.canceled)
        {
            playerHoldingTheJetpackButton = false;
        }
    }

    public void StopAttacking_AnimationEvent()
    {
        currentlyAttacking = false;
    }

    private void SetAnimatorParameters()
    {
        animator.SetBool(groundedParam, grounded);
        animator.SetBool(isFallingParam, isFalling);
        animator.SetBool(currentlyAttackingParam, currentlyAttacking);
        animator.SetBool(playerHoldingTheJetpackButtonParam, playerHoldingTheJetpackButton);
    }

    private IEnumerator JumpCounterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        jumped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);
    }

}
