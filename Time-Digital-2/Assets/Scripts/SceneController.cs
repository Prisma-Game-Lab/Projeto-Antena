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

    private playerMovement player;

    void Start()
    {
        player = playerMovement.current;
        gameIsPause = false;
        currentScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPause)
            {
                //Trava e deixa o cursor invisivel
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Resume();
            }
            else
            {
                //Destrava e deixa o cursor visivel
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
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
        AudioManager.sharedInstance.UISelect();
        pauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPause = false;
        doorSounds.resumeAlarme();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadScene(int sceneName)
    {
        AudioManager.sharedInstance.UISelect();
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        AudioManager.sharedInstance.UIBack();
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void OpenSettingsMenu(GameObject menuToDisable)
    {
        AudioManager.sharedInstance.UISelect();
        settingsMenu.SetActive(true);
        menuToDisable.SetActive(false);
    }

    public void OpenMenu(GameObject menuToEnable)
    {
        AudioManager.sharedInstance.UISelect();
        settingsMenu.SetActive(false);
        menuToEnable.SetActive(true);
    }
    public void back()
    {
        AudioManager.sharedInstance.UIBack();
        settingsMenu.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
