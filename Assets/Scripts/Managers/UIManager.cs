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

	// Menus
	public GameObject difficultyMenu;
	public GameObject settingsMenu;
	public GameObject restartMenu;

	// Active game 'menu'
	public GameObject game;
	public TextMeshProUGUI waveText;
	public TextMeshProUGUI healthText;
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI ammoText;
	public TMP_Text difficultyText;

	// Start Menu
	public GameObject startMenu;
	public TMP_Text playDefaultText;
	public GameObject difficultySelectWarning;

	// Pause Menu
	public GameObject pauseMenu;
	public Button saveButton;

	// Settings - Audio panel
	public GameObject audioPanel;
	public Slider masterVolumeSlider;
	public TextMeshProUGUI masterVolume;
	public Slider musicVolumeSlider;
	public TextMeshProUGUI musicVolume;
	public Slider gunVolumeSlider;
	public TextMeshProUGUI gunVolume;

	// Settings - Video panel
	public GameObject videoPanel;
	public TMP_Dropdown screenModeDropdown;

	// Settings - Saves panel
	public GameObject savesPanel;
	public GameObject saveButtonPrefab;
	public Transform contentPanel;
	private List<string> savedGames = new List<string>();
	List<GameObject> saveButtons = new();
	public TMP_InputField saveNameInputField;
	public string currentSave;
	public Button defaultSaveButton;
	public TMP_Text loadWarning;
	public Button idontevenknow;
	public GameObject loadSaveWarning;


	// Refactored popup stuff
	public GameObject saveNameWarning;
	public GameObject createSaveWarning;
	public Button createSave_StartMenu;
	public Button quitGame_PauseMenu;
	public Button startReturn_PauseMenu;
	public Button deleteSave_SavesMenu;
	public Button loadSave_SavesMenu;


	// Important script/object references
	private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private EnemySpawnManager enemySpawnManager;
	public GameObject player;
	public GameObject gameManager;

	// crap idk lol
	public int difficulty = 1;
	public bool isGameUnpaused = false;
	public bool isInGame = false;
	public bool didSelectDifficulty = false;
	public bool didPlayerLoadSpawnManager = false;
	public bool didPlayerLoadPowerupManager = false;

	public int enemyLevel1;
	public int enemyLevel2;
	public int enemyLevel3;
	public int bossLevel1;

	public string defaultSave;
	public string selectedSaveName;



	void Awake()
	{
		// Singleton pattern implementation
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject); // Ensures there's only one UIManager instance
		}
	}
	void Start()
	{
		SwitchToStart();
		InstantiateSaveButtons();
		InitializeVolume();
		AddButtonListeners();

		defaultSave = SaveSystem.LoadDefaultSave();
		playDefaultText.text = "Play default save \n[ " + defaultSave + " ]";

		healthSystem = player.GetComponent<HealthSystem>();
		gunController = player.GetComponent<GunController>();
		playerController = player.GetComponent<PlayerController>();
		enemySpawnManager = gameManager.GetComponentInParent<EnemySpawnManager>();

		saveButton.onClick.AddListener(() => SavePlayer(currentSave));
	}
	void AddButtonListeners()
	{
		createSave_StartMenu.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.CreateSavePopup));
		quitGame_PauseMenu.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitGameConfirm));
		startReturn_PauseMenu.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.StartReturnConfirm));
		deleteSave_SavesMenu.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.DeleteSaveConfirm));
		loadSave_SavesMenu.onClick.AddListener(() =>
		{
			if (isInGame)
			{
				Debug.Log("Cannot load save while game is active.");
				loadWarning.gameObject.SetActive(true);
			}
			else
			{
				if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
				{
					PopupManager.Instance.ShowPopup(PopupManager.PopupType.PlaySaveConfirm);
				}
				else
				{
					StartCoroutine(ShowLoadWarning());
				}
			}
		});

	}
	void Update()
	{
		waveText.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
		healthText.text = $"Health: {healthSystem.health}";
		livesText.text = $"Lives: {healthSystem.lives}";
		ammoText.text = $"{gunController.ammo[gunController.currentGunInt]}";

		isGameUnpaused = GameManager.Instance.isGameUnpaused;
		isInGame = GameManager.Instance.isInGame;

		if (healthSystem.lives <= 0)
		{
			GameManager.Instance.GameOver();
		}

		if (Input.GetKeyDown(KeyCode.Escape) && isGameUnpaused)
		{
			GameManager.Instance.PauseGame();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && !isGameUnpaused && pauseMenu.activeSelf)
		{
            GameManager.Instance.ResumeGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape) && !isGameUnpaused && settingsMenu.activeSelf)
		{
			SwitchToStart();	
		}
	}
	//public void GameOver()
	//{
	//	restartMenu.SetActive(true);
	//	isGameUnpaused = false;
	//}
	//public void RestartGame()
	//{
	//	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	//}
	public void ShowRestartMenu(){ restartMenu.SetActive(true); }
	//public void StartGame()
	//{
	//	LoadPlayer(defaultSave);
	//	CloseAllMenus();
	//	game.SetActive(true);
	//	isGameUnpaused = true;
	//	isInGame = true;

	//	healthSystem.AssignLives();
	//	Time.timeScale = 1;
	//	SetDifficultyText();

	//}
	public void CloseAllMenus()
	{
		Canvas canvas = FindObjectOfType<Canvas>();
		int childCount = canvas.transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = canvas.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
	}
	//public void StartNewGame()
	//{
	//	CloseAllMenus();
	//	game.SetActive(true);
	//	isGameUnpaused = true;
	//	isInGame = true;

	//	healthSystem.AssignLives();
	//	Time.timeScale = 1;
	//	SetDifficultyText();
	//}
	//public void PauseGame()
	//{
	//	isGameUnpaused = false;
	//	pauseMenu.SetActive(true);
	//	Time.timeScale = 0;
	//	saveButton.GetComponentInChildren<TMP_Text>().text = $"Save current game ({currentSave})";
	//}
	//public void ResumeGame()
	//{
	//	isGameUnpaused = true;
	//	pauseMenu.SetActive(false);
	//	Time.timeScale = 1;
	//}
	public void OpenDifficultyScreen()
	{
		if (!didSelectDifficulty)
		{
			startMenu.SetActive(false);
			difficultyMenu.SetActive(true);
		}
		else
		{
			StartCoroutine(ShowWarning());
		}
	}
	public IEnumerator ShowWarning()
	{
		difficultySelectWarning.SetActive(true);
		yield return new WaitForSeconds(2);
		difficultySelectWarning.SetActive(false);

	}
	public IEnumerator ShowLoadWarning()
	{
		loadSaveWarning.SetActive(true);
		yield return new WaitForSeconds(5);
		loadSaveWarning.SetActive(false);

	}
	public IEnumerator ShowSaveNameWarning()
	{
		createSaveWarning.SetActive(true);
		yield return new WaitForSeconds(5);
		createSaveWarning.SetActive(false);

	}
	public IEnumerator ShowCreateSaveWarning()
	{
		saveNameWarning.SetActive(true);
		yield return new WaitForSeconds(5);
		saveNameWarning.SetActive(false);

	}
	public void SwitchToStart()
	{
		if (isInGame)
		{
		pauseMenu.SetActive(true);
		settingsMenu.SetActive(false);
		}
		else
		{
			CloseAllMenus();
			startMenu.SetActive(true);
		}
	}
	public void SetDifficulty(int selectedDifficulty)
	{
		difficulty = selectedDifficulty;
		Debug.Log("Difficulty set to: " + difficulty);
		didSelectDifficulty = true;
	}
//	public void QuitGame()
//	{
//	#if UNITY_EDITOR
//			UnityEditor.EditorApplication.isPlaying = false;
//#else
//		Application.Quit();
//#endif
//	}
	public void OpenSettings()
	{
		CloseAllMenus();
		settingsMenu.SetActive(true);
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
	//public void UpdateFullscreenMode()
	//{
	//	switch (screenModeDropdown.value)
	//	{
	//		case 0:
	//			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
	//			break;
	//		case 1:
	//			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
	//			break;
	//		case 2:
	//			Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
	//			break;
	//		case 3:
	//			Screen.fullScreenMode = FullScreenMode.Windowed;
	//			break;
	//		default:
	//			Screen.fullScreenMode = FullScreenMode.Windowed;
	//			break;
	//	}

	//}
	public void SetDifficultyText()
	{
		if (difficulty == 1)
		{
			difficultyText.text = "Easy";
		} 
		else if (difficulty == 2)
		{
			difficultyText.text = "Normal";
		}
		else if (difficulty == 3)
		{
			difficultyText.text = "Master";
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
			savedGames.Add(save);
			GameObject newButton = Instantiate(saveButtonPrefab, contentPanel);
			newButton.GetComponentInChildren<TMP_Text>().text = save;

			newButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				string btnSaveName = newButton.GetComponentInChildren<TMP_Text>().text;
				UpdateDeleteSaveButton();

				if (!isInGame)
				{
					GameManager.Instance.currentSave = btnSaveName;
				}
			});
			AudioManager.Instance.AssignSoundToNewButton(newButton);
			newButton.tag = "ButtonWithPop";

			if(save == "") // hacky fix for getting rid of the random empty save file
			{
				Destroy(newButton);
			}
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

		if (!string.IsNullOrEmpty(saveName) && !SaveSystem.FindSavesBool(saveName))
		{
			CreateNewSave(saveName);
		}
		else
		{
			Debug.LogWarning("Save name cannot be empty OR attempted to create a new save of an already existing name.");
			StartCoroutine(ShowSaveNameWarning());
		}
	}
	public void UpdateDeleteSaveButton()
	{
		deleteSave_SavesMenu.gameObject.SetActive(true);
		defaultSaveButton.gameObject.SetActive(true);
	}
	public void SetDefaultSave()
	{
		playDefaultText.text = "Play default save \n[ " + currentSave + " ]";
		Debug.Log("hi");
		if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
		{
			SaveSystem.SetDefaultSave(currentSave);
			Debug.Log("Set '" + currentSave + "' to default save.");
		}
	}
	public void SaveFromInsideGame()
	{
		saveButton.GetComponent<Button>().onClick.AddListener(() => SavePlayer(currentSave));
	}
	public void DeleteSave()
	{

		string saveName = saveNameInputField.text;

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
