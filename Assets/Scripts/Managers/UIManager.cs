using System.Collections;
using System.Collections.Generic;
using TMPro;
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

	private HealthSystem healthSystem;
	private GunController gunController;
	public GameObject player;
	public PlayerController playerController;

	public int difficulty = 1;
	public bool isGameActive = false;
	public bool didSelectDifficulty = false;

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
		} else if (Input.GetKeyDown(KeyCode.Escape) && !isGameActive)
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
	}

	public void SavePlayer()
	{
		SaveSystem.SavePlayer(playerController);
	}
	
	public void LoadPlayer()
	{
		PlayerData data = SaveSystem.LoadPlayer();

		playerController.exp = data.exp;
        playerController.health = data.health;
		

		Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
		player.transform.position = position;

		healthSystem.UpdateHealth(data.health);
		Debug.Log(data.health);
    }
}
