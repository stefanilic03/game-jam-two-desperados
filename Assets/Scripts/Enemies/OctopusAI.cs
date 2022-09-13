using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusAI : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public Vector2 movement;
    public int directionX;
    public int directionY;

    public bool startOrStop = true;
    public bool choosingNextDirection = false;
    public float movementSpeed;

    private void Awake()
    {
        InvokeRepeating(nameof(StartMovingAndStopMoving), 0, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!startOrStop && !choosingNextDirection)
        {
            choosingNextDirection = true;
            rigidBody2D.velocity = Vector2.zero;
            ChooseNextDirection();
        }
        if (startOrStop)
        {
            choosingNextDirection = false;
            rigidBody2D.velocity = movement;
        }
    }

    void ChooseNextDirection()
    {
        directionY = Random.Range(-1, 2);
        directionX = Random.Range(-1, 2);
        if (directionY == 0 && directionX == 0)
        {
            directionX = -1;
        }
        movement = new Vector2(directionX * movementSpeed, directionY * movementSpeed);
    }

    void StartMovingAndStopMoving()
    {
        startOrStop = !startOrStop;
    }
}
