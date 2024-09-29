using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Player Data
    public int exp;
    public int health;
    public int lives;
    public float[] position;
    public int[] ammo;

    // Game data
    public int wave;
    public int[] numberOfEnemies = {0,0,0,0};

    public SaveData(PlayerController player)
    {   // This method doesn't actually save the data, SaveSystem does that.
        // this just 'assigns' the data to the public data variables for the SaveSystem script to see.

        // Assign player data
        exp = player.exp;
        health = player.health;
        lives = player.lives;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        ammo = player.ammo;

        // Assign game data
        wave = player.wave;

        numberOfEnemies[0] = EnemySpawnManager.Instance.enemyLevel1.Length;
        numberOfEnemies[1] = EnemySpawnManager.Instance.enemyLevel2.Length;
        numberOfEnemies[2] = EnemySpawnManager.Instance.enemyLevel3.Length;
        numberOfEnemies[3] = EnemySpawnManager.Instance.bossLevel1.Length;
    }
}
