using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }

	public GameObject game;
	public TextMeshProUGUI waveText;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI ammoText;
	public Button saveButton;

	public GameObject restartScreen;
	public TextMeshProUGUI gameOverText;
	public Button restartButton;

	public GameObject titleScreen;
	public Button playButton;
	public Button difficultySelectorButton;
	public GameObject difficultySelectWarning;
	public Button settingsButton;

	public GameObject difficultyScreen;

	public GameObject pauseScreen;
	public Button toTitleScreenFromPauseMenuButton;
	public Button quitGameButton;

	public GameObject quitGamePopup;
	public GameObject returnToTitleScreenPopup;
	public GameObject deleteSavePopup;

	public GameObject settingsScreen;
	public Button toTitleScreenFromSettingsButton;

	public GameObject audioPanel;
	public Slider masterVolumeSlider;
	public TextMeshProUGUI masterVolume;
	public Slider musicVolumeSlider;
	public TextMeshProUGUI musicVolume;
	public Slider gunVolumeSlider;
	public TextMeshProUGUI gunVolume;


    public GameObject videoPanel;
	public TMP_Dropdown screenModeDropdown;

	public GameObject savesPanel;
	public GameObject saveButtonPrefab;
	public Transform contentPanel;
	private List<string> savedGames = new List<string>();
	List<GameObject> saveButtons = new();
    public TMP_InputField saveNameInputField;
	public Button deleteSaveButton;
	public string currentSave;
	public Button defaultSaveButton;

	private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	public GameObject player;
	public GameObject gameManager;



	public int difficulty = 1;
	public bool isGameActive = false;
	public bool didSelectDifficulty = false;
	public bool didPlayerLoadSpawnManager = false;
	public bool didPlayerLoadPowerupManager = false;

	public int enemyLevel1;
	public int enemyLevel2;
	public int enemyLevel3;
	public int bossLevel1;

	public string defaultSave;


	void Awake()
	{
		// Singleton pattern implementation
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject); // Ensures there's only one UIManager instance
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		SwitchToTitle();
		InstantiateSaveButtons();
		InitializeVolume();
		defaultSave = SaveSystem.LoadDefaultSave();

        healthSystem = player.GetComponent<HealthSystem>();
		gunController = player.GetComponent<GunController>();
		playerController = player.GetComponent<PlayerController>();
		enemySpawnManager = gameManager.GetComponentInParent<EnemySpawnManager>();

        saveButton.GetComponent<Button>().onClick.AddListener(() => SavePlayer(currentSave));
    }

	// Update is called once per frame
	void Update()
	{
		waveText.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
		healthText.text = $"Health: {healthSystem.health}";
		livesText.text = $"Lives: {healthSystem.lives}";
		ammoText.text = $"Ammo: {gunController.ammo[gunController.currentGunInt]}";

		if (healthSystem.lives <= 0)
		{
			GameOver();
		}
		if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
		{
			PauseGame();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && !isGameActive && pauseScreen.activeSelf)
		{
			ResumeGame();
		}
	}
	public void GameOver()
	{
		restartScreen.SetActive(true);
		isGameActive = false;
	}
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void StartGame()
	{
        LoadPlayer(defaultSave);
        titleScreen.SetActive(false);
		game.SetActive(true);
		isGameActive = true;

		healthSystem.AssignLives();
		Time.timeScale = 1;
	}
	public void PauseGame()
	{
		isGameActive = false;
		pauseScreen.SetActive(true);
		Time.timeScale = 0;
	}
	public void ResumeGame()
	{
		isGameActive = true;
		pauseScreen.SetActive(false);
		Time.timeScale = 1;
	}
	public void OpenDifficultyScreen()
	{
		if (!didSelectDifficulty)
		{
			titleScreen.SetActive(false);
			difficultyScreen.SetActive(true);
		}
		else
		{
			StartCoroutine(ShowWarning());
		}
	}
	private IEnumerator ShowWarning()
	{
		difficultySelectWarning.SetActive(true);
		yield return new WaitForSeconds(2);
		difficultySelectWarning.SetActive(false);

	}
	public void SwitchToTitle()
	{

			titleScreen.SetActive(true);
			difficultyScreen.SetActive(false);
			pauseScreen.SetActive(false);
			settingsScreen.SetActive(false);
	}
	public void SetDifficulty(int selectedDifficulty)
	{
		difficulty = selectedDifficulty;
		Debug.Log("Difficulty set to: " + difficulty);
		didSelectDifficulty = true;
	}
	public void OpenPopup()
	{
		quitGamePopup.SetActive(true);
		pauseScreen.SetActive(false);
	}
	public void ClosePopup()
	{
		quitGamePopup.SetActive(false);
		returnToTitleScreenPopup.SetActive(false);
		pauseScreen.SetActive(true);
	}
	public void OpenPopupToTitleScreen()
	{
		returnToTitleScreenPopup.SetActive(true);
		pauseScreen.SetActive(false);
	}
	public void ClosePopupToTitleScreen()
	{
		RestartGame();
	}
	public void QuitGame()
	{
	#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	public void OpenSettings()
	{
		settingsScreen.SetActive(true);
		titleScreen.SetActive(false);
		pauseScreen.SetActive(false);
	}
	public void OpenAudioPanel()
	{
		audioPanel.SetActive(true);
		videoPanel.SetActive(false);
		savesPanel.SetActive(false);
	}
	public void OpenVideoPanel()
	{
		audioPanel.SetActive(false);
		videoPanel.SetActive(true);
		savesPanel.SetActive(false);
	}
	public void OpenSavesPanel()
	{
		if(Time.timeScale == 1)
		{
			audioPanel.SetActive(false);
			videoPanel.SetActive(false);
			savesPanel.SetActive(true);
		}
	}
	public void InitializeVolume()
	{
        masterVolumeSlider.value = 50;
        musicVolumeSlider.value = 50;
        gunVolumeSlider.value = 35;
        masterVolume.text = masterVolumeSlider.value.ToString();
        musicVolume.text = musicVolumeSlider.value.ToString();
        gunVolume.text = gunVolumeSlider.value.ToString();
    }
	public void DecreaseMasterVolume()
	{
		masterVolumeSlider.value--;
		masterVolume.text = masterVolumeSlider.value.ToString();

	}
	public void IncreaseMasterVolume()
	{
		masterVolumeSlider.value++;
		masterVolume.text = masterVolumeSlider.value.ToString();
	}
    public void DecreaseMusicVolume()
    {
        musicVolumeSlider.value--;
        musicVolume.text = musicVolumeSlider.value.ToString();

    }
    public void IncreaseMusicVolume()
    {
        musicVolumeSlider.value++;
        musicVolume.text = musicVolumeSlider.value.ToString();
    }
    public void DecreaseGunVolume()
    {
        gunVolumeSlider.value--;
        gunVolume.text = gunVolumeSlider.value.ToString();

    }
    public void IncreaseGunVolume()
    {
        gunVolumeSlider.value++;
        gunVolume.text = gunVolumeSlider.value.ToString();
    }
    public void UpdateMasterSlider() { masterVolume.text = masterVolumeSlider.value.ToString(); }
	public void UpdateMusicSlider() { musicVolume.text = musicVolumeSlider.value.ToString(); }
    public void UpdateGunSlider() { gunVolume.text = gunVolumeSlider.value.ToString(); }
	public void UpdateFullscreenMode()
	{
        switch (screenModeDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
			case 2:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
			case 3:
                Screen.fullScreenMode = FullScreenMode.Windowed;
				break;
			default:
				Screen.fullScreenMode = FullScreenMode.Windowed;
				break;
        }

    }
    public void InstantiateSaveButtons()
	{
		List<string> saveFiles = SaveSystem.FindSaves();

        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (saveFiles.Count == 0)
		{
			Debug.Log("No save files to load.");
			return;
		}

		foreach (var save in saveFiles)
		{
            string saveName = save;
            savedGames.Add(saveName);
            GameObject newButton = Instantiate(saveButtonPrefab, contentPanel);
			newButton.GetComponentInChildren<TMP_Text>().text = save;

			newButton.GetComponent<Button>().onClick.AddListener(() => LoadPlayer(saveName));
		}

	}
	public void DestroySaveButtons()
	{
		List<string> saveNames = SaveSystem.FindSaves();
		string saveName = saveNameInputField.text;

		for(var i = 0; i < saveNames.Count; i++)
		{
			if(saveName == saveNames[i])
			{
				Destroy(saveButtons[i]);
			}
		}	
	}
	public void CreateNewSave(string saveName)
	{
        bool saveNameInSavedGames = false;

        foreach (string savedGame in SaveSystem.FindSaves())
        {
            if (savedGame == saveName)
            {
                saveNameInSavedGames = true;
                break;
            }
        }

        if (!saveNameInSavedGames)
        {
            savedGames.Add(saveName);
            AddButton(saveName);
        }

        SavePlayer(saveName);
	}
	private void AddButton(string saveName)
	{
		GameObject newButton = Instantiate(saveButtonPrefab, contentPanel);
		newButton.GetComponentInChildren<TMP_Text>().text = saveName;

		Button btn = newButton.GetComponent<Button>();
		btn.onClick.AddListener(() => LoadPlayer(saveName));
	}
	public void OnSaveButtonClicked()
	{
		string saveName = saveNameInputField.text;

		if (!string.IsNullOrEmpty(saveName))
		{
			CreateNewSave(saveName);
		}
		else
		{
			Debug.LogWarning("Save name cannot be empty!");
		}
	}
	public void OpenPopupDeleteSave()
	{
		deleteSavePopup.SetActive(true);
	}
	public void ClosePopupDeleteSave()
	{
		deleteSavePopup.SetActive(false);
	}
	public void UpdateDeleteSaveButton()
	{
		string saveName = saveNameInputField.text;

		if (!string.IsNullOrEmpty(saveName) && SaveSystem.FindSaves().Contains(saveName))
        {
			deleteSaveButton.gameObject.SetActive(true);
			defaultSaveButton.gameObject.SetActive(true);
		}
		else
		{
			deleteSaveButton.gameObject.SetActive(false);
            defaultSaveButton.gameObject.SetActive(false);
        }

	}
	public void SetDefaultSave()
	{
        string saveName = saveNameInputField.text;

        if (!string.IsNullOrEmpty(saveName) && SaveSystem.FindSaves().Contains(saveName))
		{
			SaveSystem.SetDefaultSave(saveName);
			Debug.Log("Set " + saveName + " to default save.");
        }
	}
	public void SaveFromInsideGame()
	{
		saveButton.GetComponent<Button>().onClick.AddListener(() => SavePlayer(currentSave));
	}
	public void DeleteSave()
	{

		string saveName = saveNameInputField.text;

		if (!string.IsNullOrEmpty(saveName))
		{
			SaveSystem.DeleteSave(saveNameInputField.text);
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

		didPlayerLoadSpawnManager = true;
		didPlayerLoadPowerupManager = true;

		// Load the player data
		SaveData data = SaveSystem.LoadPlayer(saveName);

		if(data != null)
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
			masterVolumeSlider.value = data.masterVolume;
			masterVolume.text = data.masterVolume.ToString();

			musicVolumeSlider.value = data.musicVolume;
			musicVolume.text = data.musicVolume.ToString();

			gunVolumeSlider.value = data.gunVolume;
			gunVolume.text = data.musicVolume.ToString();

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
