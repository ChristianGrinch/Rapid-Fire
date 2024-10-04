using UnityEngine;
using System.IO;
using MessagePack;
using System.Collections.Generic;
using Unity.VisualScripting;
using static UnityEditorInternal.ReorderableList;

// .svf for SaVeFile
// .dsvf for Default SaVeFile

public static class SaveSystem
{
	public static void SavePlayer(PlayerController player, string saveName)
	{
		SaveData saveData = SaveData.CreateFromPlayer(player);
		byte[] bytes = MessagePackSerializer.Serialize(saveData);
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");


        File.WriteAllBytes(path, bytes);
		Debug.Log("Saved file with length: " + bytes.Length + " bytes.");
	}

	public static SaveData LoadPlayer(string saveName)
	{
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");


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

    public static List<string> FindSaves()
    {
        string path = Application.persistentDataPath;
        //Debug.Log($"Searching in path: {path}");
        string[] files = Directory.GetFiles(path, "*.svf");

        //Debug.Log($"Files found: {files.Length}");

        if (files.Length == 0)
        {
            Debug.Log("No save files found.");
            return new List<string>();
        }

        List<string> saveFileNames = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            string filePath = files[i];
            if (File.Exists(filePath))
            {
                string saveName = Path.GetFileNameWithoutExtension(filePath);
                saveFileNames.Add(saveName);

                Debug.Log("Found save file: " + saveName);
            }
        }

        return saveFileNames;
    }

    public static void DeleteSave(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogError("File does not exist! Cannot delete a nonexistent file.");
        }
    }

    public static void SetDefaultSave(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, "Default" + ".dsvf"); // Default SaVeFile

        byte[] bytes = MessagePackSerializer.Serialize(saveName);

        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved Default save name to unique save file.");
    }
    public static string LoadDefaultSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "Default" + ".dsvf");

        if (File.Exists(path))
        {
            byte[] readBytes = File.ReadAllBytes(path);
            string defaultSaveName = MessagePackSerializer.Deserialize<string>(readBytes);
            Debug.Log("Loaded Default save file");
            return defaultSaveName;
        }

        Debug.LogWarning("No default save assigned!");
        return null;
    }
}

