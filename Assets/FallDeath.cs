using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FallDeath : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsDatabase.playerTag))
        {
            collision.GetComponent<Player>().Death();
        }
    }
}
