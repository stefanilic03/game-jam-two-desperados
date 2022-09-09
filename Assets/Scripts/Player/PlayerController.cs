using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //public InputAction inputActions;
    public Rigidbody2D rigidBody2D;
    public Animator animator;

    //Check to see if the player is currently touching the ground
    public bool grounded;
    public LayerMask whatIsGround;

    //Movement
    public float movementSpeed;
    public float speedMultiplier;
    public float currentSpeed;

    //Jump
    public float jumpsAvailable = 2f;
    public float jumpForce;
    public bool isFalling = false;

    //Attacking
    public bool currentlyAttacking = false;
    public float currentBasicAttackCooldown;
    public float basicAttackCooldown;

    //Has the game started yet
    public bool hasStarted = false;

    //Animation parameters
    const string currentSpeedParam = "currentSpeed";
    const string groundedParam = "grounded";
    const string isFallingParam = "isFalling";
    const string currentlyAttackingParam = "currentlyAttacking";


    private void FixedUpdate()
    {
        grounded = false;
        if (rigidBody2D.IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            jumpsAvailable = 2f;
        }

        rigidBody2D.velocity = new Vector2(movementSpeed * speedMultiplier, rigidBody2D.velocity.y);
        currentSpeed = rigidBody2D.velocity.x;

        isFalling = false;
        if (rigidBody2D.velocity.y < -2)
        {
            isFalling = true;
        }

        //Set animation parameters
        animator.SetBool(groundedParam, grounded);
        animator.SetBool(isFallingParam, isFalling);
        animator.SetFloat(currentSpeedParam, currentSpeed);
        animator.SetBool(currentlyAttackingParam, currentlyAttacking);
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

    public void StopAttacking_AnimationEvent()
    {
        currentlyAttacking = false;
    }
}
