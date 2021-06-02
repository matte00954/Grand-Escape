//Main author: Mattias Larsson
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerVariables playerVariables, CheckpointRespawnHandler checkpointRespawnHandler, string currentLevel)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.savedata";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerVariables, checkpointRespawnHandler, currentLevel);

        formatter.Serialize(stream, data);

        stream.Close();
        if (stream.Length == 0)
            Debug.LogError("SavePlayer Stream is empty.");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.savedata";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}