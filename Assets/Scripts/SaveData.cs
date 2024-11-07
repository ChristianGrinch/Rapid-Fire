using UnityEngine;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject]
public class SaveData
{
	// Player Data
	[Key(0)] public int exp;
	[Key(1)] public int health;
	[Key(2)] public int lives;
	[Key(3)] public float[] position;
	[Key(4)] public int[] ammo;
	[Key(5)] public int speedPowerup;

	// Game data
	[Key(10)] public int wave;
	[Key(11)] public int[] numberOfEnemies = { 0, 0, 0, 0, 0 };
	[Key(12)] public int[] numberOfPowerups = { 0, 0, 0 };
	[Key(13)] public int difficulty;
	[Key(14)] public List<List<float>> enemyPositions;
	[Key(15)] public List<EnemyType> enemyTypes;
	[Key(16)] public List<int> enemyHealths;

	// Settings data
	[Key(20)] public int masterVolume;
	[Key(21)] public int musicVolume;
	[Key(22)] public int gunVolume;
	[Key(23)] public bool useSprintHold;
	[Key(24)] public int screenMode;
	[Key(25)] public string defaultSaveName;
	[Key(26)] public int autoSaveInterval;
	[Key(27)] public bool autoSaveOnExit;

	// Parameterless constructor
	public SaveData() { }

	// Factory method to create SaveData from PlayerController
	public static SaveData AssignData(PlayerController player)
	{
		Debug.Log(EnemyDataManager.Instance.enemyCount[0]);

		EnemyData enemyData = EnemySpawnManager.Instance.GetEnemyData();

		SaveData saveData = new()
		{
			// Assign player data
			exp = player.exp,
			health = player.health,
			lives = player.lives,
			speedPowerup = player.speedPowerupCount,
			position = new float[3]
			{
				player.transform.position.x,
				player.transform.position.y,
				player.transform.position.z
			},
			ammo = player.ammo,

			// Assign game data
			wave = GameManager.Instance.wave,
			numberOfEnemies = new int[]
			{
				EnemyDataManager.Instance.enemyCount[0],
				EnemyDataManager.Instance.enemyCount[1],
				EnemyDataManager.Instance.enemyCount[2],
				EnemyDataManager.Instance.enemyCount[3],
				EnemyDataManager.Instance.enemyCount[4],
			},
			numberOfPowerups = new int[]
			{
				PowerupManager.Instance.ammunition,
				PowerupManager.Instance.heartPowerups,
				PowerupManager.Instance.speedPowerups
			},
			difficulty = GameManager.Instance.difficulty,
			enemyPositions = ConvertVector3ToFloat(enemyData.Positions),
			enemyTypes = enemyData.Types,
			enemyHealths = enemyData.Healths,
		};
		return saveData;
	}

	public static SaveData CreateDefaultData(int difficulty)
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
		saveData.speedPowerup = 0;

		// Assign game data
		saveData.wave = 1;
		saveData.numberOfEnemies = new int[5];
        switch (difficulty)
		{
			case 1:
				saveData.numberOfEnemies[0] = 4; // Enemy level 1
				saveData.numberOfEnemies[1] = 2; // Enemy level 2
				saveData.numberOfEnemies[2] = 0; // Enemy level 3
				saveData.numberOfEnemies[3] = 0; // Boss level 1
                saveData.numberOfEnemies[4] = 0; // Ice Zombie
                break;
			case 2:
				saveData.numberOfEnemies[0] = 6;
				saveData.numberOfEnemies[1] = 2;
				saveData.numberOfEnemies[2] = 1;
				saveData.numberOfEnemies[3] = 0;
                saveData.numberOfEnemies[4] = 0;
                break;
			case 3:
				saveData.numberOfEnemies[0] = 8;
				saveData.numberOfEnemies[1] = 4;
				saveData.numberOfEnemies[2] = 3;
				saveData.numberOfEnemies[3] = 0;
                saveData.numberOfEnemies[4] = 0;
                break;
		}
		saveData.numberOfPowerups = new[] {0, 0, 0};
		saveData.difficulty = difficulty;
		saveData.enemyTypes = new();
		saveData.enemyPositions = new();
		saveData.enemyHealths = new();

		return saveData;
    }

	public static SaveData AssignSettingsData()
	{
		SaveData saveData = new SaveData
		{
			masterVolume = (int)AudioPanelUI.Instance.masterVolume.value,
			musicVolume = (int)AudioPanelUI.Instance.musicVolume.value,
			gunVolume = (int)AudioPanelUI.Instance.gunVolume.value,
			useSprintHold = GameManager.Instance.useSprintHold,
			screenMode = VideoPanelUI.Instance.screenMode.value,
			autoSaveInterval = SavesPanelUI.Instance.autoSaveIntervalDropdown.value,
			autoSaveOnExit = SavesPanelUI.Instance.autoSaveOnExitToggle,

		};
		return saveData;
	}
	public static SaveData CreateDefaultSettings()
	{
		SaveData saveData = new SaveData();
		
		saveData.masterVolume = 50;
		saveData.musicVolume = 50;
		saveData.gunVolume = 30;
		saveData.useSprintHold = true;
		saveData.screenMode = 0; // Exclusive fullscreen
		saveData.autoSaveInterval = 0;
		saveData.autoSaveOnExit = false;

		return saveData;
	}
	public static List<List<float>> ConvertVector3ToFloat(List<Vector3> positions)
	{
		List<List<float>> positionsAsFloats = new();

		for(var i = 0; i < positions.Count; i++)
		{
			List<float> positionAsFloat = new()
			{
				positions[i].x,
				positions[i].y,
				positions[i].z
			};

			positionsAsFloats.Add(positionAsFloat);
		}
		return positionsAsFloats;
	}
}
