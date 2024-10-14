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
		CloseAllMenus();
		difficultyMenu.SetActive(true);
	}
	public void OpenSettingsMenu()
	{
		CloseAllMenus();
		settingsMenu.SetActive(true);
	}
	public void QuitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
