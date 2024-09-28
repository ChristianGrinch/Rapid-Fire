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
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    public GameObject titleScreen;
    public Button playButton;
    public Button difficultySelectorButton;

    public GameObject difficultyScreen;

    public GameObject pauseScreen;
    public Button toTitleScreenFromPauseMenuButton;
    public Button quitGameButton;

    public GameObject quitGamePopup;


    private HealthSystem healthSystem;
    public GameObject player;

    public int difficulty = 1;
    public bool isGameActive = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
        healthText.text = $"Health: {healthSystem.health}";

        if (healthSystem.health <= 0)
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
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
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
        titleScreen.SetActive(false);
        difficultyScreen.SetActive(true);
    }
    public void SwitchToTitle()
    {
        titleScreen.SetActive(true);
        difficultyScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    public void SetDifficulty(int selectedDifficulty)
    {
        difficulty = selectedDifficulty;
        Debug.Log("Difficulty set to: " + difficulty);
    }
    public void OpenPopup()
    {
        quitGamePopup.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void ClosePopup()
    {
        quitGamePopup.SetActive(false);
        pauseScreen.SetActive(true);
    }
    public void QuitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
