using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum InterfaceElements
{
	// Start Scene
	None,
	Start,
	Difficulty,
	Settings,
	// Game Scene
	Game,
	Shop,
	Restart,
	Pause,
	Inventory,
	// Settings : Panels
	Audio,
	Video,
	Saves,
	Controls
}
[Serializable]
public class Interface
{
	public InterfaceElements interfaceEl = InterfaceElements.None;
	public GameObject gameObject = new();
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

	public List<GameObject> menuGameobjects;
	public List<GameObject> panelGameObjects;
	public List<Interface> interfaces = new(Enum.GetNames(typeof(InterfaceElements)).Length); // Number of interfaces (list) is set to the number of InterfaceElements (enum)
	public List<InterfaceElements> navigationHistory;

	void Start()
	{
		SavesPanelUI.Instance.InstantiateSaveButtons();
		AudioPanelUI.Instance.InitializeVolume();
		InitializeInterfaces();
	}
	void Update()
	{
		isGamePaused = GameManager.Instance.isGamePaused;
		isInGame = GameManager.Instance.isInGame;

		if (Input.GetKeyDown(KeyCode.Escape)) GoBackCheck();
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (IsInterfaceOpen(InterfaceElements.Shop))
			{
				ShopUI.Instance.CloseShop();
			}
			else
			{
				ShopUI.Instance.OpenShop();
			}
		}
	}
	public void InitializeInterfaces()
	{
		int i = 0;
		var enumValues = Enum.GetValues(typeof(InterfaceElements)); // Cache it so it isnt called a million times in the loop
		foreach (var interfaceEl in interfaces)
		{
			interfaceEl.interfaceEl = (InterfaceElements)enumValues.GetValue(i);
			i++;
		}
	}
	public bool IsInterfaceOpen(InterfaceElements interfaceEl)
	{
		return navigationHistory.Contains(interfaceEl);
	}
	public void OpenInterface(InterfaceElements interfaceEl)
	{
		if (!navigationHistory.Contains(interfaceEl))
		{
			navigationHistory.Add(interfaceEl);

			Interface interfaceToInstantiate = null;
			foreach (var interfaceObj in interfaces)
			{
				if (interfaceObj.interfaceEl == interfaceEl)
				{
					interfaceToInstantiate = interfaceObj;
					break;
				}
			}
			Instantiate(interfaceToInstantiate.gameObject);
		}
	}
	public void CloseInterface(InterfaceElements interfaceEl)
	{
		if (navigationHistory.Contains(interfaceEl))
		{
			navigationHistory.Remove(interfaceEl);

			if(interfaceEl == InterfaceElements.Settings)
			{
				CloseAllSettingsPanels();
			}
		}
	}
	public void CloseAllSettingsPanels()
	{
		navigationHistory.Remove(InterfaceElements.Audio);
		navigationHistory.Remove(InterfaceElements.Video);
		navigationHistory.Remove(InterfaceElements.Saves);
		navigationHistory.Remove(InterfaceElements.Controls);
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
				if (isGamePaused && IsInterfaceOpen(InterfaceElements.Shop))
				{
					ShopUI.Instance.CloseShop();
					return;
				}
				if (isGamePaused && IsInterfaceOpen(InterfaceElements.Shop))
				{
					GameManager.Instance.ResumeGame();
					return;
				}
				
			}
		}

		if (isGamePaused && IsInterfaceOpen(InterfaceElements.Settings))
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
					OpenInterface(InterfaceElements.Pause);
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

		//foreach(var menu in menusStatus.ToList()) // Unsure if this works or not, but hopefully it does
		//{
		//	menusStatus[menu.Key] = false;
		//}
		navigationHistory = new();
	}
	public void OpenDifficultyScreen()
	{
		if (!GameManager.Instance.didSelectDifficulty)
		{
			OpenInterface(InterfaceElements.Difficulty);
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
			if (IsInterfaceOpen(InterfaceElements.Settings))
			{
				// Game > Pause > Settings
				CloseInterface(InterfaceElements.Settings);
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
			OpenInterface(InterfaceElements.Start);
		}
	}
	public void OpenSettings()
	{
		OpenInterface(InterfaceElements.Settings);
		OpenInterface(InterfaceElements.Audio);
		SettingsMenuUI.Instance.OpenAudioPanel(); // Sets Audio Panel to "default" opened save, so that the save panel isn't open while in game.
	}
}
