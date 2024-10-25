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
		}
		else
		{
			Destroy(gameObject);
		}
	}
	[Header("Game Values")]
	public static float mapSize = 50;
	public int difficulty = 1;
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

    // References
    private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	private GameObject player;

    private void Start()
	{
		enemyCount = new List<int>(new int[EnemyDataManager.Instance.enemies.Length]);

		defaultSave = SaveSystem.LoadDefaultSave();
		player = GameObject.FindWithTag("Player");

		healthSystem = player.GetComponent<HealthSystem>();
		gunController = player.GetComponent<GunController>();
		playerController = player.GetComponent<PlayerController>();
		enemySpawnManager = this.GetComponentInParent<EnemySpawnManager>();
	}
    public void GameOver()
	{
		RestartMenuUI.Instance.ShowRestartMenu();
		isGameUnpaused = false;
	}
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isInGame = false;
        isGameUnpaused = false;
	}
	public void StartGame()
	{
		LoadPlayer(defaultSave);

		UIManager.Instance.CloseAllMenus();
        GameMenuUI.Instance.game.SetActive(true);

		isGameUnpaused = true;
		isInGame = true;

		Time.timeScale = 1;
		GameMenuUI.Instance.SetDifficultyText();

	}
	public void StartNewGame()
	{
		UIManager.Instance.CloseAllMenus();
        GameMenuUI.Instance.game.SetActive(true);

		isGameUnpaused = true;
		isInGame = true;

		healthSystem.AssignLives();
		Time.timeScale = 1;
        GameMenuUI.Instance.SetDifficultyText();
		LoadPlayer(currentSave);
	}
    public void StartExistingGame(){
        UIManager.Instance.CloseAllMenus();
        GameMenuUI.Instance.game.SetActive(true);

        isGameUnpaused = true;
        isInGame = true;

        Time.timeScale = 1;
        GameMenuUI.Instance.SetDifficultyText();
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
        if (!string.IsNullOrEmpty(currentSave))
        {
            SaveSystem.DeleteSave(currentSave);
        }
        else
        {
            Debug.LogWarning("Save name cannot be empty!");
        }
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
    public void LoadPlayer(string saveName)
    {
        currentSave = saveName;

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
            playerController.wave = data.wave;
            playerController.ammo = data.ammo;
			playerController.speedPowerupCount = data.speedPowerup;

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
			if(data.numberOfEnemies.Length == 4) // Handling for no iceZombie
			{
                Debug.LogWarning("Ran save incompatiblity fixer [ICE ZOMBIE]");
				int[] tempEnemies = new int[5];
				for (int i = 0; i <= 4; i++)
				{
					tempEnemies[i] = data.numberOfEnemies[i];
				}

                Debug.Log("Data difficulty: " + data.difficulty);

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

			for(var i = 0; i < data.numberOfEnemies.Length; i++)
			{
				enemyCount[i] = data.numberOfEnemies[i];
				Debug.Log(enemyCount[i]);
			}

            PowerupManager.Instance.ammunition = data.numberOfPowerups[0];
            PowerupManager.Instance.heartPowerups = data.numberOfPowerups[1];
            PowerupManager.Instance.speedPowerups = data.numberOfPowerups[2];

            // Update settings data
            AudioPanelUI.Instance.masterVolume.value = data.masterVolume;
            AudioPanelUI.Instance.master.text = data.masterVolume.ToString();

            AudioPanelUI.Instance.musicVolume.value = data.musicVolume;
            AudioPanelUI.Instance.music.text = data.musicVolume.ToString();

            AudioPanelUI.Instance.gunVolume.value = data.gunVolume;
            AudioPanelUI.Instance.gun.text = data.musicVolume.ToString();
			playerController.useSprintHold = data.useSprintHold;

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
}
