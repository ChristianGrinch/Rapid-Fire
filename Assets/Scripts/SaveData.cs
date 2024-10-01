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

    // Parameterless constructor
    public SaveData() { }

    // Factory method to create SaveData from PlayerController
    public static SaveData CreateFromPlayer(PlayerController player)
    {
        SaveData saveData = new SaveData
        {
            // Assign player data
            exp = player.exp,
            health = player.health,
            lives = player.lives,
            position = new float[3]
            {
                player.transform.position.x,
                player.transform.position.y,
                player.transform.position.z
            },
            ammo = player.ammo,

            // Assign game data
            wave = player.wave,
            numberOfEnemies = new int[4]
            {
                EnemySpawnManager.Instance.enemyLevel1.Length,
                EnemySpawnManager.Instance.enemyLevel2.Length,
                EnemySpawnManager.Instance.enemyLevel3.Length,
                EnemySpawnManager.Instance.bossLevel1.Length
            },
            numberofPowerups = new int[3]
            {
                PowerupManager.Instance.ammunition,
                PowerupManager.Instance.heartPowerups,
                PowerupManager.Instance.speedPowerups
            },
            difficulty = UIManager.Instance.difficulty,

            // Assign settings data
            masterVolume = (int)UIManager.Instance.masterVolumeSlider.value
        };
        return saveData;
    }
}
