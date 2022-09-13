using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGeneration : MonoBehaviour
{
    PowerUpObjectPooling pool;

    public List<PowerUp> powerupPrefabs;

    public Transform playerLocation;
    public float currentXPlayer;

    Quaternion spawnRotation;
    Vector3 spawnLocation;

    int spawnLocationYMinimum = -5;
    int spawnLocationYMaximum = 15;
    int currentSpawnLocationX;

    int randomNumberOfPowerUps;
    int currentPowerupPoolIndex = 0;

    private void Awake()
    {
        InvokeRepeating(nameof(GeneratePowerUps), 0f, 4f);
    }

    private void Start()
    {
        spawnRotation = new Quaternion(0, 0, 0, 0);

        pool = new PowerUpObjectPooling();

        //Fill up and instantiate powerups pool
        for (int i = 0; i < pool.poolLimit; i++)
        {
            pool.powerUpPool.Add(Instantiate(powerupPrefabs[Random.Range(0, 2)], new Vector2(0f, 0f), spawnRotation));
            pool.powerUpPool[i].gameObject.SetActive(false);
        }
    }

    void GeneratePowerUps()
    {
        if (playerLocation == null)
        {
            return;
        }

        randomNumberOfPowerUps = Random.Range(4, 20);
        for (int i = 0; i < randomNumberOfPowerUps; i++)
        {
            if (i == 0)
            {
                currentSpawnLocationX = (int)playerLocation.position.x + 100;
            }
            if (i > 0)
            {
                currentSpawnLocationX += currentSpawnLocationX + Random.Range(2, 8);
            }

            spawnLocation.x = currentSpawnLocationX;
            currentSpawnLocationX += 10;

            spawnLocation.y = Random.Range(spawnLocationYMinimum, spawnLocationYMaximum);

            if (!pool.powerUpPool[currentPowerupPoolIndex].gameObject.activeInHierarchy)
            {
                pool.powerUpPool[currentPowerupPoolIndex].gameObject.SetActive(true);
            }
            if (!pool.powerUpPool[currentPowerupPoolIndex].GetComponent<SpriteRenderer>().enabled)
            {
                pool.powerUpPool[currentPowerupPoolIndex].GetComponent<SpriteRenderer>().enabled = true;
            }

            pool.powerUpPool[currentPowerupPoolIndex].transform.position = spawnLocation;
            currentPowerupPoolIndex++;

            if (currentPowerupPoolIndex >= pool.poolLimit)
            {
                currentPowerupPoolIndex = 0;
            }
        }
    }
}
