using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager :MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

	[Header("References")]
	private GameObject gameManager;
	private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	private GameObject player;

	[Header("Enemies")]
    public int enemyLevel1; 
	public int enemyLevel2; 
	public int enemyLevel3; 
	public int bossLevel1; 
	public int iceZombie;

	[Header("Other")]
	public string currentSave;
	public string defaultSave;

    public int difficulty;

    public bool didLoadSpawnManager;
	public bool didLoadPowerupManager;
	public bool didSelectDifficulty;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
			DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
		defaultSave = SaveSystem.LoadDefaultSave();
    }
    public void GetReferences()
    {
        player = GameObject.FindWithTag("Player");
		//TODO: change game manager reference cuz this is stupid
		gameManager = GameManager.Instance.gameObject;
		enemySpawnManager = EnemySpawnManager.Instance.enemySpawnManager;
        if (player != null)
        {
            healthSystem = player.GetComponent<HealthSystem>();
            gunController = player.GetComponent<GunController>();
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player is null!");
        }
    }
    public void SavePlayer(string saveName)
	{
		SaveSystem.SavePlayer(playerController, saveName);
	}
	public void LoadPlayer(string saveName)
	{
		currentSave = saveName;

		didLoadSpawnManager = true;
		didLoadPowerupManager = true;

		// Load the player data
		SaveData data = SaveSystem.LoadPlayer(saveName);

		if (data != null)
		{
			// Update player data
			playerController.exp = data.exp;
			playerController.health = data.health;
			playerController.lives = data.lives;
			playerController.wave = data.wave;
			playerController.ammo = data.ammo;

			Vector3 position;
			position.x = data.position[0];
			position.y = data.position[1];
			position.z = data.position[2];
			player.transform.position = position;

			healthSystem.UpdateHealth(data.health);
			healthSystem.UpdateLives(data.lives);

			// Update game data
			enemySpawnManager.currentWave = data.wave;
			gunController.ammo = data.ammo;

			// Check if the save is an old save, if so, preform a modification to it so it can be compatible with current saves.
			if (data.numberOfEnemies.Length == 4) // Handling for no iceZombie
			{
				int[] tempEnemies = new int[5];
				for (int i = 0; i < 4; i++)
				{
					tempEnemies[i] = data.numberOfEnemies[i];
				}
				switch (data.difficulty) // Sets the current number of ice zombies based on saved difficulty and wave
				{
					case 1:
						tempEnemies[4] = data.wave - 4 > 0 ? data.wave - 3 : 0; // Spawns 1 iceZombie on wave 4, then increments
						break;
					case 2:
						tempEnemies[4] = data.wave - 2 > 0 ? data.wave - 2 : 0; // Spawns 1 iceZombie on wave 2, then increments
						break;
					case 3:
						tempEnemies[4] = data.wave + 1; // Spawns iceZombie starting at wave 1 and increments
						break;
				}
				data.numberOfEnemies = tempEnemies;
			}

			enemyLevel1 = data.numberOfEnemies[0];
			enemyLevel2 = data.numberOfEnemies[1];
			enemyLevel3 = data.numberOfEnemies[2];
			bossLevel1 = data.numberOfEnemies[3];
			iceZombie = data.numberOfEnemies[4];

			//TODO: update all script refs (powerupmanager, uimanager)
			PowerupManager.Instance.ammunition = data.numberofPowerups[0];
			PowerupManager.Instance.heartPowerups = data.numberofPowerups[1];
			PowerupManager.Instance.speedPowerups = data.numberofPowerups[2];

            // Update settings data
            SettingsMenuUI.Instance.masterVolumeSlider.value = data.masterVolume;
            SettingsMenuUI.Instance.masterVolume.text = data.masterVolume.ToString();

            SettingsMenuUI.Instance.musicVolumeSlider.value = data.musicVolume;
            SettingsMenuUI.Instance.musicVolume.text = data.musicVolume.ToString();

            SettingsMenuUI.Instance.gunVolumeSlider.value = data.gunVolume;
            SettingsMenuUI.Instance.gunVolume.text = data.musicVolume.ToString();

			if (data.difficulty != 0)
			{
				didSelectDifficulty = true;
				difficulty = data.difficulty;
			}
			else
			{
				difficulty = 1;
			}

		}
		else
		{
			Debug.LogError("Data is null.");
		}

	}
    public void DeleteSave()
    {
        if (!string.IsNullOrEmpty(currentSave))
        {
            SaveSystem.DeleteSave(currentSave);
        }
        else
        {
            Debug.LogWarning("Save name cannot be empty!");
        }
    }
    public void SetDefaultSave()
    {
        MainMenuUI.Instance.playDefaultText.text = "Play default save \n[ " + currentSave + " ]";
        if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
        {
            SaveSystem.SetDefaultSave(currentSave);
            Debug.Log("Set '" + currentSave + "' to default save.");
            defaultSave = SaveSystem.LoadDefaultSave();
        }
    }
}
