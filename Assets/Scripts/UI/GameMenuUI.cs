using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
	public TMP_Text version;
	[Header("Debug Elements")]
	public Button spawnEnemyDebug;
	public Button killEnemiesDebug;
	public TMP_InputField enemySpawnCountDebug;
	private int numberOfEnemiesDebug;

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
		version.text = "Version: " + Application.version;
		DebugLogic();
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
	public void DebugLogic()
	{
		spawnEnemyDebug.onClick.AddListener(() =>
		{
			for(var i = 0; i < numberOfEnemiesDebug; i++)
			{
				EnemySpawnManager.Instance.InstantiateEnemyDebug();
			}
		});
		killEnemiesDebug.onClick.AddListener(KillAllEnemiesDebug);
		enemySpawnCountDebug.onEndEdit.AddListener((string str) => { Debug.Log(str); numberOfEnemiesDebug = int.Parse(str); });
	}
    public void KillAllEnemiesDebug()
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
