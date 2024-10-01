using UnityEngine;
using System.IO;
using MessagePack;


public static class SaveSystem
{
    public static void SavePlayer(PlayerController player, string saveName)
    {
        SaveData saveData = SaveData.CreateFromPlayer(player);
        byte[] bytes = MessagePackSerializer.Serialize(saveData);
        string path = Application.persistentDataPath + saveName + ".savefile";

        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved file with length: " + bytes.Length + " bytes.");
    }

    public static SaveData LoadPlayer(string saveName)
    {
        string path = Application.persistentDataPath + saveName + ".savefile";

        if (File.Exists(path))
        {
            
            byte[] readBytes = File.ReadAllBytes(path);
            SaveData data = MessagePackSerializer.Deserialize<SaveData>(readBytes);

            Debug.Log("Loaded file with length: " + readBytes.Length + " bytes.");
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

