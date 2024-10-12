using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	// Game
	public static float mapSize = 50;
	public bool isGameUnpaused = false;
	public bool isInGame = false;
	public string defaultSave;
	public string currentSave;
	public int difficulty;
	public bool didSelectDifficulty = false;
	public int enemyLevel1; public int enemyLevel2; public int enemyLevel3; public int bossLevel1;
	public bool didLoadSpawnManager = false;
	public bool didLoadPowerupManager = false;

    // References
    private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	private GameObject player;
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
	private void Start()
	{
		defaultSave = SaveSystem.LoadDefaultSave();
		player = GameObject.FindWithTag("Player");

		healthSystem = player.GetComponent<HealthSystem>();
		gunController = player.GetComponent<GunController>();
		playerController = player.GetComponent<PlayerController>();
		enemySpawnManager = this.GetComponentInParent<EnemySpawnManager>();
	}
	public void GameOver()
	{
		UIManager.Instance.ShowRestartMenu();
		isGameUnpaused = false;
	}
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void StartGame()
	{
		LoadPlayer(defaultSave);
		UIManager.Instance.CloseAllMenus();
		UIManager.Instance.game.SetActive(true);
		isGameUnpaused = true;
		isInGame = true;

		healthSystem.AssignLives();
		Time.timeScale = 1;
		UIManager.Instance.SetDifficultyText();

	}
	public void StartNewGame()
	{
		UIManager.Instance.CloseAllMenus();
		UIManager.Instance.game.SetActive(true);
		isGameUnpaused = true;
		isInGame = true;

		healthSystem.AssignLives();
		Time.timeScale = 1;
		UIManager.Instance.SetDifficultyText();
	}
	public void PauseGame()
	{
		isGameUnpaused = false;
		UIManager.Instance.pauseMenu.SetActive(true);
		Time.timeScale = 0;
		UIManager.Instance.saveButton.GetComponentInChildren<TMP_Text>().text = $"Save current game ({currentSave})";
	}
	public void ResumeGame()
	{
		isGameUnpaused = true;
		UIManager.Instance.pauseMenu.SetActive(false);
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
        UIManager.Instance.playDefaultText.text = "Play default save \n[ " + currentSave + " ]";
        if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
        {
            SaveSystem.SetDefaultSave(currentSave);
            Debug.Log("Set '" + currentSave + "' to default save.");
            defaultSave = SaveSystem.LoadDefaultSave();
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

            enemyLevel1 = data.numberOfEnemies[0];
            enemyLevel2 = data.numberOfEnemies[1];
            enemyLevel3 = data.numberOfEnemies[2];
            bossLevel1 = data.numberOfEnemies[3];

            PowerupManager.Instance.ammunition = data.numberofPowerups[0];
            PowerupManager.Instance.heartPowerups = data.numberofPowerups[1];
            PowerupManager.Instance.speedPowerups = data.numberofPowerups[2];

            // Update settings data
            UIManager.Instance.masterVolumeSlider.value = data.masterVolume;
            UIManager.Instance.masterVolume.text = data.masterVolume.ToString();

            UIManager.Instance.musicVolumeSlider.value = data.musicVolume;
            UIManager.Instance.musicVolume.text = data.musicVolume.ToString();

            UIManager.Instance.gunVolumeSlider.value = data.gunVolume;
            UIManager.Instance.gunVolume.text = data.musicVolume.ToString();

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
