using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // Game
    public static float mapSize = 50;
    public bool isGameUnpaused = false;
    public bool isInGame = false;
    public string defaultSave;
    public string currentSave;

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
        UIManager.Instance.LoadPlayer(defaultSave);
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

}
