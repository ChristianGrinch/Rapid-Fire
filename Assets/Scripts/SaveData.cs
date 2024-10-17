using UnityEngine;
using MessagePack;
using UnityEngine.Rendering;

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
	public static SaveData AssignData(PlayerController player)
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
			numberOfEnemies = new int[]
			{
				EnemySpawnManager.Instance.level1Enemies.Count,
				EnemySpawnManager.Instance.level2Enemies.Count,
				EnemySpawnManager.Instance.level3Enemies.Count,
				EnemySpawnManager.Instance.boss1Enemies.Count,
				EnemySpawnManager.Instance.iceZombie.Count,
			},
			numberofPowerups = new int[]
			{
				PowerupManager.Instance.ammunition,
				PowerupManager.Instance.heartPowerups,
				PowerupManager.Instance.speedPowerups
			},
			difficulty = GameManager.Instance.difficulty,

			// Assign settings data
			masterVolume = (int)UIManager.Instance.masterVolumeSlider.value,
			musicVolume = (int)UIManager.Instance.musicVolumeSlider.value,
			gunVolume = (int)UIManager.Instance.gunVolumeSlider.value,
		};
		return saveData;
	}

	public static SaveData CreateDefaultData(PlayerController player, int difficulty)
	{
		if(difficulty == 0)
		{
			difficulty = 1; // Sets to easy mode by default
		}

		SaveData saveData = new SaveData();

		// Assign player data
		saveData.exp = 0;
		saveData.health = 100;
        switch (difficulty)
        {
            case 1: // Easy
                saveData.lives = 3;
                break;
            case 2: // Normal
                saveData.lives = 2;
                break;
            case 3: // Master
                saveData.lives = 1;
                break;
            default:
				Debug.LogError("Location SaveData -- Difficulty is 0 or null!");
                break;
        }
        saveData.position = new float[3] { 0, 0.5f, 0};
		saveData.ammo = new[] { 30, 50 };

		// Assign game data
		saveData.wave = 0; //TEST: this might lowk break something like it did at school when theriault was tryna help
		saveData.numberOfEnemies = new int[4];
        switch (difficulty)
		{
			case 1:
				saveData.numberOfEnemies[0] = 4;
				saveData.numberOfEnemies[1] = 2;
				saveData.numberOfEnemies[2] = 0;
				saveData.numberOfEnemies[3] = 0;
				break;
			case 2:
				saveData.numberOfEnemies[0] = 6;
				saveData.numberOfEnemies[1] = 2;
				saveData.numberOfEnemies[2] = 1;
				saveData.numberOfEnemies[3] = 0;
				break;
			case 3:
				saveData.numberOfEnemies[0] = 8;
				saveData.numberOfEnemies[1] = 4;
				saveData.numberOfEnemies[2] = 3;
				saveData.numberOfEnemies[3] = 2;
				break;
		}
		saveData.numberofPowerups = new[] {0, 0, 0};
		saveData.difficulty = difficulty;

		// Assign settings data
		saveData.masterVolume = 50;
        saveData.musicVolume = 50;
        saveData.gunVolume = 30;

        return saveData;
    }
}
