using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class HazzardKill : MonoBehaviour
{
    [SerializeField]
    private UnityEvent playerTakeDamage;
    public Enemy enemy;
    Player player;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag(TagsDatabase.playerTag) != null)
        {
            player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).GetComponent<Player>();
            playerTakeDamage.AddListener(player.takeDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemy.InTheTimeTunnel)
            {
                enemy.DestroyEnemy();
                return;
            }

            playerTakeDamage.Invoke();
        }
    }
}
