using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum Menus
{	
	// Sorted by where they first appear (ex: settings in start scene because the first time settings is encountered is in the settings scene)
	// Start Scene
	Start,
	Difficulty,
	Settings,
	// Game Scene
	Game,
	Shop,
	Restart,
	Pause
}
public enum Panels
{
	// Settings
	Audio,
	Video,
	Saves,
	Controls
}
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
	public bool isGamePaused = false;
	public bool isInGame = false;

	public Dictionary<Menus, bool> menusStatus = new();
	public Dictionary<Panels, bool> panelsStatus = new();

	void Start()
	{
		SwitchToStart();
		SavesPanelUI.Instance.InstantiateSaveButtons();
		AudioPanelUI.Instance.InitializeVolume();

		foreach (Menus menu in System.Enum.GetValues(typeof(Menus)))
		{
			menusStatus[menu] = false;
		}

		foreach (Panels panel in System.Enum.GetValues(typeof(Panels)))
		{
			panelsStatus[panel] = false;
		}
	}
	void Update()
	{
		isGamePaused = GameManager.Instance.isGamePaused;
		isInGame = GameManager.Instance.isInGame;

		if (Input.GetKeyDown(KeyCode.Escape)) GoBackCheck();
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (IsMenuOpen(Menus.Shop))
			{
				ShopUI.Instance.CloseShop();
			}
			else
			{
				ShopUI.Instance.OpenShop();
			}
		}
	}
	public void SetMenuStatus(Menus menu, bool menuStatus)
	{
		if (menusStatus.ContainsKey(menu))
		{
			menusStatus[menu] = menuStatus;
		}
	}
	public void SetPanelStatus(Panels panel, bool panelStatus)
	{
		if (panelsStatus.ContainsKey(panel))
		{
			panelsStatus[panel] = panelStatus;
		}
	}
	public bool IsMenuOpen(Menus menu)
	{
		if (menusStatus.ContainsKey(menu))
		{
			return menusStatus[menu];
		}
		return false;
	}
	public bool IsPanelOpen(Panels panel)
	{
		if (panelsStatus.ContainsKey(panel))
		{
			return panelsStatus[panel];
		}
		return false;
	}
	public void GoBackCheck()
	{
		if (!isGamePaused)
		{
			GameManager.Instance.PauseGame();
			return;
		}

		if(GameManager.GetActiveScene() == 1) // This needs to be here to prevent it from throwing an error since the PauseMenuUI doesn't exist yet
		{
			if (!PopupManager.Instance.isPopupOpen)
			{
				if (isGamePaused && ShopUI.Instance.shopMenu.activeSelf)
				{
					ShopUI.Instance.CloseShop();
					return;
				}
				if (isGamePaused && PauseMenuUI.Instance.pauseMenu.activeSelf)
				{
					GameManager.Instance.ResumeGame();
					return;
				}
				
			}
		}

		if (isGamePaused && SettingsMenuUI.Instance.settingsMenu.activeSelf)
		{
			if (isInGame)
			{
				if (SettingsMenuUI.Instance.didModifySettings && !SettingsMenuUI.Instance.didSaveSettings)
				{
					PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitWithoutSavingConfirm);
					return;
				}
				else
				{
					CloseAllMenus();
					PauseMenuUI.Instance.pauseMenu.SetActive(true);
				}
			}
			else
			{
				if (SettingsMenuUI.Instance.didModifySettings && !SettingsMenuUI.Instance.didSaveSettings)
				{
					PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitWithoutSavingConfirm);
					return;
				}
				else
				{
					SwitchToStart();
				}
			}
		}
	}
	public void CloseAllMenus()
	{
		Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		int childCount = canvas.transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = canvas.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
		GameObject.Find("Settings Canvas").GetComponent<Canvas>().transform.GetChild(0).gameObject.SetActive(false); // Also closes settings menu since its in a separate canvas
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
		if(GameManager.GetActiveScene() == 1)
		{
			if (SettingsMenuUI.Instance.settingsMenu.activeSelf)
			{
				PauseMenuUI.Instance.pauseMenu.SetActive(true);
				SettingsMenuUI.Instance.settingsMenu.SetActive(false);
			}
			else
			{
				SceneManager.sceneLoaded += GameManager.Instance.OnSceneLoaded;
				SceneManager.LoadScene(0);
				CloseAllMenus();
				GameManager.Instance.isInGame = false;
			}
		}
		else
		{
			CloseAllMenus();
			StartMenuUI.Instance.startMenu.SetActive(true);
		}
		
	}
	public void OpenSettings()
	{
		CloseAllMenus();
		SettingsMenuUI.Instance.settingsMenu.SetActive(true);
		SettingsMenuUI.Instance.OpenAudioPanel(); // Sets Audio Panel to "default" opened save, so that the save panel isn't open while in game.
	}
}
