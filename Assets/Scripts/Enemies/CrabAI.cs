using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAI : MonoBehaviour
{
    //Facing direction
    public SpriteRenderer spriteRenderer;
    bool facingLeft = true;

    //Movement
    public Rigidbody2D rigidBody2D;
    public Vector2 startingPosition;
    public int direction = -1;
    public float movementSpeed;
    float trackLength = 3;
    float trackMinimum;
    float trackMaximum;

    public Transform groundCheck;
    public bool grounded = false;
    public float platformCheckSize = 0.2f;
    public LayerMask whatIsAPlatform;

    //Decided to allow the crabs to fall of the platforms and keep 'em crazy like that
    //The alghoritm below needs some adjustments. It works if the crab is instantiated in scene editor, but doesn't if it's instantiated in EnemyGeneration script
    //Check if the enemy will fall of the platform
    //public Transform doNotFallOffThePlatform;
    //public bool trackHasBeenSet = false;

    private void Awake()
    {
        startingPosition = transform.position;
        trackMinimum = startingPosition.x - trackLength;
        trackMaximum = startingPosition.x + trackLength;
    }

    private void FixedUpdate()
    {
        Collider2D[] groundHits = Physics2D.OverlapCircleAll(groundCheck.position, platformCheckSize, whatIsAPlatform);
        if (groundHits.Length > 0)
        {
            grounded = true;
        }
        if (grounded)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (transform.position.x <= trackMinimum && direction == -1)
        {
            direction = -direction;
        }
        if (transform.position.x >= trackMaximum && direction == 1)
        {
            direction = -direction;
        }
        rigidBody2D.velocity = new Vector2(direction * movementSpeed, rigidBody2D.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (facingLeft && direction == 1)
        {
            Flip();
        }
        if (!facingLeft && direction == -1)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;

        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}
