using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerInfo data = new PlayerInfo(player);
        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerInfo LoadGame()
    {
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerInfo info = formatter.Deserialize(stream) as PlayerInfo;
            stream.Close();
            return info;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }

    }

    public static bool HaveSavedGame() {
        string path = Application.persistentDataPath + "/player.data";
        if(File.Exists(path))
        {
            return true;
        } else
        {
            return false;
        }
    }
}
