using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuHandler : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject hasSavedGameAlert;

    private void Start() {
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Menu);
    }
   
    public void LoadScene()
    {
        PlayerInfo playerInfo = SaveSystem.LoadGame();
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Play);

        if (playerInfo == null)
        {
            //VOCE NÃO POSSUI DADOS SALVOS
        }else
        {
            SceneManager.LoadScene(2);
        }
        
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
        AudioManager.sharedInstance.UISelect();
        print("Quitting...");
        Application.Quit();
    }
    public void back()
    {
        AudioManager.sharedInstance.UIBack();
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CheckSavedGame()
    {
        if (SaveSystem.HaveSavedGame())
        {
            hasSavedGameAlert.SetActive(true);
        }
        else
        {
            hasSavedGameAlert.SetActive(false);
            NewSaveGame();
            
        }
    }

    public void NewSaveGame()
    {
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Play);
        Player newPlayer = new Player();
        SaveSystem.SaveGame(newPlayer);
        SceneManager.LoadScene(2);
        
    }

    public void DisableSavedGameAlert()
    {
        hasSavedGameAlert.SetActive(false);
    }
}
