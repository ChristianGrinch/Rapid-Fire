using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
	public static StartUIManager Instance { get; private set; }

	[Header("Menus")]
	public GameObject mainMenu;
	public GameObject difficultyMenu;
	public GameObject settingsMenu;

    [Header("Settings Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject savesPanel;

    //public bool isIngame = false;
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
	private void Start()
	{
		CloseAllMenus();
		OpenMainMenu();
		SettingsMenuUI.Instance.InstantiateSaveButtons();
	}
	public void CloseAllMenus()
	{
		Canvas canvas = FindFirstObjectByType<Canvas>();
		int childCount = canvas.transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = canvas.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
	}
	public void OpenMainMenu()
	{
        CloseAllMenus();
        mainMenu.SetActive(true);
    }
	public void OpenDifficultyMenu()
	{
        if (!GameManager.Instance.didSelectDifficulty)
        {
            CloseAllMenus();
            difficultyMenu.SetActive(true);
        }
        else
        {
            StartCoroutine(MainMenuUI.Instance.ShowWarning());
        }
    }
    public void OpenSettingsMenu()
	{
		CloseAllMenus();
		settingsMenu.SetActive(true);
	}
    public void OpenAudioPanel()
    {
        audioPanel.SetActive(true);
        videoPanel.SetActive(false);
        savesPanel.SetActive(false);
    }
    public void OpenVideoPanel()
    {
        audioPanel.SetActive(false);
        videoPanel.SetActive(true);
        savesPanel.SetActive(false);
    }
    public void OpenSavesPanel()
    {
        if (Time.timeScale == 1)
        {
            audioPanel.SetActive(false);
            videoPanel.SetActive(false);
            savesPanel.SetActive(true);
        }
    }
}
