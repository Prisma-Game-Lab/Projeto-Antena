using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameFromSave : MonoBehaviour
{
    public GameObject player;
    public GameObject[] checkpoints;

    private void Awake()
    {
        PlayerInfo playerInfo = SaveSystem.LoadGame();
        if (playerInfo == null)
        {
            Player newPlayer = new Player();
            SaveSystem.SaveGame(newPlayer);
        }
    }

    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        PlayerInfo playerInfo = SaveSystem.LoadGame();
        if(playerInfo.checkpointsCount != 0)
        {
            Vector3 checkpointPosition = new Vector3(playerInfo.position[0], playerInfo.position[1], playerInfo.position[2]);
            player.transform.position = checkpointPosition;
        }
        StartCoroutine(EnableMovement());

    }

    IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(0.01f);
        player.GetComponent<playerMovement>().enableMovement = true;
    }
}
