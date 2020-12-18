using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneController : MonoBehaviour
{

    public bool gameIsPause;
    public GameObject pauseMenuUI;
    public Scene currentScene;
    public GameObject settingsMenu;

    void Start()
    {
        gameIsPause = false;
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
                //Cursor.visible = false;
                Resume();
            } else
            {
                //Destrava e deixa o cursor visivel
                //Cursor.visible = true;
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
        doorSounds.pauseAlarme();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPause = false;
        doorSounds.resumeAlarme();
    }

    public void LoadScene(int sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void OpenSettingsMenu(GameObject menuToDisable)
    {
        settingsMenu.SetActive(true);
        menuToDisable.SetActive(false);
    }

    public void OpenMenu(GameObject menuToEnable)
    {
        settingsMenu.SetActive(false);
        menuToEnable.SetActive(true);
    }
    public void back()
    {
        settingsMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
