using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gameMaster;

    public TMP_Text highScoreText;

    public float currentScore = 0f;
    public float difficultyMultiplier = 1f;
    int extraOomphOnTheScore = 10;

    public Transform player;

    private void Awake()
    {
        if (gameMaster is not null && gameMaster != this) 
        {
            Destroy(this);
            return;
        }
        if (gameMaster is null)
        {
            gameMaster = this;
        }
    }

    private void Update()
    {
        highScoreText.text = ((int)currentScore).ToString();

        if (player != null)
        {
            currentScore = Time.realtimeSinceStartup * difficultyMultiplier * extraOomphOnTheScore;
            return;
        }
        SearchForPlayer();
    }

    void SearchForPlayer()
    {
        if (GameObject.FindGameObjectWithTag(TagsDatabase.playerTag) != null)
        {
            player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).transform;
        }
    }
}
