using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum InterfaceElements
{
	// Start Scene
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
	public InterfaceElements interfaceEl;
	public GameObject gameObject;
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
	public void InitializeInterfaces()
	{
		InitializeInterfaceGameobjects();
		int i = 0;
		var enumValues = Enum.GetValues(typeof(InterfaceElements)); // Cache it so it isnt called a million times in the loop
		if(interfaces.Count == 0)
		{
			for (var j = 0; j < Enum.GetNames(typeof(InterfaceElements)).Length; j++)
			{
				interfaces.Add(new Interface());
			}
		}
		Debug.Log(interfaces.Count);
		foreach (var interfaceEl in interfaces)
		{
			interfaceEl.interfaceEl = (InterfaceElements)enumValues.GetValue(i);
			try 
			{
				Debug.Log("Ran try");
				interfaceEl.gameObject = i < menuGameobjects.Count ? menuGameobjects[i] : panelGameObjects[i - menuGameobjects.Count];
			}
			catch (Exception ex)
			{
				Debug.Log("Caught an error: " + ex.Message);
			}
			i++;
		}
		if (navigationHistory.Contains(InterfaceElements.Start))
		{
			navigationHistory.Clear();
			navigationHistory.Add(InterfaceElements.Game);
		}
		else
		{
			navigationHistory.Add(InterfaceElements.Start);
		}
	}
	public void InitializeInterfaceGameobjects()
	{
		while (menuGameobjects.Count < 8) menuGameobjects.Add(null);
		menuGameobjects[0] = StartMenuUI.Instance.startMenu;
		menuGameobjects[1] = DifficultyMenuUI.Instance.difficultyMenu;
		menuGameobjects[2] = SettingsMenuUI.Instance.settingsMenu;

		while (panelGameObjects.Count < 4) panelGameObjects.Add(null);
		panelGameObjects[0] = AudioPanelUI.Instance.audioPanel;
		panelGameObjects[1] = VideoPanelUI.Instance.videoPanel;
		panelGameObjects[2] = SavesPanelUI.Instance.savesPanel;
		panelGameObjects[3] = ControlsPanelUI.Instance.controlsPanel;
	}
	public bool IsInterfaceOpen(InterfaceElements interfaceEl)
	{
		return navigationHistory.Contains(interfaceEl);
	}
	public void OpenInterface(InterfaceElements interfaceEl)
	{
		if (!navigationHistory.Contains(interfaceEl)) // Only open if the interface isn't already open
		{
			navigationHistory.Add(interfaceEl);

			Interface interfaceToOpen = null;
			foreach (var interfaceObj in interfaces)
			{
				if (interfaceObj.interfaceEl == interfaceEl)
				{
					interfaceToOpen = interfaceObj;
					break;
				}
			}
			interfaceToOpen.gameObject.SetActive(true);
		}
	}
	public void CloseInterface(InterfaceElements interfaceEl)
	{
		if (navigationHistory.Contains(interfaceEl))
		{
			navigationHistory.Remove(interfaceEl);

			Interface interfaceToClose = null;
			foreach (var interfaceObj in interfaces)
			{
				if (interfaceObj.interfaceEl == interfaceEl)
				{
					interfaceToClose = interfaceObj;
					break;
				}
			}
			interfaceToClose.gameObject.SetActive(false);

			if (interfaceEl == InterfaceElements.Settings)
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
	public bool IsGamePaused()
	{
		return IsInterfaceOpen(InterfaceElements.Pause);
	}
}
