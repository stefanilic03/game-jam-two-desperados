using TMPro;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gameMaster;

    public TMP_Text highScoreText;

    public static float currentScore = 0f;
    public float baseScoreMultiplier = 3f;
    public static float difficultyMultiplier = 1f;
    int extraOomphOnTheScore = 10;

    public bool gameIsOn = true;

    public Player player;

    public static int endGameLocation; 

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
        endGameLocation = Random.Range(3500, 7000);

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if (player != null && highScoreText != null && !player.gamePaused && gameIsOn)
        {
            currentScore += 3 * difficultyMultiplier * extraOomphOnTheScore;
            highScoreText.text = ((int)currentScore).ToString();
            return;
        }        
        SearchForPlayerAndText();
    }

    void SearchForPlayerAndText()
    {
        if (GameObject.FindGameObjectWithTag(TagsDatabase.playerTag) != null)
        {
            player = GameObject.FindGameObjectWithTag(TagsDatabase.playerTag).GetComponent<Player>();
        }
        if (GameObject.FindGameObjectWithTag(TagsDatabase.highScoreTextTag) != null)
        {
            highScoreText = GameObject.FindGameObjectWithTag(TagsDatabase.highScoreTextTag).GetComponent<TMP_Text>();
        }
    }
}
