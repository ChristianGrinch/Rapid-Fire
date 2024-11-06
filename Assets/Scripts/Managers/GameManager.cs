using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
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
	[Header("Game Values")]
	public static float mapSize = 50;
	public int difficulty = 1;
	public int wave = 0;
	public bool isGameUnpaused = false;
	public bool isInGame = false;
	[Header("Saves")]
	public string defaultSave;
	public string currentSave;
	[Header("Enemy Counts")]
	public List<int> enemyCount;
	[Header("Checks")]
	public bool didSelectDifficulty = false;
	public bool didLoadSpawnManager = false;
	public bool didLoadPowerupManager = false;
	[Header("Instantiated Objects")]
	public GameObject instantiatedObjects;
	public GameObject enemies;
	public GameObject bullets;
	public GameObject powerups;
	public GameObject ammo;
	[Header("Tertiary stuff")]
	public bool useSprintHold = true;

	public List<EnemyType> savedEnemiesTypes;
	public List<Vector3> savedEnemiesPositions;
	public List<int> savedEnemiesHealths;
	

	[Header("Other")]
	// References
	public GameObject player; // Scripts will reference the player HERE instead of DIRECTLY referencing the player
	public HealthSystem playerHealthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	public int saveInterval;
	public bool isRunningSaveInterval;
	private void Start()
	{
		//enemyCount = new List<int>(new int[EnemyDataManager.Instance.enemies.Length]);

		defaultSave = SaveSystem.LoadDefaultSave();
		
		StartCoroutine(DelayedLoadSettings()); 
		SettingsMenuUI.Instance.didModifySettings = false;
		
	}
	private void Update()
	{
		if (saveInterval != 0 && !isRunningSaveInterval && isInGame)
		{
			StartCoroutine(WaitIntervalSave(saveInterval));
		}
	}
	public void GameOver()
	{
		RestartMenuUI.Instance.ShowRestartMenu();
		isGameUnpaused = false;
	}
	public void RestartGame()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;

		SceneManager.LoadScene(0);
		UIManager.Instance.CloseAllMenus();

		isInGame = false;
        isGameUnpaused = false;
	}
	public void StartDefaultGame()
	{
		LoadPlayer(defaultSave);

		isGameUnpaused = true;
		isInGame = true;

		Time.timeScale = 1;
		Debug.Log(difficulty);
	}
	public void StartNewGame()
	{
		LoadPlayer(currentSave);

		isGameUnpaused = true;
		isInGame = true;

		Time.timeScale = 1;
		Debug.Log(difficulty);
	}
    public void StartExistingGame()
	{
		UIManager.Instance.CloseAllMenus();
		LoadPlayer(SavesPanelUI.Instance.currentSave);

		isGameUnpaused = true;
        isInGame = true;

        Time.timeScale = 1;
		Debug.Log(difficulty);
	}
	public void PauseGame()
	{
		isGameUnpaused = false;
		PauseMenuUI.Instance.pauseMenu.SetActive(true);
		Time.timeScale = 0;
	    PauseMenuUI.Instance.saveGame.GetComponentInChildren<TMP_Text>().text = $"Save current game ({currentSave})";
	}
	public void ResumeGame()
	{
		isGameUnpaused = true;
		PauseMenuUI.Instance.pauseMenu.SetActive(false);
        GameMenuUI.Instance.game.SetActive(true);
		Time.timeScale = 1;
	}
	public void QuitGame()
	{
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}
    public void SetDefaultSave()
    {
        StartMenuUI.Instance.playDefaultText.text = "Play default save \n[ " + currentSave + " ]";
        if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
        {
            SaveSystem.SetDefaultSave(currentSave);
            Debug.Log("Set '" + currentSave + "' to default save.");	
            defaultSave = SaveSystem.LoadDefaultSave();
            SavesPanelUI.Instance.defaultSave = defaultSave;
        }
    }
    public void DeleteSave()
    {
		currentSave = SavesPanelUI.Instance.currentSave;

		if (!string.IsNullOrEmpty(currentSave))
        {
            SaveSystem.DeleteSave(currentSave);
        }
        else
        {
            Debug.LogWarning("Save name cannot be empty!");
        }
		currentSave = "";

	}
	IEnumerator WaitIntervalSave(int interval)
	{
		isRunningSaveInterval = true;
		Debug.LogWarning("INTERVAL AUTOSAVE RUNNING. Interval is: " + interval);
		yield return new WaitForSeconds(interval * 60);
		Debug.LogWarning("INTERVAL AUTOSAVE DONE. Interval is: " + interval + ". Saving.");
		if (isInGame)
		{
			SaveGame(currentSave);
		}
		else
		{
			Debug.LogWarning("Aborting interval save! Player not in game");
		}
		
		isRunningSaveInterval = false;
	}
    public void SaveGame(string saveName)
    {
        SaveSystem.SaveGame(playerController, saveName);
    }
    public void CreateSave(string saveName)
    {
        SaveSystem.CreateSave(playerController, saveName);
        currentSave = saveName;
    }
	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex == 1)  // Ensure it’s the correct scene
		{
			// Scene is now fully loaded; access new scene objects here

			player = GameObject.FindWithTag("Player");

			playerController = player.GetComponent<PlayerController>();
			playerHealthSystem = player.GetComponent<HealthSystem>();
			gunController = player.GetComponentInParent<GunController>();
			enemySpawnManager = GameObject.Find("Enemy Manager").GetComponent<EnemySpawnManager>();
			enemyCount = new List<int>(new int[EnemyDataManager.Instance.enemies.Length]);
			saveInterval = SavesPanelUI.Instance.saveInterval;

			AudioManager.Instance.player = player;

			instantiatedObjects = GameObject.Find("Instantiated Objects");
			enemies = instantiatedObjects.transform.Find("Enemies").gameObject;
			bullets = instantiatedObjects.transform.Find("Bullets").gameObject;
			powerups = instantiatedObjects.transform.Find("Powerups").gameObject;
			ammo = instantiatedObjects.transform.Find("Ammo Piles").gameObject;

			Debug.Log("loading player...");

			// Now that the scene is loaded, initialize and load player data
			InitializePlayerData(currentSave);
			GameMenuUI.Instance.SetDifficultyText();
			StartCoroutine(DelayedLoadSettings());

			SceneManager.sceneLoaded -= OnSceneLoaded;  // Unsubscribe from the event
		} 
		else if (scene.buildIndex == 0)
		{
			StartCoroutine(DelayedLoadSettings());
			difficulty = 1;
			didSelectDifficulty = false;
			didLoadSpawnManager = false;
			didLoadPowerupManager = false;
			SavesPanelUI.Instance.InstantiateSaveButtons();
			Debug.Log("build index was 0");

			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
	}

	public void LoadPlayer(string saveName)
	{
		currentSave = saveName;  // Store save name for later use

		// Subscribe to sceneLoaded event
		SceneManager.sceneLoaded += OnSceneLoaded;

		// Start loading the scene
		SceneManager.LoadScene(1);
	}

	private void InitializePlayerData(string saveName)
	{
		didLoadSpawnManager = true;
		didLoadPowerupManager = true;

		// Load the player data
		SaveData data = SaveSystem.LoadGame(saveName);

		if (data != null)
		{
			// Update player data
			playerController.exp = data.exp;
			playerController.health = data.health;
			playerController.lives = data.lives;
			wave = data.wave;
			playerController.ammo = data.ammo;
			playerController.speedPowerupCount = data.speedPowerup;

			Vector3 position;
			position.x = data.position[0];
			position.y = data.position[1];
			position.z = data.position[2];
			player.transform.position = position;

			playerHealthSystem.UpdateHealth(data.health);
			playerHealthSystem.UpdateLives(data.lives);

			// Update game data
			enemySpawnManager.currentWave = data.wave;
			gunController.ammo = data.ammo;

			// Check if the save is an old save and modify for compatibility
			if (data.numberOfEnemies.Length == 4)
			{
				Debug.LogWarning("Ran save incompatibility fixer [ICE ZOMBIE]");
				int[] tempEnemies = new int[5];
				for (int i = 0; i <= 3; i++)
				{
					tempEnemies[i] = data.numberOfEnemies[i];
				}

				// Set ice zombies based on difficulty and wave
				switch (data.difficulty)
				{
					case 1:
						tempEnemies[4] = data.wave - 4 > 0 ? data.wave - 3 : 0;
						break;
					case 2:
						tempEnemies[4] = data.wave - 2 > 0 ? data.wave - 2 : 0;
						break;
					case 3:
						tempEnemies[4] = data.wave + 1;
						break;
				}
				data.numberOfEnemies = tempEnemies;
			}

			// Set enemy counts
			//for (var i = 0; i < data.numberOfEnemies.Length; i++)
			//{
			//	enemyCount[i] = data.numberOfEnemies[i];
			//}
			// Set enemy data
			savedEnemiesTypes = data.enemyTypes;
			for(var i = 0; i < data.enemyPositions.Count; i++)
			{
				var x = data.enemyPositions[i][0];
				var y = data.enemyPositions[i][0];
				var z = data.enemyPositions[i][0];
				savedEnemiesPositions.Add(ConvertFloatsToVector3(x, y, z));
			}
			savedEnemiesHealths = data.enemyHealths;

			// Set powerup counts
			PowerupManager.Instance.ammunition = data.numberOfPowerups[0];
			PowerupManager.Instance.heartPowerups = data.numberOfPowerups[1];
			PowerupManager.Instance.speedPowerups = data.numberOfPowerups[2];

			if (data.difficulty != 0)
			{
				didSelectDifficulty = true;
				difficulty = data.difficulty;
			}
		}
		else
		{
			Debug.LogError("Data is null.");
		}
	}
	public Vector3 ConvertFloatsToVector3(float x, float y, float z)
	{
		Vector3 position = new()
		{
			x = x,
			y = y,
			z = z
		};
		return position;
	}
	public void SaveSettings()
	{
		SaveSystem.SaveSettings();
	}
	private IEnumerator DelayedLoadSettings()
	{
		yield return null; // Wait one frame for UI elements to initialize
		LoadSettings(); // Now the UI components should be ready
	}
	public void LoadSettings()
	{
		
		SaveData data = SaveSystem.LoadSettings();

		if (data != null)
		{
			Debug.Log("data not null.");
			AudioPanelUI.Instance.masterVolume.value = data.masterVolume;
			AudioPanelUI.Instance.master.text = data.masterVolume.ToString();

			AudioPanelUI.Instance.musicVolume.value = data.musicVolume;
			AudioPanelUI.Instance.music.text = data.musicVolume.ToString();

			AudioPanelUI.Instance.gunVolume.value = data.gunVolume;
			AudioPanelUI.Instance.gun.text = data.gunVolume.ToString();
			useSprintHold = data.useSprintHold;

			VideoPanelUI.Instance.screenMode.value = data.screenMode;
			VideoPanelUI.Instance.ChangeScreenMode(data.screenMode);
			if (data.useSprintHold)
			{
				ControlsPanelUI.Instance.sprintMode.value = 0;
			}
			else if (!data.useSprintHold)
			{
				ControlsPanelUI.Instance.sprintMode.value = 1;
			}
			SavesPanelUI.Instance.saveInterval = data.autoSaveInterval;
			SavesPanelUI.Instance.autoSaveIntervalDropdown.onValueChanged.RemoveAllListeners();
			SavesPanelUI.Instance.autoSaveIntervalDropdown.value = data.autoSaveInterval;
			SavesPanelUI.Instance.autoSaveIntervalDropdown.onValueChanged.AddListener((int value) =>
			{
				SavesPanelUI.Instance.saveInterval = value;
				SettingsMenuUI.Instance.didModifySettings = true;
			});

			SavesPanelUI.Instance.onExitSave = data.autoSaveOnExit;
			SavesPanelUI.Instance.autoSaveOnExitToggle.onValueChanged.RemoveAllListeners();
			SavesPanelUI.Instance.autoSaveOnExitToggle.isOn = data.autoSaveOnExit;
			SavesPanelUI.Instance.autoSaveOnExitToggle.onValueChanged.AddListener((bool value) =>
			{
				SavesPanelUI.Instance.onExitSave = value;
				SettingsMenuUI.Instance.didModifySettings = true;
			});
		}
		else
		{
			Debug.Log("data null.");
			SaveSystem.CreateSaveSettings();
			LoadSettings();
		}
		SettingsMenuUI.Instance.didModifySettings = false;
	}
}
