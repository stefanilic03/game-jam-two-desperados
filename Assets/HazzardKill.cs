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

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        playerTakeDamage.AddListener(Player.takeDamage);
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
