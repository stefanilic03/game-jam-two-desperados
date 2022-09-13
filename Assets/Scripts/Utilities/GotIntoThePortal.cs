using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotIntoThePortal : MonoBehaviour
{
    public GameObject endGameMenu;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsDatabase.playerTag))
        {
            endGameMenu.SetActive(true);
            Destroy(collision.gameObject);
        }
    }
}
