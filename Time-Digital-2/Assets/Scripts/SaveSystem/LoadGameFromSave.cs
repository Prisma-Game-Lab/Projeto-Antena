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
        Vector3 checkpointPosition = checkpoints[playerInfo.checkpointsCount].gameObject.transform.position;
        Debug.Log("checkpoint pos");
        Debug.Log(checkpointPosition);
        player.transform.position = checkpointPosition;
        Debug.Log("PLAYER");
        Debug.Log(this.gameObject.transform.position);
        StartCoroutine(EnableMovement());
    }

    IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(0.01f);
        player.GetComponent<playerMovement>().enableMovement = true;
    }
}
