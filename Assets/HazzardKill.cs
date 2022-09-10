using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HazzardKill : MonoBehaviour
{
    [SerializeField]
    private UnityEvent death;

    private void Start()
    {
        death.AddListener(Player.death);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            death.Invoke();
        }
    }
}
