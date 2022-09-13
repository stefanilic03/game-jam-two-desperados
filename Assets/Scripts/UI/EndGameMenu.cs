using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenu : MonoBehaviour
{
    const string highScorePref = "HighScore";
    public TMP_Text previousHighScore;

    public void StartNewRun()
    {
        GameMaster.currentScore = 0;
        GameMaster.difficultyMultiplier *= 2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveHighScoreAndEnd()
    {
        if ((int)GameMaster.currentScore > PlayerPrefs.GetInt(highScorePref))
        {
            PlayerPrefs.SetInt(highScorePref, (int)GameMaster.currentScore);
        }
    }

    private void OnEnable()
    {
        previousHighScore.gameObject.SetActive(true);
        previousHighScore.text = "Previous high score: " + PlayerPrefs.GetInt(highScorePref).ToString();
    }
}
