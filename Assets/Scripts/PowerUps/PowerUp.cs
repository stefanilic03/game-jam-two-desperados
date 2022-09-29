using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private UnityEvent healPlayer;
    [SerializeField]
    private UnityEvent regenerateJetpackFuelPlayer;

    public AudioSource sfx;

    Player player;

    public SpriteRenderer spriteRenderer;

    public bool redOrBlue = true;
    int disableDelay = 2;

    public int numberOfPointsOnPickup;

    private void Reset()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).GetComponent<Player>();

        healPlayer.AddListener(player.healthPowerUp);
        regenerateJetpackFuelPlayer.AddListener(player.jetpackPowerUp);

        numberOfPointsOnPickup *= (int)GameMaster.difficultyMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagsDatabase.playerTag))
        {
            HealOrRegenerateJetpackFuel();
            sfx.Play();
            spriteRenderer.enabled = false;
            GameMaster.currentScore += numberOfPointsOnPickup;
            StartCoroutine(DisableObjectDelay());
        }
    }

    IEnumerator DisableObjectDelay()
    {
        yield return new WaitForSeconds(disableDelay);
        gameObject.SetActive(false);
    }

    private void HealOrRegenerateJetpackFuel()
    {
        if (redOrBlue)
        {
            healPlayer.Invoke();
        }
        if (!redOrBlue)
        {
            regenerateJetpackFuelPlayer.Invoke();
        }
    }
}
