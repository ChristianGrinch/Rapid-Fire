using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    public TMP_Text difficultyText;
    public TMP_Text waveText;
    public TMP_Text healthText;
    public TMP_Text livesText;
    public TMP_Text ammoText;

    private HealthSystem healthSystem;
    private GunController gunController;
    private PlayerController playerController;

    private bool gotReferences;
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
        SetDifficultyText();
    }
    public void GetReferences()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            healthSystem = player.GetComponent<HealthSystem>();
            gunController = player.GetComponent<GunController>();
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Player is null!");
        }
        gotReferences = true;
    }
    private void Update()
    {
        if (gotReferences)
        {
            waveText.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
            healthText.text = $"Health: {healthSystem.health}";
            livesText.text = $"Lives: {healthSystem.lives}";
            ammoText.text = $"{gunController.ammo[gunController.currentGunInt]}";

            if (healthSystem.lives <= 0)
            {
                GameManager.Instance.GameOver();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isGameUnpaused)
        {
            GameManager.Instance.PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isGameUnpaused && GameUIManager.Instance.pauseMenu.activeSelf)
        {
            GameManager.Instance.ResumeGame();
        }
    }
    public void SetDifficultyText()
    {
        if (GameManager.Instance.difficulty == 1)
        {
            difficultyText.text = "Easy";
        }
        else if (GameManager.Instance.difficulty == 2)
        {
            difficultyText.text = "Normal";
        }
        else if (GameManager.Instance.difficulty == 3)
        {
            difficultyText.text = "Master";
        }
    }
}
