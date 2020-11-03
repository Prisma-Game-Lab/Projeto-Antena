using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneController : MonoBehaviour
{

    public static bool gameIsPause = false;
    public GameObject pauseMenuUI;
    public Scene currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPause)
            {
                //Trava e deixa o cursor invisivel
                //Cursor.lockState = CursorLockMode.Locked;
                Resume();
            } else
            {
                //Destrava e deixa o cursor visivel
                //Cursor.lockState = CursorLockMode.None;
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPause = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPause = false;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
