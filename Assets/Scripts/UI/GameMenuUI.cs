using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GunController;
using static SlotData;

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
	[Header("Buttons")]
	public Button shop;
    [Header("Text")]
    public TMP_Text difficulty;
    public TMP_Text health;
    public TMP_Text lives;
    public TMP_Text wave;
    public TMP_Text ammo;
	public TMP_Text version;
	[Header("Images")]
	public Image ammoImage;
	public Sprite rifleAmmo;
	public Sprite pistolAmmo;
	public Image saveIcon;
	[Header("Debug Elements")]
	public Button spawnEnemyDebug;
	public Button killEnemiesDebug;
	public TMP_InputField enemySpawnCountDebug;
	private int numberOfEnemiesDebug;
	public Button getEnemyData;

    // References
    private GameObject player;
    private HealthSystem healthSystem;
    private GunController gunController;
    private EnemySpawnManager enemySpawnManager;
    private GameManager gameManager;
    private void Start()
    {
		player = GameManager.Instance.player;
        healthSystem = player.GetComponent<HealthSystem>();
        gunController = player.GetComponent<GunController>();
        gameManager = FindFirstObjectByType<GameManager>();
        enemySpawnManager = gameManager.GetComponent<EnemySpawnManager>();
		version.text = "Version: " + Application.version;
		DebugLogic();

		shop.onClick.AddListener(() => ShopUI.Instance.OpenShop());
	}
    private void Update()
    {
		if(gunController.currentGunData.gunType == GunType.Pistol)
		{
			ammoImage.sprite = pistolAmmo;
		} 
		else if(gunController.currentGunData.gunType == GunType.AssaultRifle)
		{
			ammoImage.sprite = rifleAmmo;
		}
        wave.text = $"Wave {EnemySpawnManager.Instance.currentWave}";
        health.text = $"Health: {healthSystem.health}";
        lives.text = $"Lives: {healthSystem.lives}";
		//TODO: cyhange to primary/secondary, not guntype
		int currentAmmo;
		if (gunController.currentGunData.itemType == ItemDataType.Primary)
		{
			currentAmmo = InventoryManager.Instance.selectedGuns[0].ammo;
		}
		else
		{
			currentAmmo = InventoryManager.Instance.selectedGuns[1].ammo;
		}

		ammo.text = currentAmmo.ToString();

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
		getEnemyData.onClick.AddListener(() =>
		{
			var positionsAsFloats = SaveData.ConvertVector3ToFloat(EnemySpawnManager.Instance.GetEnemyData().Positions);
			EnemyData enemyData = EnemySpawnManager.Instance.GetEnemyData();
			for(var i = 0; i < enemyData.Positions.Count; i++)
			{
				//Debug.Log(enemyData.Positions[i]);
				Debug.Log(enemyData.Types[i]);

				Debug.Log(positionsAsFloats[i][0] + " X");
				Debug.Log(positionsAsFloats[i][1] + " Y");
				Debug.Log(positionsAsFloats[i][2] + " Z");
			}
			
		});
	}
    public void KillAllEnemiesDebug()
    {
        GameObject parent = GameManager.Instance.enemies;
        int childCount = parent.transform.childCount;
        for (var i = 0; i < childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}
