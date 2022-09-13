using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class TimePortal : MonoBehaviour
{
    [SerializeField]
    private UnityEvent timePortalEvent;

    public float suckInSpeed;
    public Transform suckInLocation;
    public Animator animator;

    const string portalCreationAnimation = "portalCreation";

    Player player;
    Rigidbody2D playerRigidBody2D;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).GetComponent<Player>();
        playerRigidBody2D = player.GetComponent<Rigidbody2D>();
        timePortalEvent.AddListener(player.timePortal);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsDatabase.playerTag))
        {
            timePortalEvent.Invoke();
            playerRigidBody2D.constraints = RigidbodyConstraints2D.None;
            playerRigidBody2D.gravityScale = 0;
            playerRigidBody2D.velocity = Vector2.zero;
            animator.Play(portalCreationAnimation);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsDatabase.playerTag))
        {
            playerRigidBody2D.velocity = Vector2.zero;
            player.transform.position = Vector2.MoveTowards(player.transform.position, suckInLocation.position, suckInSpeed);
        }
    }
}
