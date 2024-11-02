using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }
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
	public bool isGameUnpaused = false;
	public bool isInGame = false;

	void Start()
	{
		SwitchToStart();
		SavesPanelUI.Instance.InstantiateSaveButtons();
		AudioPanelUI.Instance.InitializeVolume();
	}
	void Update()
	{
		isGameUnpaused = GameManager.Instance.isGameUnpaused;
		isInGame = GameManager.Instance.isInGame;

		if (Input.GetKeyDown(KeyCode.Escape) && isGameUnpaused)
		{
			GameManager.Instance.PauseGame();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && !isGameUnpaused && PauseMenuUI.Instance.pauseMenu.activeSelf)
		{
			GameManager.Instance.ResumeGame();
		}

		if (Input.GetKeyDown(KeyCode.Escape) && !isGameUnpaused && SettingsMenuUI.Instance.settingsMenu.activeSelf)
		{
			if (isInGame)
			{
				if (SettingsMenuUI.Instance.didModifySettings && !SettingsMenuUI.Instance.didSaveSettings)
				{
					PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitWithoutSavingConfirm);
				}
				//CloseAllMenus();
				//PauseMenuUI.Instance.pauseMenu.SetActive(true);
			}
			else
			{
				SwitchToStart();
			}
			
		}
	}
	public void CloseAllMenus()
	{
		Canvas canvas = FindAnyObjectByType<Canvas>();
		int childCount = canvas.transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = canvas.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
	}
	public void CloseAllSettingsPanels() // bad cuz hard coded unlike closeallmenus but i could care less right now
	{
		AudioPanelUI.Instance.audioPanel.SetActive(false);
		VideoPanelUI.Instance.videoPanel.SetActive(false);
		SavesPanelUI.Instance.savesPanel.SetActive(false);
		ControlsPanelUI.Instance.controlsPanel.SetActive(false);
	}
	public void OpenDifficultyScreen()
	{
		if (!GameManager.Instance.didSelectDifficulty)
		{
			StartMenuUI.Instance.startMenu.SetActive(false);
			DifficultyMenuUI.Instance.difficultyMenu.SetActive(true);
		}
		else
		{
			StartCoroutine(StartMenuUI.Instance.DifficultySelectWarning());
		}
	}
	public void SwitchToStart()
	{
		if(SceneManager.GetActiveScene().buildIndex == 2)
		{
			if (SettingsMenuUI.Instance.settingsMenu.activeSelf)
			{
				PauseMenuUI.Instance.pauseMenu.SetActive(true);
				SettingsMenuUI.Instance.settingsMenu.SetActive(false);
			}
			else
			{
				 Debug.Log("setting scene to start");
				SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
				SceneManager.LoadScene(0);
				CloseAllMenus();
				GameManager.Instance.isInGame = false;
				GameManager.Instance.EmptyInstantiatedObjects();
			}
		}
		else
		{
			if (isInGame)
			{
				PauseMenuUI.Instance.pauseMenu.SetActive(true);
				SettingsMenuUI.Instance.settingsMenu.SetActive(false);
			}
			else
			{
				CloseAllMenus();
				StartMenuUI.Instance.startMenu.SetActive(true);
			}
		}
		
	}
	public void OpenSettings()
	{
		CloseAllMenus();
		SettingsMenuUI.Instance.settingsMenu.SetActive(true);
        SettingsMenuUI.Instance.OpenAudioPanel(); // Sets Audio Panel to "default" opened save, so that the save panel isn't open while in game.
    }
}
