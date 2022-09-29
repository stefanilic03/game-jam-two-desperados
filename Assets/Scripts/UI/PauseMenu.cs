using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Player player;
    public GameObject pauseMenuPanel;

    const string PlayerActionMapName = "PlayerActions";

    public void Resume()
    {
        Time.timeScale = 1f;

        player.gamePaused = false;
        pauseMenuPanel.SetActive(false);

        player.playerInput.SwitchCurrentActionMap(PlayerActionMapName);
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        player.gamePaused = false;
        pauseMenuPanel.SetActive(false);

        player.playerInput.SwitchCurrentActionMap(PlayerActionMapName);

        //Reset the score if it's a new run
        SetGameSettingsForRetry();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetGameSettingsForRetry()
    {
        GameMaster.currentScore = 0;
        GameMaster.difficultyMultiplier = 1;
        GameMaster.gameMaster.gameIsOn = true;
        GameMaster.endGameLocation = Random.Range(3500, 7000);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
