using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerInfo 
{
    public int checkpointsCount;
    public float[] position;

    public PlayerInfo(Player player)
    {
        checkpointsCount = player.checkpointsCount;

        position = new float[3];
        position[0] = player.position.x;
        position[1] = player.position.y;
        position[2] = player.position.z;
    }

}
