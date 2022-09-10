using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent healPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemy>() is not null)
        {
            collision.GetComponent<IEnemy>().DestroyEnemy();
            healPlayer.Invoke();
        }
    }
}
