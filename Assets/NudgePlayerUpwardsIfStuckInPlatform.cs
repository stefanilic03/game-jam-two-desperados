using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NudgePlayerUpwardsIfStuckInPlatform : MonoBehaviour
{
    const string playerTag = "Player";
    Vector2 nudgeLocation;
    float nudgeSpeed = 0f;
    float nudgeSpeedIncrease = 0.225f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            nudgeSpeed += nudgeSpeedIncrease;
            nudgeLocation = new Vector2(collision.transform.position.x, collision.transform.position.y + nudgeSpeed);
            collision.transform.position = nudgeLocation;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            nudgeSpeed = 0f;
        }
    }
}
