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
    public float groundCheckSize = 0.23f;
    public bool grounded;
    public LayerMask whatIsGround;

    //Jump
    public int jumpsAvailable = 2;
    public float jumpForce;
    public bool isFalling = false;
    public float fallFasterForce;

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
        grounded = false;
        Collider2D[] groundHits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckSize, whatIsGround);
        if (groundHits.Length > 0)
        {
            grounded = true;
            jumpsAvailable = 2;
        }

        isFalling = false;
        if (rigidBody2D.velocity.y < -2)
        {
            isFalling = true;
            rigidBody2D.AddForce(Vector2.down * fallFasterForce);
        }

        if (playerHoldingTheJetpackButton && jetpackBar.currentJetpackFuel > 0)
        {
            jetpackBar.currentJetpackFuel -= 4f;
            jetpackBar.regenerationDelay = Time.time + 0.75f;
            rigidBody2D.AddForce(Vector2.up * jetpackForce);
        }

        //Set animation parameters
        animator.SetBool(groundedParam, grounded);
        animator.SetBool(isFallingParam, isFalling);
        animator.SetBool(currentlyAttackingParam, currentlyAttacking);
        animator.SetBool(playerHoldingTheJetpackButtonParam, playerHoldingTheJetpackButton);
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && jumpsAvailable > 0)
        {
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);
    }

}
