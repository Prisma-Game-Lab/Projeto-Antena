using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject hasSavedGameAlert;
    public Button continuarButton;

    private void Start() {
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Menu);
        if (!ES3.KeyExists("posicao"))
        {
            continuarButton.interactable = false;
        }
    }


    public void LoadScene()
    {
        AudioManager.sharedInstance.UISelect();
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Play);
        SceneManager.LoadScene(2);
    }
    public void OpenSettingsMenu()
    {
        AudioManager.sharedInstance.UISelect();
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void Credits()
    {
        AudioManager.sharedInstance.UISelect();
        SceneManager.LoadScene(3);
    }
    public void Exit()
    {
        AudioManager.sharedInstance.UIBack();
        print("Quitting...");
        Application.Quit();
    }
    public void back()
    {
        AudioManager.sharedInstance.UIBack();
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void NewGame()
    {
        AudioManager.sharedInstance.UISelect();
        if (ES3.KeyExists("posicao"))
        {
            hasSavedGameAlert.SetActive(true);
        }
        else
        {
            AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Play);
            SceneManager.LoadScene(2);

        }
    }
    public void StartNewGame()
    {
        AudioManager.sharedInstance.UISelect();
        ES3.DeleteKey("posicao");
        ES3.DeleteKey("rotacao");
        ES3.DeleteKey("energia");
        SceneManager.LoadScene(2);
    }


    public void DisableSavedGameAlert()
    {
        AudioManager.sharedInstance.UIBack();
        hasSavedGameAlert.SetActive(false);
    }
}
