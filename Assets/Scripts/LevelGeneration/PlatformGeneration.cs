using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformGeneration : MonoBehaviour
{
    //How this script works:
    //Platforms get spawned in blocks of 80, which is the platform object pool limit
    //There are 2 pools for platforms because the player is always running on one of them, so the other can be moved
    //When the player is in the middle of a given block, the previous block will be moved in front of the current block
    //This way we don't reserve additional memory for instantiating objects and the game can truly be endless
    //This is true for every other script with random object generation
    //There are also time tunnels which spawn randomly in random locations
    //After reaching a certain point, a portal spawns which gives the player a choice to end the run and record the high score in PlayerPrefs, or restart the run, reset the high score and in the next run, gain score twice as fast

    ObjectPooling pool1;
    ObjectPooling pool2;
    ObjectPooling starts;
    TimeTunnelObjectPool timeTunnelObjectPool;

    public List<Platform> platformPrefabs;
    public List<TimeTunnel> timeTunnelPrefabs;

    public Transform playerLocation;
    float currentXPlayer;

    Vector3 currentPosition;
    Quaternion spawnRotation;

    bool currentlySpawning = false;

    int randomY;
    int lastRandomY = 5;
    int randomNumberOfPlatformsInARow;
    bool numberOfPlatformsInARowLowerThanFive = false;

    int startsCounter;

    public Transform timePortal;
    int timeTunnelCreationChance;
    int timeTunnelPoolCounter = 0;

    int endGameLocation = 5000;

    private void Start()
    {
        currentPosition = transform.position;
        spawnRotation = new Quaternion(0, 0, 0, 0);

        pool1 = new ObjectPooling();
        pool2 = new ObjectPooling();
        starts = new ObjectPooling();
        timeTunnelObjectPool = new TimeTunnelObjectPool();

        //Fill up time tunnel pool
        for (int i = 0; i < timeTunnelObjectPool.poolLimit; i++)
        {
            timeTunnelObjectPool.tunnelPool.Add(Instantiate(timeTunnelPrefabs[Random.Range(0, 2)], new Vector2(0, -100), spawnRotation));
            timeTunnelObjectPool.tunnelPool[i].gameObject.SetActive(false);
        }

        //Fill up first platforms in a row, or "starts" pool
        for (int i = 0; i < starts.poolLimit; i++)
        {
            starts.platformPool.Add(Instantiate(platformPrefabs[3], new Vector2(0, -100), spawnRotation));
            starts.platformPool[i].gameObject.SetActive(false);
        }

        //Fill up platform pool 1
        for (int i = 0; i < pool1.poolLimit; i++)
        {
            pool1.platformPool.Add(Instantiate(platformPrefabs[Random.Range(0, 2)], currentPosition, spawnRotation));
            currentPosition.x++;
        }

        pool1.middleOfABlock = (pool1.platformPool[pool1.poolLimit - 1].transform.position.x + pool1.platformPool[0].transform.position.x) / 2;

        //Fill up platform pool 2
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
            GenerateNextBlock(pool2);
        }
        if (currentXPlayer >= pool2.middleOfABlock && !currentlySpawning && pool2.middleOfABlock > pool1.middleOfABlock)
        {
            currentlySpawning = true;
            GenerateNextBlock(pool1);
        }
    }

    void GenerateNextBlock(ObjectPooling pool)
    {
        TimeTunnelGeneration();
        for (int i = 0; i < pool.poolLimit; i++)
        {
            //Make sure that there are no less than 5 platforms in a row
            if (i + 5 > pool.poolLimit)
            {
                numberOfPlatformsInARowLowerThanFive = true;
            }
            else
            {
                numberOfPlatformsInARowLowerThanFive = false;
            }

            //Choose random Y spawn coordinate and make sure that the next Y is more than 5, but less than 11 units apart from the lastRandomY -> next Y must be in lastRandomY +- 6 range
            //This is important so the platforms don't overlap and make the player get stuck somewhere and not able to move forward
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
            if (randomY < -5 || randomY > 15)
            {
                lastRandomY = Random.Range(5, 11);
            }

            //Random number of platforms in a segment
            randomNumberOfPlatformsInARow = Random.Range(5, 20);
            currentPosition.y = randomY;

            //Make sure that there are no less than 5 platforms in a row and enable the segments that are valid
            if (numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(false);
            }
            if (!starts.platformPool[startsCounter].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
            {
                starts.platformPool[startsCounter].gameObject.SetActive(true);
            }

            //Set the position of the first platform
            starts.platformPool[startsCounter].transform.position = currentPosition;
            startsCounter++;
            if (startsCounter >= starts.poolLimit)
            {
                startsCounter = 0;
            }
            //Update the position so the next platform is placed one unit to the right
            currentPosition.x++;

            //If there should be more than 5 platforms in a row, but the pool would be overflown, limit the number of platforms to pool limit
            //These platforms get disabled anyway, but it's important that they are placed because of the age of the platform in the object pool
            //If you remove this, some segments will have empty platforms
            if (i + randomNumberOfPlatformsInARow > pool.poolLimit)
            {
                randomNumberOfPlatformsInARow = pool.poolLimit - i;
            }

            //Place platforms in a row on a random Y
            for (int j = 0; j < randomNumberOfPlatformsInARow; j++)
            {
                if (numberOfPlatformsInARowLowerThanFive)
                {
                    pool.platformPool[i].gameObject.SetActive(false);
                }
                if (!pool.platformPool[i].gameObject.activeInHierarchy && !numberOfPlatformsInARowLowerThanFive)
                {
                    pool.platformPool[i].gameObject.SetActive(true);
                }
                pool.platformPool[i].transform.position = currentPosition;
                currentPosition.x++;
                if (j < randomNumberOfPlatformsInARow - 1)
                {
                    //Also update "i" so the platforms won't get skipped
                    i++;
                }
            }
        }

        CheckIfEndGame();

        //Calculate the middle of the current block of platforms
        pool.middleOfABlock = (pool.platformPool[pool.poolLimit - 1].transform.position.x + pool.platformPool[0].transform.position.x) / 2;

        currentlySpawning = false;
    }

    private void TimeTunnelGeneration()
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
    }

    private void CheckIfEndGame()
    {
        if (currentPosition.x >= endGameLocation)
        {
            currentPosition.x += 15;
            currentPosition.y = 0;
            Instantiate(platformPrefabs[4], currentPosition, spawnRotation);
            currentPosition.x += 30;
            timePortal.position = currentPosition;
            this.enabled = false;
            playerLocation.GetComponent<Player>().endGame = true;
            return;
        }
    }
}
