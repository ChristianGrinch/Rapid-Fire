using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;

    private HealthSystem healthSystem;
    public GameObject player;

    public bool isGameActive;

    // Start is called before the first frame update
    void Start()
    {
        healthSystem = player.GetComponent<HealthSystem>();
        isGameActive = true;
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
}
