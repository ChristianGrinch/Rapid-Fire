using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
	[Header("Menus")]
	public GameObject mainMenu;
	public GameObject difficultyMenu;
	public GameObject settingsMenu;

	[Header("Main Menu")]
	public TMP_Text playDefaultText;
	public GameObject difficultySelectWarning;

	[Header("Settings - Audio Panel")]
	public GameObject audioPanel;
	public Slider masterVolumeSlider;
	public TextMeshProUGUI masterVolume;
	public Slider musicVolumeSlider;
	public TextMeshProUGUI musicVolume;
	public Slider gunVolumeSlider;
	public TextMeshProUGUI gunVolume;

	[Header("Settings - Video Panel")]
	public GameObject videoPanel;

	[Header("Settings - Saves Panel")]
	public GameObject savesPanel;
	public GameObject saveButtonPrefab;
	public Transform contentPanel;
	public TMP_InputField saveNameInputField;
	public Button defaultSaveButton;
	public TMP_Text loadWarning;
	public Button loadSaveButton;
	public GameObject emptySaveWarning;
    public string currentSave;
    private List<string> savedGames = new List<string>();
    private List<GameObject> saveButtons = new();
    public void CloseAllMenus()
    {
		Canvas canvas = Canvas.FindFirstObjectByType<Canvas>();
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
}
