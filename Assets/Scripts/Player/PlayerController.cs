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
    public float jumpsAvailable = 3f;
    public float jumpForce;
    public bool isFalling = false;

    //Has the game started yet
    public bool hasStarted = false;

    //Animation parameters
    const string currentSpeedParam = "currentSpeed";
    const string groundedParam = "grounded";
    const string isFallingParam = "isFalling";


    private void FixedUpdate()
    {
        grounded = false;
        if (rigidBody2D.IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            jumpsAvailable = 3f;
        }

        rigidBody2D.velocity = new Vector2(movementSpeed * speedMultiplier, rigidBody2D.velocity.y);
        currentSpeed = rigidBody2D.velocity.x;

        isFalling = false;
        if (rigidBody2D.velocity.y < 0)
        {
            isFalling = true;
        }

        //Set animation parameters
        animator.SetBool(groundedParam, grounded);
        animator.SetBool(isFallingParam, isFalling);
        animator.SetFloat(currentSpeedParam, currentSpeed);
    }

    public void OnJump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && jumpsAvailable > 0)
        {
            rigidBody2D.velocity = Vector2.up * jumpForce;
        }
    }
}
