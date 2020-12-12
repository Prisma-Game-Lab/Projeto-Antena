using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
   public Vector3 position;
   public int checkpointsCount;

   public Player GetNewPlayer()
   {
        Player newPlayer = new Player
        {
            checkpointsCount = 0
        };

        return newPlayer;
   }


   public void SavePlayer(Player player)
   {
        SaveSystem.SaveGame(player);
   }


   public Player LoadPlayer()
   {
        PlayerInfo info = SaveSystem.LoadGame();
        Player newPlayer = new Player
        {
            checkpointsCount = info.checkpointsCount,
            position = new Vector3(info.position[0], info.position[1], info.position[2])
        };
        return newPlayer;
    }
}
