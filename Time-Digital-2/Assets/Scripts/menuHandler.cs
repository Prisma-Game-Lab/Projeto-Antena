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
        // Verificar se existe load
        PlayerInfo playerInfo = SaveSystem.LoadGame();
        if (playerInfo != null)
        {
            //VOCE TEM CERTEZA QUE QUR COMECAR UM NOVO DESAFIO, VOCE PERDERA OS SEU ULTIMO PROGRESSO
            //Y/N
            // aparecer o prompt 
         
        }
        SceneManager.LoadScene(1);
    }
    public void LoadScene()
    {
        PlayerInfo playerInfo = SaveSystem.LoadGame();
        if (playerInfo == null)
        {
            //VOCE NÃO POSSUI DADOS SALVOS
        }else
        {

            SceneManager.LoadScene(1);
        }
        
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

    public void NewSaveGame()
    {
        Player newPlayer = new Player();
        SaveSystem.SaveGame(newPlayer);
    }
}
