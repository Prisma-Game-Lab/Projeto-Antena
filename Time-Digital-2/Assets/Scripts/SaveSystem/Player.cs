using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
   public Vector3 position;
   public int checkpointsCount;

   public Player()
   {
        checkpointsCount = 0;
        position = Vector3.zero;
   }


   public static void SavePlayer()
   {
        SaveSystem.SaveGame(this);
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
