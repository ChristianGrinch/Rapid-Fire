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
    public GameObject debug;
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
    private EnemySpawnManager enemySpawnManager;
    private GameManager gameManager;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        healthSystem = player.GetComponent<HealthSystem>();
        gunController = player.GetComponent<GunController>();
        gameManager = FindFirstObjectByType<GameManager>();
        enemySpawnManager = gameManager.GetComponent<EnemySpawnManager>();
    }
    private void Update()
    {
        wave.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
        health.text = $"Health: {healthSystem.health}";
        lives.text = $"Lives: {healthSystem.lives}";
        ammo.text = $"{gunController.ammo[gunController.currentGunInt]}";
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab)) // very hard to trigger. must press all 3 at exactly the same time
        {
            if (debug.activeSelf)
            {
                debug.SetActive(false);
            }
            else
            {
                debug.SetActive(true);
            }
        }
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
    public void DEBUGKillAllEnemies()
    {
        GameObject parent = EnemySpawnManager.Instance.enemyParent;
        int childCount = parent.transform.childCount;
        for (var i = 0; i < childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
