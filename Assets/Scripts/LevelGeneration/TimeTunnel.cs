using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class TimeTunnel : MonoBehaviour
{
    [SerializeField]
    private UnityEvent timeTunnelTravelEvent;

    Player player;

    const string playerTag = "Player";
    const string enemyTag = "Enemy";

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).GetComponent<Player>();
        timeTunnelTravelEvent.AddListener(player.timeTunnelTravel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            timeTunnelTravelEvent.Invoke();
        }
        if (collision.CompareTag(enemyTag))
        {
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            collision.GetComponent<Enemy>().InTheTimeTunnel = true;
            collision.GetComponent<Animator>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            timeTunnelTravelEvent.Invoke();
        }
    }
}
