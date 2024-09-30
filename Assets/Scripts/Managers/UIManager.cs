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

	public GameObject settingsScreen;
	public Button toTitleScreenFromSettingsButton;

	public GameObject audioPanel;
	public GameObject lowerMasterVolume;
	public GameObject increaseMasterVolume;
	public Slider masterVolumeSlider;
	public TextMeshProUGUI masterVolume;

	public GameObject videoPanel;

	public GameObject savesPanel;
	public TMP_InputField[] saveNameInputField = new TMP_InputField[3];

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

	private string[] saveName = {"Save1", "Save2", "Save3"};
	public bool[] hasSavedGame = new bool[3];


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

		healthSystem = player.GetComponent<HealthSystem>();
		gunController = player.GetComponent<GunController>();
		playerController = player.GetComponent<PlayerController>();
		enemySpawnManager = gameManager.GetComponentInParent<EnemySpawnManager>();

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
		if(Time.timeScale == 1)
		{
            titleScreen.SetActive(true);
            difficultyScreen.SetActive(false);
            pauseScreen.SetActive(false);
            settingsScreen.SetActive(false);
		}
		else
		{
			pauseScreen.SetActive(true);
			settingsScreen.SetActive(false);
		}

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

	public void ChangeSaveName(int save)
	{
		StartCoroutine(DelayedSaveNameChange(save));
		hasSavedGame[save - 1] = true;
	}
    private IEnumerator DelayedSaveNameChange(int save)
    {
        yield return new WaitForEndOfFrame();
        saveName[save - 1] = saveNameInputField[save - 1].text;
		saveNameInputField[save - 1].interactable = false;
    }

    public void SavePlayer(int save)
	{
		SaveSystem.SavePlayer(playerController, saveName[save - 1]);

	}
	
	public void LoadPlayer(int save)
	{
		didPlayerLoadSpawnManager = true;
		didPlayerLoadPowerupManager = true;

        // Load the player data
        SaveData data = SaveSystem.LoadPlayer(saveName[save - 1]);

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

			saveNameInputField[0].interactable = data.hasSavedGame[0];
            saveNameInputField[1].interactable = data.hasSavedGame[1];
            saveNameInputField[2].interactable = data.hasSavedGame[2];

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
