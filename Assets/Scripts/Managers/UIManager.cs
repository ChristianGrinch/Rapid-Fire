using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance { get; private set; }
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
    private HealthSystem healthSystem;
	public GameObject player;
	public bool isGameUnpaused = false;
	public bool isInGame = false;

	void Start()
	{
		SwitchToStart();
		SavesPanelUI.Instance.InstantiateSaveButtons();
		AudioPanelUI.Instance.InitializeVolume();

		healthSystem = player.GetComponent<HealthSystem>();
	}
	void Update()
	{
		isGameUnpaused = GameManager.Instance.isGameUnpaused;
		isInGame = GameManager.Instance.isInGame;

		if (healthSystem.lives <= 0)
		{
			GameManager.Instance.GameOver();
            Time.timeScale = 0;
        }

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
			SwitchToStart();	
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
	public void OpenDifficultyScreen()
	{
		if (!GameManager.Instance.didSelectDifficulty)
		{
			StartMenuUI.Instance.startMenu.SetActive(false);
			DifficultyMenuUI.Instance.difficultyMenu.SetActive(true);
		}
		else
		{
			StartCoroutine(StartMenuUI.Instance.SaveNameWarning());
		}
	}
	public void SwitchToStart()
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
	public void OpenSettings()
	{
		CloseAllMenus();
		SettingsMenuUI.Instance.settingsMenu.SetActive(true);
        OpenAudioPanel(); // Sets Audio Panel to "default" opened save, so that the save panel isn't open while in game.
    }
	public void OpenAudioPanel()
	{
		AudioPanelUI.Instance.audioPanel.SetActive(true);
        VideoPanelUI.Instance.videoPanel.SetActive(false);
        SavesPanelUI.Instance.savesPanel.SetActive(false);
	}
	public void OpenVideoPanel()
	{
        AudioPanelUI.Instance.audioPanel.SetActive(false);
		VideoPanelUI.Instance.videoPanel.SetActive(true);
        SavesPanelUI.Instance.savesPanel.SetActive(false);
	}
	public void OpenSavesPanel()
	{
		if(!isInGame)
		{
            AudioPanelUI.Instance.audioPanel.SetActive(false);
            VideoPanelUI.Instance.videoPanel.SetActive(false);
			SavesPanelUI.Instance.savesPanel.SetActive(true);
		}
	}
}
