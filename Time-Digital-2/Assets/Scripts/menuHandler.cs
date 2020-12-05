using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Credits()
    {

    }
    public void Exit()
    {
        print("Quitting...");
        Application.Quit();
    }
    public void back()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
