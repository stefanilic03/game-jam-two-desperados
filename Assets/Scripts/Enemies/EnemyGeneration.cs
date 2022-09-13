using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    EnemyObjectPooling pool;

    public List<Enemy> enemyPrefabs;

    public Transform playerLocation;
    public float currentXPlayer;

    Quaternion spawnRotation;
    Vector3 spawnLocation;

    public int spawnLocationY = 30;
    public int spawnLocationOctopusY = 16;
    public int currentSpawnLocationX;

    int randomNumberOfEnemies;
    int currentEnemyPoolIndex = 0;

    private void Awake()
    {
        InvokeRepeating(nameof(GenerateEnemies), 0f, 4f);
    }

    private void Start()
    {
        spawnRotation = new Quaternion(0, 0, 0, 0);

        pool = new EnemyObjectPooling();

        //Fill up and instantiate enemy pool
        for (int i = 0; i < pool.poolLimit; i++)
        {
            pool.enemyPool.Add(Instantiate(enemyPrefabs[Random.Range(0, 3)], new Vector2(0f, 0f), spawnRotation));
            pool.enemyPool[i].gameObject.SetActive(false);
        }
    }

    void GenerateEnemies()
    {
        if (playerLocation == null)
        {
            return;
        }
        randomNumberOfEnemies = Random.Range(4, 10);
        for (int i = 0; i < randomNumberOfEnemies; i++)
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

            if (pool.enemyPool[currentEnemyPoolIndex].GetComponent<Enemy>().flyingEnemy)
            {
                spawnLocation.y = spawnLocationOctopusY;
            }
            if (!pool.enemyPool[currentEnemyPoolIndex].GetComponent<Enemy>().flyingEnemy)
            {
                spawnLocation.y = spawnLocationY;
            }

            if (GetComponent<Animator>() != null && GetComponent<Animator>().enabled == false)
            {
                pool.enemyPool[currentEnemyPoolIndex].GetComponent<Animator>().enabled = true;
            }
            pool.enemyPool[currentEnemyPoolIndex].GetComponent<Enemy>().InTheTimeTunnel = false;
            pool.enemyPool[currentEnemyPoolIndex].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

            if (!pool.enemyPool[currentEnemyPoolIndex].gameObject.activeInHierarchy)
            {
                pool.enemyPool[currentEnemyPoolIndex].gameObject.SetActive(true);
            }

            pool.enemyPool[currentEnemyPoolIndex].transform.position = spawnLocation;
            currentEnemyPoolIndex++;

            if (currentEnemyPoolIndex >= pool.poolLimit)
            {
                currentEnemyPoolIndex = 0;
            }
        }
    }
}
