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
    TimeTunnelObjectPool timeTunnelObjectPool;

    public List<Platform> platformPrefabs;
    public List<TimeTunnel> timeTunnelPrefabs;

    public Transform playerLocation;
    public float currentXPlayer;

    Vector3 currentPosition;
    Quaternion spawnRotation;

    public bool currentlySpawning = false;

    int randomY;
    int lastRandomY = 5;
    int randomNumberOfPlatformsInARow;
    bool numberOfPlatformsInARowLowerThanFive = false;

    public int startsCounter;

    public int timeTunnelCreationChance;
    public int timeTunnelPoolCounter = 0;

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
        timeTunnelObjectPool = new TimeTunnelObjectPool();

        for (int i = 0; i < timeTunnelObjectPool.poolLimit; i++)
        {
            timeTunnelObjectPool.tunnelPool.Add(Instantiate(timeTunnelPrefabs[Random.Range(0, 2)], new Vector2(0, -100), spawnRotation));
            timeTunnelObjectPool.tunnelPool[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < starts.poolLimit; i++)
        {
            starts.platformPool.Add(Instantiate(platformPrefabs[3], new Vector2(0, -100), spawnRotation));
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
        if (playerLocation == null)
        {
            return;
        }
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
        timeTunnelCreationChance = Random.Range(0, 11);
        if (timeTunnelCreationChance < 11)
        {
            if (!timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].gameObject.activeInHierarchy)
            {
                timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].gameObject.SetActive(true);
            }
            currentPosition.y = Random.Range(-5, 15);
            timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].transform.position = currentPosition;
            timeTunnelPoolCounter++;
            if (timeTunnelPoolCounter >= timeTunnelObjectPool.poolLimit)
            {
                timeTunnelPoolCounter = 0;
            }
            
        }
        for (int i = 0; i < pool2.poolLimit; i++)
        {
            //if (i + 5 > pool2.poolLimit)
            //{
            //    pool2.poolLimit += (i + 5) - pool2.poolLimit;
            //    for (int j = 0; j < pool2.poolLimit; j++)
            //    {
            //        pool2.platformPool.Add(Instantiate(platformPrefabs[Random.Range(0, 2)], new Vector2(0, -500), spawnRotation));
            //    }
            //}
            if (i + 5 > pool2.poolLimit)
            {
                numberOfPlatformsInARowLowerThanFive = true;
            }
            else
            {
                numberOfPlatformsInARowLowerThanFive = false;
            }
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
            //if (UtilityMethods.IsIntegerInRangeInclusive(randomY, lastRandomY - 5, randomY + 5))
            //{
            //    if (randomY >= lastRandomY)
            //    {
            //        randomY -= 5;
            //    }
            //    if (randomY < lastRandomY)
            //    {
            //        randomY += 5;
            //    }
            //}

            lastRandomY = randomY;
            if (randomY < -5 || randomY > 15)
            {
                lastRandomY = Random.Range(5, 11);
            }
            randomNumberOfPlatformsInARow = Random.Range(5, 20);
            currentPosition.y = randomY;
            if (numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(false);
            }
            if (!starts.platformPool[startsCounter].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(true);
            }
            starts.platformPool[startsCounter].transform.position = currentPosition;
            startsCounter++;
            if (startsCounter >= starts.poolLimit)
            {
                startsCounter = 0;
            }
            currentPosition.x++;

            if (i + randomNumberOfPlatformsInARow > pool2.poolLimit)
            {
                randomNumberOfPlatformsInARow = pool2.poolLimit - i;
            }

            for (int j = 0; j < randomNumberOfPlatformsInARow; j++)
            {
                if (numberOfPlatformsInARowLowerThanFive)
                {
                    pool2.platformPool[i].gameObject.SetActive(false);
                }
                if (!pool2.platformPool[i].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
                {
                    pool2.platformPool[i].gameObject.SetActive(true);
                }
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
        timeTunnelCreationChance = Random.Range(0, 11);
        if (timeTunnelCreationChance < 11)
        {
            if (!timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].gameObject.activeInHierarchy)
            {
                timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].gameObject.SetActive(true);
            }
            currentPosition.y = Random.Range(-5, 15);
            timeTunnelObjectPool.tunnelPool[timeTunnelPoolCounter].transform.position = currentPosition;
            timeTunnelPoolCounter++;
            if (timeTunnelPoolCounter >= timeTunnelObjectPool.poolLimit)
            {
                timeTunnelPoolCounter = 0;
            }
        }
        for (int i = 0; i < pool1.poolLimit; i++)
        {
            //This makes sure that at least 5 platforms are spawned in a single segment
            //Terrible performance, needs rework
            //if (i + 5 > pool1.poolLimit)
            //{
            //    pool1.poolLimit += (i + 5) - pool1.poolLimit;
            //    for (int j = 0; j < pool1.poolLimit; j++)
            //    {
            //        pool1.platformPool.Add(Instantiate(platformPrefabs[Random.Range(0, 2)], new Vector2(0, -500), spawnRotation));
            //    }
            //}
            if (i + 5 > pool2.poolLimit)
            {
                numberOfPlatformsInARowLowerThanFive = true;
            }
            else
            {
                numberOfPlatformsInARowLowerThanFive = false;
            }
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

            //if (UtilityMethods.IsIntegerInRangeInclusive(randomY, lastRandomY - 5, randomY + 5))
            //{
            //    if (randomY >= lastRandomY)
            //    {
            //        randomY -= 5;
            //    }
            //    if (randomY < lastRandomY)
            //    {
            //        randomY += 5;
            //    }
            //}

            lastRandomY = randomY;
            if (randomY < -5 || randomY > 15)
            {
                lastRandomY = Random.Range(5, 11);
            }
            randomNumberOfPlatformsInARow = Random.Range(5, 20);
            currentPosition.y = randomY;
            if (numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(false);
            }
            if (!starts.platformPool[startsCounter].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(true);
            }
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
                if (numberOfPlatformsInARowLowerThanFive)
                {
                    pool2.platformPool[i].gameObject.SetActive(false);
                }
                if (!pool2.platformPool[i].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
                {
                    pool2.platformPool[i].gameObject.SetActive(true);
                }
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
