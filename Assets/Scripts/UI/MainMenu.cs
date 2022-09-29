using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject controlsPanel;
    public GameObject mainMenuPanel;

    const string gameSceneName = "GameScene";

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Controls()
    {
        controlsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void BackButton()
    {
        controlsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
