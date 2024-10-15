using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
	public int enemyLevel1; public int enemyLevel2; public int enemyLevel3; public int bossLevel1; public int iceZombie;
	public bool didLoadSpawnManager = false;
	public bool didLoadPowerupManager = false;

    // References
    private HealthSystem healthSystem;
	private GunController gunController;
	private PlayerController playerController;
	private GameObject player;

	public GameObject gameManager;
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
		gameManager = gameObject;

    }
    private void Update()
    {
		difficulty = SaveManager.Instance.difficulty;
    }
    public void GetReferences()
	{
        player = GameObject.FindWithTag("Player");
		if(player != null)
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
    public void SetDifficulty(int selectedDifficulty)
    {
        difficulty = selectedDifficulty;
        Debug.Log("Difficulty set to: " + difficulty);
        didSelectDifficulty = true;
    }
    public void GameOver()
	{
		GameUIManager.Instance.ShowRestartMenu();
		isGameUnpaused = false;
	}
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void StartGame()
	{
		currentSave = defaultSave;
		StartUIManager.Instance.CloseAllMenus();
		isGameUnpaused = true;
		isInGame = true;
		if(healthSystem != null)
		{
            healthSystem.AssignLives();
		}
		else
		{
			Debug.LogError("health system is null!");
		}
		
		Time.timeScale = 1;
        Debug.Log(GameUI.Instance);
        GameUI.Instance.SetDifficultyText();

    }
	public void StartNewGame()
	{
		StartUIManager.Instance.CloseAllMenus();
		GameUIManager.Instance.game.SetActive(true);
		isGameUnpaused = true;
		isInGame = true;

		healthSystem.AssignLives();
		Time.timeScale = 1;
        GameUI.Instance.SetDifficultyText();
	}
	public void PauseGame()
	{
		isGameUnpaused = false;
		GameUIManager.Instance.pauseMenu.SetActive(true);
		Time.timeScale = 0;
        PauseMenuUI.Instance.saveBtn.GetComponentInChildren<TMP_Text>().text = $"Save current game ({currentSave})";
	}
	public void ResumeGame()
	{
		isGameUnpaused = true;
        GameUIManager.Instance.pauseMenu.SetActive(false);
		Time.timeScale = 1;
	}
	public void QuitGame() // KEEP
	{
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}
}
