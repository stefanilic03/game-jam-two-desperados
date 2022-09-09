using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IEnemy>() is not null)
        {
            collision.GetComponent<IEnemy>().DestroyEnemy();
        }
    }
}
