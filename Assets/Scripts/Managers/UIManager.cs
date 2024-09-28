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
    public Button backToTitleScreenButton;
    public Button easyButton;
    public Button mediumButton;
    public Button masterButton;

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
        healthSystem = player.GetComponent<HealthSystem>();

        playButton.onClick.AddListener(StartGame);
        difficultySelectorButton.onClick.AddListener(SwitchToMenu);
        backToTitleScreenButton.onClick.AddListener(SwitchToTitle);

        easyButton.onClick.AddListener(() => SetDifficulty(1));
        mediumButton.onClick.AddListener(() => SetDifficulty(2));
        masterButton.onClick.AddListener(() => SetDifficulty(3));
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
    }
    public void PauseGame()
    {
        isGameActive = false;
    }

    public void SwitchToMenu()
    {
        titleScreen.SetActive(false);
        difficultyScreen.SetActive(true);
    }
    public void SwitchToTitle()
    {
        titleScreen.SetActive(true);
        difficultyScreen.SetActive(false);
    }

    public void SetDifficulty(int selectedDifficulty)
    {
        difficulty = selectedDifficulty;
        Debug.Log("Difficulty set to: " + difficulty);
    }
}
