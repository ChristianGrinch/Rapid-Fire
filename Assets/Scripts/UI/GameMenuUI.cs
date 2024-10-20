using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenuUI : MonoBehaviour
{
    public static GameMenuUI Instance { get; private set; }
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
    [Header("Menu")]
    public GameObject game;
    [Header("Text")]
    public TMP_Text difficulty;
    public TMP_Text health;
    public TMP_Text lives;
    public TMP_Text wave;
    public TMP_Text ammo;

    // References
    private GameObject player;
    private HealthSystem healthSystem;
    private GunController gunController;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        healthSystem = player.GetComponent<HealthSystem>();
        gunController = player.GetComponent<GunController>();
    }
    private void Update()
    {
        wave.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
        health.text = $"Health: {healthSystem.health}";
        lives.text = $"Lives: {healthSystem.lives}";
        ammo.text = $"{gunController.ammo[gunController.currentGunInt]}";
    }
    public void SetDifficultyText()
    {
        switch (GameManager.Instance.difficulty)
        {
            case 1:
                difficulty.text = "Easy";
                break;
            case 2:
                difficulty.text = "Normal";
                break;
            case 3:
                difficulty.text = "Master";
                break;
        }
    }
}
