using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGeneration : MonoBehaviour
{
    public static PlatformGeneration platformGeneration;

    ObjectPooling pool1;
    ObjectPooling pool2;
    ObjectPooling starts;

    public List<Platform> platformPrefabs;

    public Transform playerLocation;
    public float currentXPlayer;

    Vector3 currentPosition;
    Quaternion spawnRotation;

    public bool currentlySpawning = false;

    int randomY;
    int lastRandomY = 5;
    int randomNumberOfPlatformsInARow;

    public int startsCounter;


    private void Awake()
    {
        if (platformGeneration is not null && platformGeneration != this)
        {
            Destroy(this);
            return;
        }
        if (platformGeneration is null)
        {
            platformGeneration = this;
        }
    }

    
    private void Start()
    {
        currentPosition = transform.position;
        spawnRotation = new Quaternion(0, 0, 0, 0);

        pool1 = new ObjectPooling();
        pool2 = new ObjectPooling();
        starts = new ObjectPooling();

        for (int i = 0; i < starts.poolLimit; i++)
        {
            starts.platformPool.Add(Instantiate(platformPrefabs[3], new Vector2(0, -500), spawnRotation));
            starts.platformPool[i].gameObject.SetActive(false);
        }

        //Fill up and instantiate pool 1

        for (int i = 0; i < pool1.poolLimit; i++)
        {
            pool1.platformPool.Add(Instantiate(platformPrefabs[Random.Range(0, 2)], currentPosition, spawnRotation));
            currentPosition.x++;
        }

        pool1.middleOfABlock = (pool1.platformPool[pool1.poolLimit - 1].transform.position.x + pool1.platformPool[0].transform.position.x) / 2;

        //Fill up and instantiate pool 2

        for (int i = 0; i < pool2.poolLimit; i++)
        {
            pool2.platformPool.Add(Instantiate(platformPrefabs[Random.Range(0, 2)], currentPosition, spawnRotation));
            currentPosition.x++;
        }

        pool2.middleOfABlock = (pool2.platformPool[pool2.poolLimit - 1].transform.position.x + pool2.platformPool[0].transform.position.x) / 2;
    }

    private void Update()
    {
        currentXPlayer = playerLocation.position.x;

        // if the player is close to the next block, start generating from the pool
        if (currentXPlayer >= pool1.middleOfABlock && !currentlySpawning && pool1.middleOfABlock > pool2.middleOfABlock)
        {
            currentlySpawning = true;
            GenerateNextBlock2();
        }
        if (currentXPlayer >= pool2.middleOfABlock && !currentlySpawning && pool2.middleOfABlock > pool1.middleOfABlock)
        {
            currentlySpawning = true;
            GenerateNextBlock1();
        }
    }

    void GenerateNextBlock2()
    {
        for (int i = 0; i < pool2.poolLimit; i++)
        {
            randomY = Random.Range(-5, 15);
            if (Mathf.Abs(randomY - lastRandomY) < 5)
            {
                if (randomY >= lastRandomY)
                {
                    randomY += 5;
                }
                if (randomY < lastRandomY)
                {
                    randomY -= 5;
                }
            }
            if (Mathf.Abs(randomY - lastRandomY) > 10)
            {
                if (randomY >= lastRandomY)
                {
                    randomY -= 5;
                }
                if (randomY < lastRandomY)
                {
                    randomY += 5;
                }
            }

            lastRandomY = randomY;
            randomNumberOfPlatformsInARow = Random.Range(5, 20);
            currentPosition.y = randomY;
            starts.platformPool[startsCounter].gameObject.SetActive(true);
            starts.platformPool[startsCounter].transform.position = currentPosition;
            startsCounter++;
            if (startsCounter >= starts.poolLimit)
            {
                startsCounter = 0;
            }
            currentPosition.x++;

            if (i + randomNumberOfPlatformsInARow > pool1.poolLimit)
            {
                randomNumberOfPlatformsInARow = pool2.poolLimit - i;
            }

            for (int j = 0; j < randomNumberOfPlatformsInARow; j++)
            {
                pool2.platformPool[i].transform.position = currentPosition;
                currentPosition.x++;
                if (j < randomNumberOfPlatformsInARow - 1)
                {
                    i++;
                }
            }
        }

        pool2.middleOfABlock = (pool2.platformPool[pool2.poolLimit - 1].transform.position.x + pool2.platformPool[0].transform.position.x) / 2;

        currentlySpawning = false;
    }

    void GenerateNextBlock1()
    {
        for (int i = 0; i < pool1.poolLimit; i++)
        {
            randomY = Random.Range(-5, 15);

            if (Mathf.Abs(randomY - lastRandomY) < 5)
            {
                if (randomY >= lastRandomY)
                {
                    randomY += 5;
                }
                if (randomY < lastRandomY)
                {
                    randomY -= 5;
                }
            }
            if (Mathf.Abs(randomY - lastRandomY) > 10)
            {
                if (randomY >= lastRandomY)
                {
                    randomY -= 5;
                }
                if (randomY < lastRandomY)
                {
                    randomY += 5;
                }
            }

            lastRandomY = randomY;
            randomNumberOfPlatformsInARow = Random.Range(5, 20);
            currentPosition.y = randomY;
            starts.platformPool[startsCounter].gameObject.SetActive(true);
            starts.platformPool[startsCounter].transform.position = currentPosition;
            startsCounter++;
            if (startsCounter >= starts.poolLimit)
            {
                startsCounter = 0;
            }
            currentPosition.x++;

            if (i + randomNumberOfPlatformsInARow > pool1.poolLimit)
            {
                randomNumberOfPlatformsInARow = pool1.poolLimit - i;
            }
            for (int j = 0; j < randomNumberOfPlatformsInARow; j++)
            {
                pool1.platformPool[i].transform.position = currentPosition;
                currentPosition.x++;
                if (j < randomNumberOfPlatformsInARow - 1)
                {
                    i++;
                }
            }
        }

        pool1.middleOfABlock = (pool1.platformPool[pool1.poolLimit - 1].transform.position.x + pool1.platformPool[0].transform.position.x) / 2;
        
        currentlySpawning = false;
    }
}
