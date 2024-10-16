using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject]
public class SaveData
{
    // Player Data
    [Key(0)] public int exp;
    [Key(1)] public int health;
    [Key(2)] public int lives;
    [Key(3)] public float[] position;
    [Key(4)] public int[] ammo;

    // Game data
    [Key(10)] public int wave;
    [Key(11)] public int[] numberOfEnemies = { 0, 0, 0, 0 };
    [Key(12)] public int[] numberofPowerups = { 0, 0, 0 };
    [Key(13)] public int difficulty;

    // Settings data
    [Key(20)] public int masterVolume;
    [Key(21)] public int musicVolume;
    [Key(22)] public int gunVolume;

    // Parameterless constructor
    public SaveData() { }

    // Factory method to create SaveData from PlayerController
    public static SaveData CreateFromPlayer(PlayerController player)
    {
        SaveData saveData = new SaveData();
        // Assign player data
        if(player != null)
        {
            Debug.Log("Player null");
            saveData.exp = player.exp;
            saveData.health = player.health;
            saveData.lives = player.lives;
            saveData.position = new float[3]
            {
                player.transform.position.x,
                player.transform.position.y,
                player.transform.position.z
            };
            saveData.ammo = player.ammo;

            // Assign game data
            saveData.wave = player.wave;
        }
        else
        {
            Debug.Log("Player not null");
            int defaultHealth = 100;
            int defaultLives = 3;
            int[] defaultAmmo = { 30, 50 };

            switch (SaveManager.Instance.difficulty)
            {
                case 1:
                    Debug.Log("case 1");
                    defaultLives = 3;
                    break;
                case 2:
                    Debug.Log("case 2");
                    defaultLives = 2;
                    break;
                case 3:
                    Debug.Log("case 3");
                    defaultLives = 1;
                    break;
            }

            saveData.health = defaultHealth;
            saveData.lives = defaultLives;
            saveData.position = new float[3] { 0, 0.5f, 0 };
            saveData.ammo = defaultAmmo;

            saveData.wave = 1;
        }

        if (EnemySpawnManager.Instance != null)
        {
            saveData.numberOfEnemies = new int[]
            {
            EnemySpawnManager.Instance.level1Enemies.Count,
            EnemySpawnManager.Instance.level2Enemies.Count,
            EnemySpawnManager.Instance.level3Enemies.Count,
            EnemySpawnManager.Instance.boss1Enemies.Count,
            EnemySpawnManager.Instance.iceZombie.Count,
            };
        }
        else
        {
            switch (GameManager.Instance.difficulty)
            {
                case 1:
                    saveData.numberOfEnemies = new int[]
                    {
                        4,
                        0,
                        0,
                        0
                    };
                    break;
                case 2:
                    saveData.numberOfEnemies = new int[]
                    {
                        6,
                        2,
                        1,
                        0
                    };
                    break;
                case 3:
                    saveData.numberOfEnemies = new int[]
                    {
                        8,
                        4,
                        3,
                        2
                    };
                    break;
            }
        }
        
        if(PowerupManager.Instance != null)
        {
            saveData.numberofPowerups = new int[]
            {
            PowerupManager.Instance.ammunition,
            PowerupManager.Instance.heartPowerups,
            PowerupManager.Instance.speedPowerups
            };
        }
        else
        {
            saveData.numberofPowerups = new int[]
            {
                0,
                0,
                0
            };
        }
        
        saveData.difficulty = SaveManager.Instance.difficulty;

        // Assign settings data
        saveData.masterVolume = (int)SettingsMenuUI.Instance.masterVolumeSlider.value;
        saveData.musicVolume = (int)SettingsMenuUI.Instance.musicVolumeSlider.value;
        saveData.gunVolume = (int)SettingsMenuUI.Instance.gunVolumeSlider.value;
        return saveData;
    }
}
